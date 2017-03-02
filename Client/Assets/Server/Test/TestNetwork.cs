using UnityEngine;
using Protos.Login;

public class TestNetwork : MonoBehaviour
{
    void Start()
    {
        LoginReq model1 = new LoginReq() { username = "maomao", password = "boy" };

        // 序列化
        byte[] req = Util.Serialize(model1);
        Debug.Log(System.BitConverter.ToString(req));

        // 模拟打包
        byte[] data = MessageParse.Parse(1, Consts_CommandId.C2S_Login, req);

        // --------------------
        // 网络传输......
        // --------------------

        // 模拟解包
        MessageData res = MessageParse.Unparse(data);

        // 反序列化
        LoginReq model2 = Util.Deserialize<LoginReq>(res.body.msg);

        // 查看结果
        Debug.Log("commandId : " + res.body.commandId);
        Debug.Log("username : " + model2.username);
        Debug.Log("password : " + model2.password);
    }
}
