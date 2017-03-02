using System;
using System.Net.Sockets;

#region  委托
// 定义委托
public delegate void OnConnectedHandler(object sender, ConnectedEventArgs e);
public delegate void OnDisConnectedHandler(object sender, ConnectedEventArgs e);
public delegate void OnErrorHandler(object sender, ErrorEventArgs e);

// Connected 参数
// DisConnected 参数
public class ConnectedEventArgs : EventArgs
{
    public Socket socket;
    public ConnectedEventArgs(Socket socket)
    { this.socket = socket; }
}

// Error 参数
public class ErrorEventArgs : EventArgs
{
    public SocketException exception;
    public ErrorEventArgs(SocketException exception)
    { this.exception = exception; }
}
#endregion

/// <summary>
/// 网络抽象类
/// </summary>
public abstract class NetBase
{
    // 事件
    public abstract event OnConnectedHandler OnConnectedEvent;
    public abstract event OnDisConnectedHandler OnDisConnectedEvent;
    public abstract event OnErrorHandler OnErrorEvent;

    // 发起连接
    public abstract void Connect(string ip, int port);

    // 重新连接
    public abstract void ReConnect();

    // 发送消息
    public abstract void Send(byte[] data);

    // 获取消息
    public abstract MessageData Loop();

    // 关闭连接
    public abstract void Close();

    // 判断是否连接
    public abstract bool Connected { get; }
}
