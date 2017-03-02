using Protos.LoginRole;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterSceneHandler : IHandler
{
    public int commandId { get { return Consts_CommandId.S2C_LoginRole; } }

    public void Execute(MessageBody data)
    {
        LoginRoleRes res = Util.Deserialize<LoginRoleRes>(data.msg);
        if (res != null)
        {
            // 登录成功
            if (res.result == 0)
            {
                Debug.Log("进入场景成功!!!");
                Data d = res.data;
                Globals.Instance.PlayerManager.SetLoginRole(d.nickName, d.profession, d.sex,
                    d.level, d.coin, d.gold, d.life, d.attack, d.defend,
                    d.magic, d.agility, d.speed, d.fortune, d.technique,
                    d.skillPoint, d.status, d.positionX, d.positionY);
                SceneManager.LoadScene(1);
            }
            else if (res.result == 1)
            {
                Debug.Log("进入场景失败!!!");
            }
        }
    }
}
