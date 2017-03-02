using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

/// <summary>
/// 异步网络框架
/// </summary>
public class NetAsyn : NetBase
{
    #region  属性
    public override event OnConnectedHandler OnConnectedEvent;
    public override event OnDisConnectedHandler OnDisConnectedEvent;
    public override event OnErrorHandler OnErrorEvent;

    private Socket socket;
    private IPEndPoint remote;

    // 缓冲区
    private byte[] recvBuf = new byte[1024 * 1024];
    // 是否正在读取数据
    private bool isRead = false;

    // 接收缓冲区
    private List<byte> buffer;
    // 消息队列
    private Queue<MessageData> datas;
    #endregion

    #region  私有函数
    // ----------初始化----------
    void _Init()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        buffer = new List<byte>();
        datas = new Queue<MessageData>();
    }

    // ----------异步连接----------
    void _BeginConnect()
    {
        socket.BeginConnect(remote, _EndConnect, socket);
    }

    // ----------连接回调----------
    void _EndConnect(IAsyncResult async)
    {
        if (Connected)
        {
            socket.EndConnect(async);
            socket.BeginReceive(recvBuf, 0, 1024 * 1024, SocketFlags.None, _RecvMsg, recvBuf);
            if (OnConnectedEvent != null)
            {
                this.OnConnectedEvent(this, new ConnectedEventArgs(socket));
            }
        }
        // 连接失败
        else
        {
            // AccessDenied = 10013
            _OnError(new SocketException((int)SocketError.AccessDenied));
        }
    }

    // ----------读取消息----------
    void _RecvMsg(IAsyncResult async)
    {
        try
        {
            // 结束异步消息读取并获取消息长度
            int count = socket.EndReceive(async);
            byte[] buff = new byte[count];
            // 复制缓冲区
            Buffer.BlockCopy(recvBuf, 0, buff, 0, count);
            // 复制到数据缓冲区
            buffer.AddRange(buff);
            // 查看是否正在读取数据
            if (!isRead)
            {
                isRead = true;
                _OnData();
            }
        }
        catch (Exception)
        {
            Debug.Log("RecvMsg Error!!!");
            Close();
        }
        socket.BeginReceive(recvBuf, 0, 1024 * 1024, SocketFlags.None, _RecvMsg, recvBuf);
    }

    // ----------递归地获取消息----------
    void _OnData()
    {
        MessageHead head = MessageParse.UnparseHead(buffer.ToArray());
        if (head == null)
        {
            isRead = false;
            return;
        }
        else
        {
            // 13 = 4(HEAD) + 1(ProtoVersion) + 4(ServerVersion) + 4(Length)
            int length = head.Length + 13;
            // 数据全部到达
            if (length <= buffer.Count)
            {
                MessageBody body = MessageParse.UnparseBody(head.Length, buffer.ToArray());
                if (body != null)
                {
                    // 构造消息
                    MessageData data = new MessageData() { head = head, body = body };
                    // 消息入队
                    datas.Enqueue(data);
                    // 移除缓冲区数据
                    buffer.RemoveRange(0, head.Length + 13);
                }
                else
                {
                    // ProtocolOption = 10042 Option unknown, or unsupported
                    _OnError(new SocketException((int)SocketError.ProtocolOption));
                }
            }
        }
        // 递归调用
        _OnData();
    }

    // ----------发生错误----------
    void _OnError(SocketException exception)
    {
        if (OnErrorEvent != null)
        {
            this.OnErrorEvent(this, new ErrorEventArgs(exception));
        }
    }
    #endregion

    // --------------------
    // 下面是一些外界调用的接口
    // --------------------
    #region  公有函数
    // 发起连接
    public override void Connect(string ip, int port)
    {
        remote = new IPEndPoint(IPAddress.Parse(ip), port);
        _Init();
        _BeginConnect();
    }

    // 重新连接
    public override void ReConnect()
    {
        Close();
        _Init();
        _BeginConnect();
    }

    // 发送消息
    public override void Send(byte[] data)
    {
        socket.Send(data);
    }

    // 获取消息
    public override MessageData Loop()
    {
        if (datas.Count > 0)
        {
            return datas.Dequeue();
        }
        return null;
    }

    // 关闭连接
    public override void Close()
    {
        if (OnDisConnectedEvent != null)
        {
            this.OnDisConnectedEvent(this, new ConnectedEventArgs(socket));
        }
        if (Connected)
        {
            socket.Close();
        }
        socket = null;
    }

    // 判断是否连接
    public override bool Connected
    {
        get { return socket != null && socket.Connected; }
    }
    #endregion
}
