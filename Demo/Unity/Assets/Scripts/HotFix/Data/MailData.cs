//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;
using SimpleJSON;



namespace HotFix.Data
{

public sealed partial class MailData :  Bright.Config.BeanBase 
{
    public MailData(JSONNode _json) 
    {
        { if(!_json["tag"].IsNumber) { throw new SerializationException(); }  Tag = _json["tag"]; }
        { if(!_json["mailLevel"].IsNumber) { throw new SerializationException(); }  MailLevel = _json["mailLevel"]; }
        { if(!_json["mailType"].IsNumber) { throw new SerializationException(); }  MailType = _json["mailType"]; }
        { if(!_json["mailTitle"].IsString) { throw new SerializationException(); }  MailTitle = _json["mailTitle"]; }
        { if(!_json["subTitle"].IsString) { throw new SerializationException(); }  SubTitle = _json["subTitle"]; }
        { if(!_json["mailContent"].IsString) { throw new SerializationException(); }  MailContent = _json["mailContent"]; }
        { if(!_json["icon"].IsString) { throw new SerializationException(); }  Icon = _json["icon"]; }
        { if(!_json["mailBase"].IsString) { throw new SerializationException(); }  MailBase = _json["mailBase"]; }
        { if(!_json["rewardTag"].IsNumber) { throw new SerializationException(); }  RewardTag = _json["rewardTag"]; }
        PostInit();
    }

    public MailData(int tag, int mailLevel, int mailType, string mailTitle, string subTitle, string mailContent, string icon, string mailBase, int rewardTag ) 
    {
        this.Tag = tag;
        this.MailLevel = mailLevel;
        this.MailType = mailType;
        this.MailTitle = mailTitle;
        this.SubTitle = subTitle;
        this.MailContent = mailContent;
        this.Icon = icon;
        this.MailBase = mailBase;
        this.RewardTag = rewardTag;
        PostInit();
    }

    public static MailData DeserializeMailData(JSONNode _json)
    {
        return new MailData(_json);
    }

    /// <summary>
    /// 序号
    /// </summary>
    public int Tag { get; private set; }
    /// <summary>
    /// 重要等级
    /// </summary>
    public int MailLevel { get; private set; }
    /// <summary>
    /// 邮件类型
    /// </summary>
    public int MailType { get; private set; }
    /// <summary>
    /// 邮件标题
    /// </summary>
    public string MailTitle { get; private set; }
    /// <summary>
    /// 副标题
    /// </summary>
    public string SubTitle { get; private set; }
    /// <summary>
    /// 邮件内容
    /// </summary>
    public string MailContent { get; private set; }
    /// <summary>
    /// 邮件ICON
    /// </summary>
    public string Icon { get; private set; }
    /// <summary>
    /// 邮件底图
    /// </summary>
    public string MailBase { get; private set; }
    /// <summary>
    /// 邮件奖励
    /// </summary>
    public int RewardTag { get; private set; }

    public const int __ID__ = 53877281;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "Tag:" + Tag + ","
        + "MailLevel:" + MailLevel + ","
        + "MailType:" + MailType + ","
        + "MailTitle:" + MailTitle + ","
        + "SubTitle:" + SubTitle + ","
        + "MailContent:" + MailContent + ","
        + "Icon:" + Icon + ","
        + "MailBase:" + MailBase + ","
        + "RewardTag:" + RewardTag + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}