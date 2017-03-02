using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消息处理类
/// </summary>
public class HandlerManager : MonoBehaviour
{
    // 每个 commandId 对应一个或者多个 handler
    private Dictionary<int, List<IHandler>> handlerDict = new Dictionary<int, List<IHandler>>();

    void Awake()
    {
        // 注册命令
        Register(new ConnectHandler());  // 1000
        Register(new LoginHandler());  // 1001
        Register(new RegisterHandler());  // 1002
        Register(new CreateRoleHandler());  // 1003
        Register(new EnterSceneHandler());  // 1004
    }

    private void Register(IHandler handler)
    {
        if (handler == null) { return; }
        // 多次添加
        if (handlerDict.ContainsKey(handler.commandId))
        {
            handlerDict[handler.commandId].Add(handler);
        }
        // 第一次添加
        else
        {
            List<IHandler> handlers = new List<IHandler>();
            handlers.Add(handler);
            handlerDict.Add(handler.commandId, handlers);
        }
    }

    // 根据 commandId 获取 handler
    private List<IHandler> GetHandlers(int commandId)
    {
        if (handlerDict.ContainsKey(commandId)) { return handlerDict[commandId]; }
        return null;
    }

    // 处理消息
    public void Execute(MessageBody data)
    {
        // Debug.LogWarning("S2C commandId : " + data.commandId);
        // Debug.LogWarning("S2C msg : " + System.Text.Encoding.UTF8.GetString(data.msg));

        // 获取所有 handlers
        List<IHandler> handlers = GetHandlers(data.commandId);
        // 遍历所有 handlers
        foreach (var handler in handlers)
        {
            // handler 执行各自的方法
            handler.Execute(data);
        }
    }
}
