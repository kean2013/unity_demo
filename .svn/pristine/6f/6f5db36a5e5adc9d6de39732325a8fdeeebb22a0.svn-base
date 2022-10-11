using HotFix.Enum;
using HotFix.Event;
using HotFix.Tool;
using System;
using UnityEngine;
namespace HotFix.UI
{
    using Enum;
    using System.Reflection;

    public class LoginUIView : BaseUIView
    {
        private LoginUIComp m_UiComp;

        public LoginUIView()
        {
            CreateWindow("LoginUI", "Root");
        }

        public override void InitElements()
        {
            m_UiComp = new LoginUIComp(WndComp);
            m_UiComp.m_GameStart.onClick.Add(GameStartBtnClick);

            m_UiComp.m_LoginCP.m_username.text = "name";
            m_UiComp.m_LoginCP.m_password.text = "passwords";
            m_UiComp.m_ServerSelectBtn.visible = false;
        }

        public override void OnHide()
        {
        }

        public override void OnShow(params object[] para)
        {

        }

        public override void RegisterEvent()
        {
            EventMgr.Instance.Register(HEventType.NetConnectEvent, NetConnectEventCallBack);
        }

        public override void UnRegisterEvent()
        {
            EventMgr.Instance.UnRegister(HEventType.NetConnectEvent, NetConnectEventCallBack);
        }

        private void GameStartBtnClick()
        {
            LoginUIMgr.Instance.LoginRequest(0, 1, "this is test");
        }

        private void NetConnectEventCallBack(HEvent hEvent)
        {
            var data = hEvent as NetConnectEvent;
            if (data.State == 1)
            {
                m_UiComp.Ctrl_Root.selectedIndex = 1;
            }
        }
    }
}
