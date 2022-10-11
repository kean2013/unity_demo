using FairyGUI;
using HA;
using ILRuntime.Runtime.Enviorment;

namespace JEngine.Helper
{
    public class HotFixLoadedHelper
    {
        public static void Init(AppDomain appDomain)
        {

            UIConfig.globalModalWaiting = "ui://Common/Waiting";//"ui://1go7k7dzt7652";

            UIConfig.popupMenu = "ui://Common/PopupMenu";
            UIConfig.modalLayerColor = new UnityEngine.Color(0f, 0f, 0f, 0.6f);


            //如果有设计分隔条
            UIConfig.popupMenu_seperator = "ui://Common/hseperator";


            UIObjectFactory.SetLoaderExtension(typeof(ExGloader));
        }
    }
}