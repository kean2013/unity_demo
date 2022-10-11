using FairyGUI;
using JEngine.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class JUIManager : MonoBehaviour
    {
        Stack<JWindow> _stack = new Stack<JWindow>();
        List<JWindow> _active = new List<JWindow>();

        public static JUIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("UIManager").AddComponent<JUIManager>();
                }
                return _instance;
            }
        }

        private static JUIManager _instance;
        public JJoystick joystick;

        public void Awake()
        {
            Log.Print("Awake");

            DontDestroyOnLoad(this);
            Init();
        }

        public void OnDestroy()
        {
            GRoot.inst.CloseAllWindows();
            foreach (var hw in _active) hw?.OnDispose();

            _active.Clear(); 
            _stack.Clear();
        }

        public void OnApplicationQuit()
        {
            OnDestroy();
        }

        private void Init()
        {
            UIConfig.bringWindowToFrontOnClick = false;


            //FontManager.RegisterFont(FontManager.GetFont("MNJZDX_s"), "迷你简中等线");
            //FontManager.RegisterFont(FontManager.GetFont("FZY4JW_0"), "方正粗圆简体");
            //UIConfig.defaultFont = "FZY4JW_0";
            //HtmlParseOptions.DefaultLinkColor = Color.white;

            // UIObjectFactory.SetLoaderExtension(typeof(ExGloader));
        }

        private T GetActive<T> () where T : JWindow, new()
        {
            foreach (var w in _active)
            {
                if ( w is T) return (T)w;
            }

            return default(T);
        }

        public void Show<T>() where T : JWindow, new()
        {
            T wnd = (T)GetActive<T>() ?? new T();

            if (!_active.Contains(wnd))
            {
                wnd.asWindow.name = wnd.GetType().ToString();
                _active.Add(wnd);
            }

            _stack.Push(wnd);
            wnd?.Show();
        }

        public void ShowModal<T> () where T : JWindow, new()
        {
            T wnd = (T)GetActive<T>() ?? new T();

            wnd.asWindow.modal = true;
            wnd.asWindow.__doShowAnimation = wnd.__DoShowAnimation;

            if (!_active.Contains(wnd))
            {
                wnd.asWindow.name = wnd.GetType().ToString();
                _active.Add(wnd);
            }

            _stack.Push(wnd);
            wnd?.Show();
        }

        // 在Unity主工程中，无法通过Activator来创建热更DLL内类型的实例，必须通过AppDomain来创建实例
        private JWindow CreateWindow(string pkgstr, string wndstr, bool isAsync)
        {
            JWindow wnd = null;
            try
            {
                string fullClassName = "HA." + pkgstr;
                Type type = Type.GetType(fullClassName);
                if (type != null)
                    wnd = type.Assembly.CreateInstance(fullClassName) as JWindow;
                else
                    wnd = InitJEngine.Instance.Instantiate<JWindow>(fullClassName);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }

            return wnd;
        }

        #region HWindow
        void ShowWindow(JWindow wnd) => GRoot.inst.ShowWindow(wnd.asWindow);
        void HideWindow(JWindow wnd) => GRoot.inst.HideWindow(wnd.asWindow);
        void HideWindowImmediately(JWindow wnd) => GRoot.inst.HideWindowImmediately(wnd.asWindow);

        public JWindow GetTopWindow()
        {
            Window wnd = GRoot.inst.GetTopWindow();
            if (wnd == null) return null;

            foreach (var w in _active)
            {
                if (w.asWindow == wnd) return w;
            }

            return null;
        }

        private Window GetWindow(string wndname)
        {
            int cnt = GRoot.inst.numChildren;
            for (int i = cnt - 1; i >= 0; i--)
            {
                GObject g = GRoot.inst.GetChildAt(i);
                if (g is Window && g.name.CompareTo(wndname) == 0)
                {
                    return (Window)(g);
                }
            }

            return null;
        }

        public void CloseAllWindows() => GRoot.inst.CloseAllWindows();

        public void BringToFront(JWindow wnd) => GRoot.inst.BringToFront(wnd.asWindow);

        #endregion

        #region Toast

        #endregion

        #region PopupMenu
        private static PopupMenu popup;
        public static PopupMenu GetPopupMenu()
        {
            if (popup == null)
            {
                return popup = new PopupMenu();
            }

            popup.ClearItems();
            return popup;
        }

        #endregion

        #region ModalWait 
        public bool modalWaiting => GRoot.inst.modalWaiting;

        public static IEnumerator ShowModalWait(Func<bool> can_close, float delay = 0f, float timeout = 0f)
        {
            if (delay > 0.001f)
            {
                yield return new WaitForSeconds(delay);
            }

            if (can_close()) yield break;
            GRoot.inst.ShowModalWait();

            if (timeout > 0.001f)
            {
                while ((timeout -= Time.deltaTime) > 0)
                {
                    if (can_close()) break;
                    yield return 1;
                }
            }
            else
            {
                yield return new WaitUntil(can_close);
            }

            GRoot.inst.CloseModalWait();
        }
        #endregion

        #region Tooltips
        public void ShowTooltips(string tips, float delay = 0.1f) => GRoot.inst.ShowTooltips(tips, delay);

        public void HideTooltips() => GRoot.inst.HideTooltips();
        #endregion

        #region scene

        #endregion
    }
}
