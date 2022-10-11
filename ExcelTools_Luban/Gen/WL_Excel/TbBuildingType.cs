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



namespace cfg.WL_Excel
{

public sealed partial class TbBuildingType
{
    private readonly Dictionary<int, WL_Excel.BuildingType> _dataMap;
    private readonly List<WL_Excel.BuildingType> _dataList;
    
    public TbBuildingType(JSONNode _json)
    {
        _dataMap = new Dictionary<int, WL_Excel.BuildingType>();
        _dataList = new List<WL_Excel.BuildingType>();
        
        foreach(JSONNode _row in _json.Children)
        {
            var _v = WL_Excel.BuildingType.DeserializeBuildingType(_row);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
        PostInit();
    }

    public Dictionary<int, WL_Excel.BuildingType> DataMap => _dataMap;
    public List<WL_Excel.BuildingType> DataList => _dataList;

    public WL_Excel.BuildingType GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public WL_Excel.BuildingType Get(int key) => _dataMap[key];
    public WL_Excel.BuildingType this[int key] => _dataMap[key];

    public void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var v in _dataList)
        {
            v.Resolve(_tables);
        }
        PostResolve();
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var v in _dataList)
        {
            v.TranslateText(translator);
        }
    }
    
    
    partial void PostInit();
    partial void PostResolve();
}

}