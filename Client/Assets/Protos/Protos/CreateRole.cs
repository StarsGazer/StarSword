//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: Protos/CreateRole.proto
namespace Protos.CreateRole
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"CreateRoleReq")]
  public partial class CreateRoleReq : global::ProtoBuf.IExtensible
  {
    public CreateRoleReq() {}
    
    private string _nickName;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"nickName", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string nickName
    {
      get { return _nickName; }
      set { _nickName = value; }
    }
    private int _profession;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"profession", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int profession
    {
      get { return _profession; }
      set { _profession = value; }
    }
    private bool _sex;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"sex", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool sex
    {
      get { return _sex; }
      set { _sex = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"CreateRoleRes")]
  public partial class CreateRoleRes : global::ProtoBuf.IExtensible
  {
    public CreateRoleRes() {}
    
    private int _result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }

    private int _characterId = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"characterId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int characterId
    {
      get { return _characterId; }
      set { _characterId = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}