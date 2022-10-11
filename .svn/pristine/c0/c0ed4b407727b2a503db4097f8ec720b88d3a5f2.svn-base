using HotFix.Enum;
using HotFix.Event;
using HotFix.Tool;
using System;
using UnityEngine;
namespace HotFix.UI
{
    public class MailUIView : BaseUIView
    {
        private MailUIComp m_UiComp;

        public MailUIView()
        {
            CreateWindow("MailUI", "Root");
        }

        public override void InitElements()
        {
			m_UiComp = new MailUIComp(WndComp);
        }

        public override void OnHide()
        {
        }

        public override void OnShow(params object[] para)
        {
			//if(para[0]!= null)
            //{
            //    JLogger.Log(" test  "+ para[0]);
            //}
        }

        public override void RegisterEvent()
        {
            //EventMgr.Instance.Register(HEventType.NetConnectEvent, NetConnectEventCallBack);
        }

        public override void UnRegisterEvent()
        {
			//EventMgr.Instance.UnRegister(HEventType.NetConnectEvent, NetConnectEventCallBack);
        }

        //private void NetConnectEventCallBack(HEvent hEvent)
        //{
        //    var data = hEvent as NetConnectEvent;
        //}

    }
}
