using Protos.CreateRole;
using Protos.Login;
using Protos.LoginRole;
using Protos.Register;
// using UnityEngine;

public class SocketManager : Singleton<SocketManager>
{
    public void SendMsg_Login(string username, string password)
    {
        LoginReq req = new LoginReq() { username = username, password = Util.Md5Sum(password) };
        Globals.Instance.SendMsg<LoginReq>(Consts_CommandId.C2S_Login, req);
    }

    public void SendMsg_Register(string username, string password)
    {
        RegisterReq req = new RegisterReq() { username = username, password = Util.Md5Sum(password) };
        Globals.Instance.SendMsg<RegisterReq>(Consts_CommandId.C2S_Register, req);
    }

    public void SendMsg_CreateRole(string nickName, int profession, bool sex)
    {
        CreateRoleReq req = new CreateRoleReq() { nickName = nickName, profession = profession, sex = sex };
        Globals.Instance.SendMsg<CreateRoleReq>(Consts_CommandId.C2S_CreateRole, req);
    }

    public void SendMsg_LoginRole(int characterId)
    {
        LoginRoleReq req = new LoginRoleReq() { characterId = characterId };
        Globals.Instance.SendMsg<LoginRoleReq>(Consts_CommandId.C2S_LoginRole, req);
    }
}
