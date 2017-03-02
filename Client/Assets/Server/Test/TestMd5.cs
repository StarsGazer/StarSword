using UnityEngine;

public class TestMd5 : MonoBehaviour
{
    void Start()
    {
        // Md5加密前
        string password_before = "boy";
        // Md5加密后
        string password_after = Util.Md5Sum(password_before);

        // 查看结果
        Debug.Log("before : " + password_before);
        Debug.Log("after : " + password_after);
    }
}
