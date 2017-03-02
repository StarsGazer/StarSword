using Protos.CreateRole;
using UnityEngine;

public class CreateRoleHandler : IHandler
{
    public int commandId { get { return Consts_CommandId.S2C_CreateRole; } }

    public void Execute(MessageBody data)
    {
        CreateRoleRes res = Util.Deserialize<CreateRoleRes>(data.msg);
        if (res != null)
        {
            if (res.result == 0)
            {
                Globals.Instance.PlayerManager.SetCreateRole(res.characterId);
                Debug.Log("创建成功!!!");
                Debug.Log("characterId = " + res.characterId);

                Main.Instance.EnterScene();  // 进入场景
            }
            else if (res.result == 1)
            {
                Debug.Log("昵称已被占用!!!");
            }
            else if (res.result == 2)
            {
                Debug.Log("昵称非法!!!");
            }
            else if (res.result == 3)
            {
                Debug.Log("创建失败!!!");
            }
        }
    }
}
