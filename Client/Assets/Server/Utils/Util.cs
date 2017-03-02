using System.IO;
using System.Text;
using ProtoBuf;

public static class Util
{
    // 序列化
    public static byte[] Serialize<T>(T model)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            Serializer.Serialize<T>(ms, model);
            byte[] result = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(result, 0, result.Length);
            return result;
        }
    }

    // 反序列化
    public static T Deserialize<T>(byte[] data)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            ms.Write(data, 0, data.Length);
            ms.Position = 0;
            T result = Serializer.Deserialize<T>(ms);
            return result;
        }
    }

    // Md5加密
    public static string Md5Sum(string input)
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();
    }
}
