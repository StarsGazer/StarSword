using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 网络管理类
/// </summary>
public class NetManager : MonoBehaviour
{
    #region 属性
    public string IP = "127.0.0.1";
    public int PORT = 9981;

    // 是否开启断线重连
    public bool IsReConnect = false;
    // 每次发起重连时间
    public float ReConnectTime = 10.0f;
    // 是否正在重新连接
    private bool IsReConnecting = false;

    // 多线程网络（默认）或者异步网络
    public bool ThreadOrAsyn = true;
    // 网络框架对象
    private NetBase m_Client;

    // 连接成功回调
    private Action ConnectSuccessCallback;
    #endregion

    #region 私有方法
    // 进入游戏前初始化
    void Awake()
    {
        _Init();
    }

    // 初始化网络框架
    void _Init()
    {
        if (ThreadOrAsyn)
            m_Client = new NetThread();
        else
            m_Client = new NetAsyn();
        m_Client.OnConnectedEvent += OnConnect;
        m_Client.OnDisConnectedEvent += OnDisConnect;
        m_Client.OnErrorEvent += OnError;
    }

    // 在固定帧数处理消息
    void FixedUpdate()
    {
        if (Connected)
        {
            // 处理消息
            Globals.Instance.ExecuteMsg(m_Client.Loop());
        }
        else
        {
            // 连接断开
            _LostConnect();
        }
    }

    // 断线重连，通知用户掉线
    void _LostConnect()
    {
        if (IsReConnect && !IsReConnecting)
        {
            Debug.Log("重新连接!!!");
            StartCoroutine(ReConnect());
        }
    }

    // 开启协程
    IEnumerator ReConnect()
    {
        IsReConnecting = true;
        ReConnect(null);

        // 等待重连
        yield return new WaitForSeconds(ReConnectTime);
        IsReConnecting = false;
    }
    #endregion

    #region  定义回调
    void OnConnect(object sender, ConnectedEventArgs e)
    {
        Debug.LogWarning("OnConnect");
        if (Connected)
        {
            // 回调函数
            if (ConnectSuccessCallback != null)
            {
                ConnectSuccessCallback();
                ConnectSuccessCallback = null;
            }
        }
    }

    void OnDisConnect(object sender, ConnectedEventArgs e)
    {
        Debug.LogWarning("OnDisConnect");
    }

    void OnError(object sender, ErrorEventArgs e)
    {
        Debug.LogError("OnError");
    }
    #endregion

    // --------------------
    // 下面是一些外界调用的接口
    // --------------------
    #region  公有方法
    // 重新初始化
    public void ReInit()
    {
        _Init();
    }

    // 发起连接
    public void Connect(Action callback)
    {
        ConnectSuccessCallback = callback;
        m_Client.Connect(IP, PORT);
    }

    // 重新连接
    public void ReConnect(Action callback)
    {
        ConnectSuccessCallback = callback;
        m_Client.ReConnect();
    }

    // 发送消息
    public void Send(byte[] data)
    {
        if (data != null && Connected)
        {
            m_Client.Send(data);
        }
    }

    // 关闭连接
    public void Close()
    {
        if (Connected)
        {
            m_Client.Close();
        }
    }

    // 判断是否连接
    public bool Connected
    { get { return m_Client != null && m_Client.Connected; } }
    #endregion
}
