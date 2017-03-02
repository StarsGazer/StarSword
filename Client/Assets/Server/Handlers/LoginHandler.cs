using Protos.Login;
using UnityEngine;

public class LoginHandler : IHandler
{
    public int commandId { get { return Consts_CommandId.S2C_Login; } }

    public void Execute(MessageBody data)
    {
        LoginRes res = Util.Deserialize<LoginRes>(data.msg);
        if (res != null)
        {
            // 登录成功
            if (res.result == 0)
            {
                Debug.Log("登录成功!!!");
                Globals.Instance.PlayerManager.SetLogin(res.userId, res.characterId);
                if (res.characterId != 0)
                {
                    Main.Instance.EnterScene();  // 进入场景
                }
                else
                {
                    Main.Instance.ShowCreateRolePanel();  // 弹出创建角色面板
                }
            }
            else if (res.result == 1)
            {
                Debug.Log("用户名不存在!!!");
            }
            else if (res.result == 2)
            {
                Debug.Log("密码错误!!!");
            }
            else if (res.result == 3)
            {
                Debug.Log("用户不允许登录!!!");
            }
        }
    }
}
