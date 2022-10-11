using FairyGUI;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Window的Wrapper
    /// 使用者，需要来完成构造函数，OnInit以及显示和隐藏的效果。
    /// 用组合的方式实现相继承了Window的作用
    /// 构造函数需要调用  _wnd.AddUISource(new UIResLoad("SettingPad"));
    /// OnInit 的时候调用 _wnd.contentPane = UIPackage.CreateObject("SettingPad", "Main").asCom;
    /// Init可以手动调用，或者由onAddedToStage的时候自动触发
    /// </summary>
    public class JWindow
    {
        protected Window _wnd = new Window();
        protected GComponent asPanel => _wnd.contentPane;
        public Window asWindow => _wnd;

        public bool isShowing => _wnd.isShowing;
        public bool isTop => _wnd.isTop;

        public string name => _wnd.name;

        public string gameObjectName => _wnd.gameObjectName;

        public GObject GetChild(string go) => _wnd.contentPane.GetChild(go);
        public void SetPanel(GComponent comp) => _wnd.contentPane = comp;

        public bool modal => _wnd.modal;

        public void Show() => _wnd.Show();      // __addedToStage
        public void Hide() => _wnd.Hide();      // __removeFromStage
        public void Init() => _wnd.Init();
        public void HideImmediately() => _wnd.HideImmediately();
        public void Center(bool restraint = false) => _wnd.Center(restraint);
        public void ToggleStatus() => _wnd.ToggleStatus();
        public void BringToFront() => _wnd.BringToFront();


        public JWindow()
        {
            _wnd.__onInit = OnInit;
            _wnd.__onHide = OnHide;
            _wnd.__onShown = OnShown;
            _wnd.__doHideAnimation = DoHideAnimation;
            _wnd.__doShowAnimation = DoShowAnimation;

            _wnd.__onDispose = OnDispose;

        }

        virtual protected void OnInit() { }
        virtual protected void OnHide() {
           //  __dispose();
        }

        void __dispose()
        {
            _wnd.contentPane?.Dispose();
            _wnd.contentPane = null;

            _wnd.Dispose();
            _wnd = null;
        }

        virtual protected void OnShown() { }
        virtual public void OnDispose() { }       
        virtual protected void DoShowAnimation() { }
        virtual protected void DoHideAnimation()
        {
            HideImmediately();
        }


        public void __DoShowAnimation()
        {
            _wnd.Center();

            _wnd.SetScale(0.8f, 0.8f);
            _wnd.SetPivot(0.5f, 0.5f);

            // 动画结束后调用onShown
            _wnd.TweenScale(new Vector2(1.0f, 1.0f), 0.3f).SetEase(EaseType.BackOut)
                .OnComplete(OnShown);
        }
    }
}
