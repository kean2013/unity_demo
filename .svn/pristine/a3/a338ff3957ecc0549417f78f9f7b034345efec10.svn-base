using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotFix.Tool;
using HA;
using System;
using IL = ILRuntime.Runtime.Enviorment;

namespace HotFix.UI
{
    using Common;
    public class UIMgr : Singleton<UIMgr>
    {
        List<BaseUIView> m_Active;

        public override void OnInit()
        {
            m_Active = new List<BaseUIView>();
        }

        public override void OnUnInit()
        {
            base.UnInit();
        }

        public void Open<T>(params object[] para) where T : BaseUIView, new()
        {
            var wnd = Show<T>();
            if (!m_Active.Contains(wnd))
            {
                wnd.WindowName = wnd.GetType().ToString();
                m_Active.Add(wnd);
            }
            wnd?.Open(para);
        }

        public void Open(Type t, params object[] para)
        {
            bool isActive = false;
            BaseUIView wnd = null;
            foreach (var w in m_Active)
            {
                if (t == w.GetType())
                {
                    isActive = true;
                    wnd = w;
                    break;
                }
            }

            if (!isActive)
            {
#if ILHotFix
                wnd = InitJEngine.Instance.Instantiate<BaseUIView>(t.FullName);
#else
                wnd = t.Assembly.CreateInstance(t.FullName) as BaseUIView;
#endif
            }

            if (!m_Active.Contains(wnd))
            {
                wnd.WindowName = wnd.GetType().ToString();
                m_Active.Add(wnd);
            }
            wnd?.Open(para);
        }

        public void Close<T>() where T : BaseUIView, new()
        {
            var wnd = GetActive<T>();
            if (m_Active.Contains(wnd))
            {
                wnd.WindowName = wnd.GetType().ToString();
                m_Active.Remove(wnd);
                wnd.Close();
            }
        }

        public bool CheckOpen<T>() where T : BaseUIView, new()
        {
            var wnd = GetActive<T>();
            if (m_Active.Contains(wnd))
            {
                return true;
            }

            return false;
        }

        public T Show<T>() where T : BaseUIView, new()
        {
            T wnd = (T)GetActive<T>() ?? new T();

            return wnd;
        }

        private T GetActive<T>() where T : BaseUIView, new()
        {
            foreach (var w in m_Active)
            {
                var ret = w as T;
                if(ret != null)
                    return ret;
            }

            return default(T);
        }
    }
}