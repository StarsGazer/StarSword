using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

/// <summary>
/// 多线程网络框架
/// </summary>
public class NetThread : NetBase
{
    #region  属性
    public override event OnConnectedHandler OnConnectedEvent;
    public override event OnDisConnectedHandler OnDisConnectedEvent;
    public override event OnErrorHandler OnErrorEvent;

    private Socket m_Socket;
    private IPEndPoint m_Remote;
    private Thread m_Thread;

    // Select 检查列表
    private ArrayList m_CheckRead, m_CheckSend, m_CheckError;
    // 发送数据队列
    private Queue<byte[]> m_SendBuff;
    // 接受数据队列
    private Queue<MessageData> m_Datas;
    // 上锁，防止多线程问题
    private object _lock;
    #endregion

    #region  私有方法
    // ----------初始化----------
    private void _Init()
    {
        m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // 开启 Select 线程
        m_Thread = new Thread(new ThreadStart(_LoopRun));
        m_Thread.IsBackground = true;

        m_CheckRead = new ArrayList();
        m_CheckSend = new ArrayList();
        m_CheckError = new ArrayList();

        m_SendBuff = new Queue<byte[]>();
        m_Datas = new Queue<MessageData>();

        _lock = new object();
    }

    // ----------异步连接----------
    private void _BeginConnect()
    {
        m_Socket.BeginConnect(m_Remote, _EndConnect, m_Socket);
    }

    // ----------连接回调----------
    private void _EndConnect(IAsyncResult async)
    {
        // 连接成功
        if (Connected)
        {
            // 开启 Select 线程
            m_Thread.Start();
            m_Socket.EndConnect(async);
            if (OnConnectedEvent != null)
            {
                this.OnConnectedEvent(this, new ConnectedEventArgs(m_Socket));
            }
        }
        // 连接失败
        else
        {
            // AccessDenied = 10013
            _OnError(new SocketException((int)SocketError.AccessDenied));
        }
    }

    // ----------Select 线程----------
    private void _LoopRun()
    {
        while (Connected)
        {
            // 清空检查列表
            m_CheckRead.Clear();
            m_CheckSend.Clear();
            m_CheckError.Clear();
            // 加入检查列表
            m_CheckRead.Add(m_Socket);
            m_CheckSend.Add(m_Socket);
            m_CheckError.Add(m_Socket);

            /*
                Select 非阻塞地检查多个套接字
                满足条件的套接字留在 ArrayList 类的对象
            */
            Socket.Select(m_CheckRead, null, null, 100);
            if (m_CheckRead.Count > 0)
            { _OnRead(); }

            Socket.Select(null, m_CheckSend, null, 100);
            if (m_CheckSend.Count > 0)
            { _OnSend(); }

            Socket.Select(null, null, m_CheckError, 100);
            if (m_CheckError.Count > 0)
            { _OnError(null); }
        }
    }
    #endregion

    #region  Select 调用
    // ----------读取消息----------
    private void _OnRead()
    {
        /*
            Available 是从网络接收的、可供读取的数据字节数
            这个值是缓冲区中已经接收数据的字节数，不是实际数据的字节数
        */
        if (m_Socket.Available > 0)
        {
            try
            {
                byte[] buffer = new byte[13];
                // 接收数据
                m_Socket.Receive(buffer, 13, SocketFlags.Peek);
                // 解析消息头
                MessageHead head = MessageParse.UnparseHead(buffer);
                // 解析失败
                if (head == null)
                {
                    // ProtocolOption = 10042 Option unknown, or unsupported
                    _OnError(new SocketException((int)SocketError.ProtocolOption));
                }
                // 解析成功
                else
                {
                    // 13 = 4(HEAD) + 1(ProtoVersion) + 4(ServerVersion) + 4(Length)
                    int length = head.Length + 13;
                    // 数据全部到达
                    if (length <= m_Socket.Available)
                    {
                        buffer = new byte[length];
                        // 接收数据
                        m_Socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                        // 解析消息体
                        MessageBody body = MessageParse.UnparseBody(head.Length, buffer);
                        if (body != null)
                        {
                            MessageData data = new MessageData() { head = head, body = body };
                            // 上锁，阻塞
                            lock (_lock) { m_Datas.Enqueue(data); }
                        }
                        else
                        {
                            // ProtocolOption = 10042 Option unknown, or unsupported
                            _OnError(new SocketException((int)SocketError.ProtocolOption));
                        }
                    }
                }
            }
            catch (SocketException e) { _OnError(e); }
        }
    }

    // ----------发送消息----------
    private void _OnSend()
    {
        // 上锁，阻塞
        Monitor.Enter(m_SendBuff);
        while (m_SendBuff.Count > 0 && Connected)
        {
            byte[] buffer = m_SendBuff.Dequeue();
            m_Socket.Send(buffer);
        }
        Monitor.Exit(m_SendBuff);
    }

    // ----------发生错误----------
    private void _OnError(SocketException exception)
    {
        if (OnErrorEvent != null)
        {
            this.OnErrorEvent(this, new ErrorEventArgs(exception));
        }
        Close();
    }
    #endregion

    // --------------------
    // 下面是一些外界调用的接口
    // --------------------
    #region  公有方法
    // 发起连接
    public override void Connect(string ip, int port)
    {
        _Init();
        m_Remote = new IPEndPoint(IPAddress.Parse(ip), port);
        _BeginConnect();
    }

    // 重新连接
    public override void ReConnect()
    {
        // 关闭原来的连接，防止出现多连接
        Close();
        _Init();
        _BeginConnect();
    }

    // 发送消息
    public override void Send(byte[] data)
    {
        if (data != null)
        {
            // 上锁，阻塞
            Monitor.Enter(m_SendBuff);
            m_SendBuff.Enqueue(data);
            Monitor.Exit(m_SendBuff);
        }
    }

    // 获取消息
    public override MessageData Loop()
    {
        if (m_Datas.Count > 0)
        {
            MessageData data = null;
            // 上锁，阻塞
            Monitor.Enter(m_Datas);
            // 多重判断，防止多线程问题
            if (m_Datas.Count > 0)
            {
                data = m_Datas.Dequeue();
            }
            Monitor.Exit(m_Datas);
            return data;
        }
        return null;
    }

    // 关闭连接
    public override void Close()
    {
        if (OnDisConnectedEvent != null)
        {
            this.OnDisConnectedEvent(this, new ConnectedEventArgs(m_Socket));
        }
        if (Connected)
        {
            // 线程休眠，关闭连接
            Thread.Sleep(10);
            m_Socket.Shutdown(SocketShutdown.Both);
            m_Socket.Close();
        }
        m_Socket = null;
    }

    // 判断是否连接
    public override bool Connected
    {
        get { return m_Socket != null && m_Socket.Connected; }
    }
    #endregion
}
