/// <summary>
/// --------------------
/// 消息头 13 字节
/// --------------------
/// [HEAD_0 HEAD_0 HEAD_0 HEAD_0 ProtoVersion ServerVersion Length]
/// --------------------
/// 消息体 4 + msg.Length 字节
/// --------------------
/// [commandId msg]
/// --------------------
/// </summary>

// 消息头
public class MessageHead
{
    public byte HEAD_0 { get; set; }
    public byte HEAD_1 { get; set; }
    public byte HEAD_2 { get; set; }
    public byte HEAD_3 { get; set; }
    public byte ProtoVersion { get; set; }
    public int ServerVersion { get; set; }
    public int Length { get; set; }
}

// 消息体
public class MessageBody
{
    public int commandId { get; set; }
    public byte[] msg { get; set; }
}

// 消息
public class MessageData
{
    public MessageHead head { get; set; }
    public MessageBody body { get; set; }
}

public class MessageParse
{
    // ----------解析消息头----------
    public static MessageHead UnparseHead(byte[] buffer)
    {
        if (buffer.Length >= 13)  // 消息头长度足够
        {
            MessageHead head = new MessageHead();
            head.HEAD_0 = buffer[0];
            head.HEAD_1 = buffer[1];
            head.HEAD_2 = buffer[2];
            head.HEAD_3 = buffer[3];
            head.ProtoVersion = buffer[4];

            // (5 6 7 8 9 10 11 12) 错误方法
            // System.Array.Reverse(buffer, 5, 8);

            // (5 6 7 8) (9 10 11 12) 网络序 -> C# 小端序
            System.Array.Reverse(buffer, 5, 4);
            System.Array.Reverse(buffer, 9, 4);

            // 获取 ServerVersion
            head.ServerVersion = System.BitConverter.ToInt32(buffer, 5);
            // 获取 Length
            head.Length = System.BitConverter.ToInt32(buffer, 9);

            return head;
        }
        return null;
    }

    // ----------解析消息体----------
    public static MessageBody UnparseBody(int length, byte[] buffer)
    {
        if (buffer.Length >= length + 13)  // 消息体长度足够
        {
            MessageBody body = new MessageBody();

            // (13 14 15 16) 网络序 -> C# 小端序
            System.Array.Reverse(buffer, 13, 4);

            // 获取 commandId
            body.commandId = System.BitConverter.ToInt32(buffer, 13);
            // 获取 msg
            body.msg = new byte[length - 4];
            System.Array.Copy(buffer, 17, body.msg, 0, body.msg.Length);

            return body;
        }
        return null;
    }

    // ----------解析消息----------
    public static MessageData Unparse(byte[] buffer)
    {
        MessageHead head = UnparseHead(buffer);
        if (head != null)
        {
            MessageBody body = UnparseBody(head.Length, buffer);
            if (body != null)
            {
                MessageData data = new MessageData() { head = head, body = body };
                return data;
            }
        }
        return null;
    }

    // ----------打包消息头----------
    private static byte[] ParseHead(int serverVersion, int length)
    {
        byte[] head = new byte[13];
        head[0] = 0;
        head[1] = 0;
        head[2] = 0;
        head[3] = 0;
        head[4] = 0;

        // 复制 ServerVersion
        System.Array.Copy(System.BitConverter.GetBytes(serverVersion), 0, head, 5, 4);
        // 复制 Length
        System.Array.Copy(System.BitConverter.GetBytes(length), 0, head, 9, 4);

        // (5 6 7 8 9 10 11 12) 错误方法
        // System.Array.Reverse(data, 5, 8);

        // (5 6 7 8) (9 10 11 12) C# 小端序 -> 网络序
        System.Array.Reverse(head, 5, 4);
        System.Array.Reverse(head, 9, 4);

        return head;
    }

    // ----------打包消息体----------
    private static byte[] ParseBody(int commandId, byte[] msg)
    {
        if (msg == null)  // 消息体为空
        {
            byte[] body = new byte[4];
            // 复制 commandId
            System.Array.Copy(System.BitConverter.GetBytes(commandId), 0, body, 0, 4);
            // (0 1 2 3) C# 小端序 -> 网络序
            System.Array.Reverse(body, 0, 4);
            return body;
        }
        else  // 消息体不为空
        {
            byte[] body = new byte[4 + msg.Length];
            // 复制 commandId
            System.Array.Copy(System.BitConverter.GetBytes(commandId), 0, body, 0, 4);
            // (0 1 2 3) C# 小端序 -> 网络序
            System.Array.Reverse(body, 0, 4);
            // 复制 msg
            System.Array.Copy(msg, 0, body, 4, msg.Length);
            return body;
        }
    }

    // ----------打包消息----------
    public static byte[] Parse(int serverVersion, int commandId, byte[] msg = null)
    {
        // 打包消息体
        byte[] body = ParseBody(commandId, msg);
        if (body != null)
        {
            // 打包消息头
            byte[] head = ParseHead(serverVersion, body.Length);
            byte[] data = new byte[head.Length + body.Length];
            // 复制 head
            System.Array.Copy(head, 0, data, 0, head.Length);
            // 复制 body
            System.Array.Copy(body, 0, data, head.Length, body.Length);
            return data;
        }
        return null;
    }
}
