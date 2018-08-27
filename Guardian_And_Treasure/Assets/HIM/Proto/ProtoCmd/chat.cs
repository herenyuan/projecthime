//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: chat.proto
namespace proto.chat
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ChatRequest")]
  public partial class ChatRequest : global::ProtoBuf.IExtensible
  {
    public ChatRequest() {}
    
    private proto.chat.ChatType _chatType = proto.chat.ChatType.World;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"chatType", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(proto.chat.ChatType.World)]
    public proto.chat.ChatType chatType
    {
      get { return _chatType; }
      set { _chatType = value; }
    }
    private proto.chat.UserInfo _receiver = null;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"receiver", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public proto.chat.UserInfo receiver
    {
      get { return _receiver; }
      set { _receiver = value; }
    }
    private string _message = "";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"message", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string message
    {
      get { return _message; }
      set { _message = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ChatResponse")]
  public partial class ChatResponse : global::ProtoBuf.IExtensible
  {
    public ChatResponse() {}
    
    private proto.chat.ChatType _chatType = proto.chat.ChatType.World;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"chatType", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(proto.chat.ChatType.World)]
    public proto.chat.ChatType chatType
    {
      get { return _chatType; }
      set { _chatType = value; }
    }
    private proto.chat.UserInfo _sender = null;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"sender", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public proto.chat.UserInfo sender
    {
      get { return _sender; }
      set { _sender = value; }
    }
    private proto.chat.UserInfo _receiver = null;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"receiver", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public proto.chat.UserInfo receiver
    {
      get { return _receiver; }
      set { _receiver = value; }
    }
    private string _message = "";
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"message", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string message
    {
      get { return _message; }
      set { _message = value; }
    }
    private string _url = "";
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"url", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string url
    {
      get { return _url; }
      set { _url = value; }
    }
    private proto.chat.ShowType _showType = proto.chat.ShowType.Marquee;
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"showType", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(proto.chat.ShowType.Marquee)]
    public proto.chat.ShowType showType
    {
      get { return _showType; }
      set { _showType = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UserInfo")]
  public partial class UserInfo : global::ProtoBuf.IExtensible
  {
    public UserInfo() {}
    
    private string _roleId = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"roleId", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string roleId
    {
      get { return _roleId; }
      set { _roleId = value; }
    }
    private string _roleName = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"roleName", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string roleName
    {
      get { return _roleName; }
      set { _roleName = value; }
    }
    private int _roleType = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"roleType", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int roleType
    {
      get { return _roleType; }
      set { _roleType = value; }
    }
    private int _index = default(int);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"index", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int index
    {
      get { return _index; }
      set { _index = value; }
    }
    private int _vipLevel = default(int);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"vipLevel", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int vipLevel
    {
      get { return _vipLevel; }
      set { _vipLevel = value; }
    }
    private string _leagueName = "";
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"leagueName", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string leagueName
    {
      get { return _leagueName; }
      set { _leagueName = value; }
    }
    private int _roleLevel = default(int);
    [global::ProtoBuf.ProtoMember(11, IsRequired = false, Name=@"roleLevel", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int roleLevel
    {
      get { return _roleLevel; }
      set { _roleLevel = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ListInfoRequest")]
  public partial class ListInfoRequest : global::ProtoBuf.IExtensible
  {
    public ListInfoRequest() {}
    
    private string _roleId = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"roleId", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string roleId
    {
      get { return _roleId; }
      set { _roleId = value; }
    }
    private string _roleName = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"roleName", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string roleName
    {
      get { return _roleName; }
      set { _roleName = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    [global::ProtoBuf.ProtoContract(Name=@"ChatType")]
    public enum ChatType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"World", Value=1)]
      World = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Current", Value=2)]
      Current = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Legion", Value=3)]
      Legion = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Team", Value=4)]
      Team = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Person", Value=5)]
      Person = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Horn", Value=6)]
      Horn = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Mine", Value=10)]
      Mine = 10,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ErrorCode", Value=7)]
      ErrorCode = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Announce", Value=8)]
      Announce = 8,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Notice", Value=9)]
      Notice = 9
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"ShowType")]
    public enum ShowType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"Marquee", Value=2)]
      Marquee = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ChatChannel", Value=4)]
      ChatChannel = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"MarqueeAndChatChannel", Value=6)]
      MarqueeAndChatChannel = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"UpCenter", Value=8)]
      UpCenter = 8,
            
      [global::ProtoBuf.ProtoEnum(Name=@"MarqueeAndUpCenter", Value=10)]
      MarqueeAndUpCenter = 10,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ChatChannelAndUpCenter", Value=12)]
      ChatChannelAndUpCenter = 12,
            
      [global::ProtoBuf.ProtoEnum(Name=@"All", Value=14)]
      All = 14
    }
  
}