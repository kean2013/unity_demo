using HotFix.Data;
using HotFix.Tool;
using System.IO;
using HotFix.Common;

namespace HotFix.Mgr
{
    public class DataMgr : Singleton<DataMgr>
    {
        private Tables cfgTables;
        public Tables CfgTables
        {
            get
            {
                if (cfgTables == null)
                    cfgTables = GetCfgTables();
                return cfgTables;
            }
        }
        
        private Tables GetCfgTables()
        {
            var tablesCtor = typeof(Tables).GetConstructors()[0];
            var loaderReturnType = tablesCtor.GetParameters()[0].ParameterType.GetGenericArguments()[1];
            // 根据cfg.Tables的构造函数的Loader的返回值类型决定使用json还是ByteBuf Loader
            System.Delegate loader = loaderReturnType == typeof(Bright.Serialization.ByteBuf) ?
                new System.Func<string, Bright.Serialization.ByteBuf>(LoadByteBuf)
                : (System.Delegate)new System.Func<string, SimpleJSON.JSONNode>(LoadJson);

            var tables = (Tables)tablesCtor.Invoke(new object[] { loader });
            return tables;
        }


        private static SimpleJSON.JSONNode LoadJson(string file)
        {
            return SimpleJSON.JSON.Parse(File.ReadAllText($"Assets/Resources/ExcelData_json/{file}.json", System.Text.Encoding.UTF8));
        }

        private static Bright.Serialization.ByteBuf LoadByteBuf(string file)
        {
            return new Bright.Serialization.ByteBuf(File.ReadAllBytes($"Assets/Resources/ExcelData_json/{file}.bytes"));
        }

#if UNITY_EDITOR
        public void Reload()
        {
            cfgTables = GetCfgTables();
        }
#endif

    }
}
