using Protos.Register;
using UnityEngine;

public class RegisterHandler : IHandler
{
    public int commandId { get { return Consts_CommandId.S2C_Register; } }

    public void Execute(MessageBody data)
    {
        RegisterRes res = Util.Deserialize<RegisterRes>(data.msg);
        if (res != null)
        {
            // 注册成功
            if (res.result == 0)
            {
                Debug.Log("注册成功!!!");
            }
            else if (res.result == 1)
            {
                Debug.Log("用户名已被占用!!!");
            }
            else if (res.result == 2)
            {
                Debug.Log("用户名非法!!!");
            }
        }
    }
}
