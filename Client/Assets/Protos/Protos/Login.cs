//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: Protos/Login.proto
namespace Protos.Login
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginReq")]
  public partial class LoginReq : global::ProtoBuf.IExtensible
  {
    public LoginReq() {}
    
    private string _username;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"username", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string username
    {
      get { return _username; }
      set { _username = value; }
    }
    private string _password;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"password", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string password
    {
      get { return _password; }
      set { _password = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginRes")]
  public partial class LoginRes : global::ProtoBuf.IExtensible
  {
    public LoginRes() {}
    
    private int _result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }

    private int _userId = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"userId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int userId
    {
      get { return _userId; }
      set { _userId = value; }
    }

    private int _characterId = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"characterId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
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