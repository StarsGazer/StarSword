using System;
using UnityEngine;

public class Globals : UnitySingleton<Globals>
{
    #region  属性
    // 服务端版本号
    public int SERVERVERSION = 0;

    // 网络管理类
    private NetManager _NetManager;
    public NetManager NetManager { get { return _NetManager; } }
    // 消息处理类
    private HandlerManager _HandlerManager;
    public HandlerManager HandlerManager { get { return _HandlerManager; } }
    // 玩家管理类
    private PlayerManager _PlayerManager;
    public PlayerManager PlayerManager { get { return _PlayerManager; } }

    public bool Connected { get { return NetManager.Connected; } }
    #endregion

    #region  Unity方法
    // 开始前
    public override void Awake()
    {
        // 继承父类
        base.Awake();

        // 加载管理类
        _Load();
    }

    // 开始时
    void Start()
    {
        // ----------发起连接----------
        Connect(null);
    }

    // 销毁时
    void OnDestroy()
    {
        NetManager.Close();
    }
    #endregion

    #region  初始化
    // 加载管理类
    void _Load()
    {
        /* 
            _NetManager = _Create<NetManager>();
            这里我们直接在视图里创建 NetManager，方便修改
        */
        _NetManager = GetComponentInChildren<NetManager>();
        _HandlerManager = _Create<HandlerManager>();
        _PlayerManager = _Create<PlayerManager>();
    }

    // 创建管理类
    T _Create<T>() where T : Component
    {
        T t = this.GetComponentInChildren<T>();
        // 不存在就创建
        if (t == null)
        {
            // 创建物体
            GameObject go = new GameObject(typeof(T).Name);
            // 创建组件
            t = go.AddComponent<T>();
            // 设置父组件
            go.transform.parent = this.transform;
        }
        return t;
    }
    #endregion

    #region  网络方法
    // 发起连接
    public void Connect(Action callback)
    {
        NetManager.Connect(callback);
    }

    // 处理消息
    public void ExecuteMsg(MessageData data)
    {
        if (data != null) { HandlerManager.Execute(data.body); }
    }

    // 发送消息
    public void SendMsg(int commandId)
    {
        // -----------只有命令，没有消息-----------
        byte[] sendBytes = MessageParse.Parse(SERVERVERSION, commandId);
        NetManager.Send(sendBytes);
    }
    public void SendMsg<T>(int commandId, T data)
    {
        // -----------Protobuf-----------
        byte[] msg = Util.Serialize<T>(data);
        byte[] sendBytes = MessageParse.Parse(SERVERVERSION, commandId, msg);
        NetManager.Send(sendBytes);
    }
    #endregion
}
