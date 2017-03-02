using UnityEngine;

public class ConnectHandler : IHandler
{
    public int commandId { get { return Consts_CommandId.S2C_Connect; } }

    public void Execute(MessageBody data)
    {
        if (data != null)
        {
            Debug.LogWarning(System.Text.Encoding.UTF8.GetString(data.msg));
        }
    }
}
