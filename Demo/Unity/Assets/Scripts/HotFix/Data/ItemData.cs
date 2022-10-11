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

public sealed partial class ItemData :  Bright.Config.BeanBase 
{
    public ItemData(JSONNode _json) 
    {
        { if(!_json["tag"].IsNumber) { throw new SerializationException(); }  Tag = _json["tag"]; }
        { if(!_json["name"].IsString) { throw new SerializationException(); }  Name = _json["name"]; }
        { if(!_json["desc"].IsString) { throw new SerializationException(); }  Desc = _json["desc"]; }
        { if(!_json["Icon"].IsString) { throw new SerializationException(); }  Icon = _json["Icon"]; }
        { if(!_json["quality"].IsNumber) { throw new SerializationException(); }  Quality = _json["quality"]; }
        { if(!_json["Prompt"].IsNumber) { throw new SerializationException(); }  Prompt = _json["Prompt"]; }
        { if(!_json["itemTag"].IsNumber) { throw new SerializationException(); }  ItemTag = _json["itemTag"]; }
        { if(!_json["order"].IsNumber) { throw new SerializationException(); }  Order = _json["order"]; }
        { if(!_json["type"].IsString) { throw new SerializationException(); }  Type = _json["type"]; }
        { if(!_json["use"].IsNumber) { throw new SerializationException(); }  Use = _json["use"]; }
        { if(!_json["maxLimit"].IsNumber) { throw new SerializationException(); }  MaxLimit = _json["maxLimit"]; }
        { if(!_json["tips"].IsNumber) { throw new SerializationException(); }  Tips = _json["tips"]; }
        { if(!_json["access"].IsNumber) { throw new SerializationException(); }  Access = _json["access"]; }
        { var _j = _json["useItem"]; if (_j.Tag != JSONNodeType.None && _j.Tag != JSONNodeType.NullValue) { { if(!_j.IsObject) { throw new SerializationException(); }  UseItem = UserItemStruct.DeserializeUserItemStruct(_j);  } } else { UseItem = null; } }
        { if(!_json["required_Level"].IsString) { throw new SerializationException(); }  RequiredLevel = _json["required_Level"]; }
        { if(!_json["automatic"].IsNumber) { throw new SerializationException(); }  Automatic = _json["automatic"]; }
        { var __json0 = _json["acquisition"]; if(!__json0.IsArray) { throw new SerializationException(); } int _n0 = __json0.Count; Acquisition = new int[_n0]; int __index0=0; foreach(JSONNode __e0 in __json0.Children) { int __v0;  { if(!__e0.IsNumber) { throw new SerializationException(); }  __v0 = __e0; }  Acquisition[__index0++] = __v0; }   }
        { if(!_json["itemValue"].IsString) { throw new SerializationException(); }  ItemValue = _json["itemValue"]; }
        { if(!_json["text"].IsNumber) { throw new SerializationException(); }  Text = _json["text"]; }
        PostInit();
    }

    public ItemData(int tag, string name, string desc, string Icon, int quality, int Prompt, int itemTag, int order, string type, int use, int maxLimit, int tips, int access, UserItemStruct useItem, string required_Level, int automatic, int[] acquisition, string itemValue, int text ) 
    {
        this.Tag = tag;
        this.Name = name;
        this.Desc = desc;
        this.Icon = Icon;
        this.Quality = quality;
        this.Prompt = Prompt;
        this.ItemTag = itemTag;
        this.Order = order;
        this.Type = type;
        this.Use = use;
        this.MaxLimit = maxLimit;
        this.Tips = tips;
        this.Access = access;
        this.UseItem = useItem;
        this.RequiredLevel = required_Level;
        this.Automatic = automatic;
        this.Acquisition = acquisition;
        this.ItemValue = itemValue;
        this.Text = text;
        PostInit();
    }

    public static ItemData DeserializeItemData(JSONNode _json)
    {
        return new ItemData(_json);
    }

    /// <summary>
    /// 索引
    /// </summary>
    public int Tag { get; private set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; private set; }
    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; private set; }
    /// <summary>
    /// 品质
    /// </summary>
    public int Quality { get; private set; }
    /// <summary>
    /// 获取提示
    /// </summary>
    public int Prompt { get; private set; }
    /// <summary>
    /// 标签
    /// </summary>
    public int ItemTag { get; private set; }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order { get; private set; }
    /// <summary>
    /// 道具类型
    /// </summary>
    public string Type { get; private set; }
    /// <summary>
    /// 是否可批量使用
    /// </summary>
    public int Use { get; private set; }
    /// <summary>
    /// 堆叠上限
    /// </summary>
    public int MaxLimit { get; private set; }
    /// <summary>
    /// 道具tips
    /// </summary>
    public int Tips { get; private set; }
    /// <summary>
    /// 前往
    /// </summary>
    public int Access { get; private set; }
    /// <summary>
    /// 使用触发事件
    /// </summary>
    public UserItemStruct UseItem { get; private set; }
    /// <summary>
    /// 使用条件
    /// </summary>
    public string RequiredLevel { get; private set; }
    /// <summary>
    /// 自动使用
    /// </summary>
    public int Automatic { get; private set; }
    /// <summary>
    /// 获取途径
    /// </summary>
    public int[] Acquisition { get; private set; }
    /// <summary>
    /// 道具价值文本
    /// </summary>
    public string ItemValue { get; private set; }
    /// <summary>
    /// 道具使用失败提示文本
    /// </summary>
    public int Text { get; private set; }

    public const int __ID__ = 1241678205;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        UseItem?.Resolve(_tables);
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
        UseItem?.TranslateText(translator);
    }

    public override string ToString()
    {
        return "{ "
        + "Tag:" + Tag + ","
        + "Name:" + Name + ","
        + "Desc:" + Desc + ","
        + "Icon:" + Icon + ","
        + "Quality:" + Quality + ","
        + "Prompt:" + Prompt + ","
        + "ItemTag:" + ItemTag + ","
        + "Order:" + Order + ","
        + "Type:" + Type + ","
        + "Use:" + Use + ","
        + "MaxLimit:" + MaxLimit + ","
        + "Tips:" + Tips + ","
        + "Access:" + Access + ","
        + "UseItem:" + UseItem + ","
        + "RequiredLevel:" + RequiredLevel + ","
        + "Automatic:" + Automatic + ","
        + "Acquisition:" + Bright.Common.StringUtil.CollectionToString(Acquisition) + ","
        + "ItemValue:" + ItemValue + ","
        + "Text:" + Text + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}
