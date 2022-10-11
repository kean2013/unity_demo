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

public sealed partial class LanguageData :  Bright.Config.BeanBase 
{
    public LanguageData(JSONNode _json) 
    {
        { if(!_json["tag"].IsNumber) { throw new SerializationException(); }  Tag = _json["tag"]; }
        { if(!_json["cn"].IsString) { throw new SerializationException(); }  Cn = _json["cn"]; }
        { if(!_json["en"].IsString) { throw new SerializationException(); }  En = _json["en"]; }
        PostInit();
    }

    public LanguageData(int tag, string cn, string en ) 
    {
        this.Tag = tag;
        this.Cn = cn;
        this.En = en;
        PostInit();
    }

    public static LanguageData DeserializeLanguageData(JSONNode _json)
    {
        return new LanguageData(_json);
    }

    /// <summary>
    /// 序号
    /// </summary>
    public int Tag { get; private set; }
    /// <summary>
    /// 中文
    /// </summary>
    public string Cn { get; private set; }
    /// <summary>
    /// 英文
    /// </summary>
    public string En { get; private set; }

    public const int __ID__ = -1928011966;
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
        + "Cn:" + Cn + ","
        + "En:" + En + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}
