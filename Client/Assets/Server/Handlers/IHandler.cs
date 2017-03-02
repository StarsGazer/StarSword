public interface IHandler
{
    // 唯一的 Id
    int commandId { get; }
    // 执行方法，线程安全
    void Execute(MessageBody data);
}
