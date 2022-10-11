using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BM;
using FairyGUI;
using FairyGUI.Utils;
using JEngine.Core;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// GRoot维护层级关系，
    /// GUIManager管理生命周期
    /// 管理的内容包括窗口的调度，Toast，Tooltips，PopupMenu以及ModalWait
    /// 提供一致行为
    /// </summary>
    /// 

    public class UIResource : IUISource
    {
        public bool ABRes = false;
        public UIResource(string pkgname) { fileName = pkgname; }

        private bool _loaded;
        
        public string fileName { get; set; }

        public void Cancel()
        {

        }

        public bool loaded => _loaded;

        public void Load(UILoadCallback callback)
        {
            loadres(fileName, false, (go) => { _loaded = true; callback?.Invoke(); });
        }

        private void loadres(string pkgstr, bool isABRes, Action<UIPackage> cb)
        {
            UIPackage var = UIPackage.GetByName(pkgstr);
            if (var == null)
            {
                if (isABRes)
                {
                    var tkdesc = AssetMgr.LoadAsync(PathHelper.ABPathDesc(pkgstr), null);
                    var tkres = AssetMgr.LoadAsync(PathHelper.ABPathRes(pkgstr), null);

                    var = UIPackage.AddPackage(tkdesc.Result as AssetBundle , tkres.Result as AssetBundle);
                }
                else
                {
                    var = UIPackage.AddPackage(PathHelper.WndPath(pkgstr));
                }
            }

            if (var == null) return;
            cb?.Invoke(var);
        }
    }

    public class PathHelper
    {
        public static string WndPath(string wndstr) => "UI/Packages/" + wndstr;
        public static string ABPathDesc(string wndstr) => wndstr + ".bytes";
        public static string ABPathRes(string wndstr) => wndstr + "_res.bytes";

        public static string EmojiABPath(string fname) => Application.streamingAssetsPath + "/" + fname;

        public static string PrefabPath(string prefab) => "Assets/HotUpdateResources/Prefab/" + prefab;

        public static string ScenePath(string scene) => AssetComponentConfig.AssetLoadMode == AssetLoadMode.Develop ? scene : "Assets/HotUpdateResources/Scene/" + scene;
    }

    /*
    public class GUIManager : MonoBehaviour
    {
        Dictionary<string, HWindow> _allWindows = new Dictionary<string, HWindow>();
        public static GUIManager Instance;

        public bool ABRes;

        public void Awake()
        {
            Log.Print("Awake");


            Instance = this;
            DontDestroyOnLoad(this);
            Init();
        }

        public void OnDestroy()
        {
            GRoot.inst.CloseAllWindows();
            foreach (var hw in _allWindows.Values) hw?.OnDispose();
            _allWindows.Clear();
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
        
        public void Show(string pkgstr, string wndstr, Action onLoaded = null   )
        {
            var wnd = GetActiveWindow(pkgstr, wndstr);
            if (wnd != null)
            {
                wnd.Show();
                return;
            }

            // 在Unity主工程中，无法通过Activator来创建热更DLL内类型的实例，必须通过AppDomain来创建实例
            wnd = CreateWindow(pkgstr, wndstr, true);
            if (wnd == null) return;

            _allWindows.Add(pkgstr + "." + wndstr, wnd);
            wnd.Show();
        }

        private HWindow CreateWindow(string pkgstr, string wndstr, bool isAsync)
        {
            HWindow wnd = null;
            try
            {
                string fullClassName = "HA." + pkgstr;
                Type type = Type.GetType(fullClassName);
                if (type != null)
                    wnd = type.Assembly.CreateInstance(fullClassName) as HWindow;
                else
                    wnd = InitJEngine.Instance.CreateInstance<HWindow>(fullClassName);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }

            return wnd;
        }
        

       


        public HWindow GetActiveWindow(string pkgstr, string wndstr)
        {
            if (_allWindows.TryGetValue(pkgstr + "." + wndstr, out HWindow wnd)) return wnd;
            return null;
        }
        public Window GetWindow(string name)
        {
            int cnt = GRoot.inst.numChildren;          

            for (int i = cnt; i >= 0; i--)
            {
                GObject g = GRoot.inst.GetChildAt(i);
                if (g is Window && g.name == name)
                {
                    return g as Window;
                }
            }

            return null;
        }

#region HWindow
        void ShowWindow(HWindow wnd) => GRoot.inst.ShowWindow(wnd.asWindow);
        void HideWindow(HWindow wnd) => GRoot.inst.HideWindow(wnd.asWindow);
        void HideWindowImmediately(HWindow wnd) => GRoot.inst.HideWindowImmediately(wnd.asWindow);

        public HWindow GetTopWindow()
        {
            Window wnd = GRoot.inst.GetTopWindow();
            if (wnd == null) return null;

            foreach (var w in _allWindows.Values)
            {
                if (w.asWindow == wnd) return w;
            }

            return null;
        }
        public void CloseAllWindows() => GRoot.inst.CloseAllWindows();

        public void BringToFront(HWindow wnd) => GRoot.inst.BringToFront(wnd.asWindow);

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
    }
        
    /// <summary>
    /// 使用ILRuntime，不能直接继承FGUI.Windows
    /// 因此，需要一个转换器
    /// </summary>
    public class UIWindow
    {
        protected Window _wnd = new Window();
        public bool isShowing => _wnd?.isShowing??false;
        public bool isTop => _wnd.isTop;
        public bool modal => _wnd.modal;

        private bool _inited = false;

        public Window asWnd => _wnd;

        public GComponent asPanel => _wnd.contentPane;
        public void BringToFront() => _wnd.BringToFront();
        public void HideImmediately() { _wnd.HideImmediately(); OnHide(); Dispose(); }
        public void ToggleStatus() => _wnd.ToggleStatus();

        //必须要提供一个无参数的构造函数, 跨域继承要求
        public UIWindow() { }

        virtual protected void DoShowAnimation() => OnShown();
        virtual protected void DoHideAnimation() => HideImmediately();
        virtual protected void OnInit() { }
        virtual protected void OnShown() { }
        virtual protected void OnHide() { }
        virtual protected void Init() { }

        public void Hide() { if (this.isShowing) DoHideAnimation(); }

        public void ShowPopup()
        {
            // 加入到GRoot中
            _wnd.Show();
            _wnd.SetScale(0.8f, 0.8f);
            _wnd.SetPivot(0.5f, 0.5f);

            // 动画结束后调用onShown
            _wnd.TweenScale(new Vector2(1.0f, 1.0f), 0.3f).SetEase(EaseType.BackOut)
                .OnUpdate(_wnd.InvalidateBatchingState)
                .OnComplete(OnShown);
        }

        public void HidePopup()
        {
            // 动画结束后调用 HideImmediately
            _wnd.TweenFade(0.1f, 0.3f).OnComplete(HideImmediately);
        }

        /// <summary>
        /// 销毁，必须显示调用
        /// </summary>
        public void Dispose()
        {
            _wnd.contentPane?.Dispose();
            _wnd.contentPane = null;

            _wnd.Dispose();
            _wnd = null;
        }

        public void _Init()
        {
            if (_inited) return;

            // 初始化资源路径，模拟_wnd.Init
            Init();
        }

        protected void LoadRes(string pkgstr, string wndstr, bool isAsync)
        {
            // 加载资源
            GUIManager.Instance.StartCoroutine(
                CreateWindow(pkgstr, wndstr,
                go =>
                {
                    // 加载成功
                    _inited = true;
                    _wnd.contentPane = go;

                    OnInit();

                    // 如果是显示状态，则显示
                    if (isShowing) DoShowAnimation();
                },
                isAsync)
            );
        }

        public void Show() 
        {
            // AddChild
            if (!isShowing) _wnd.Show();

            // 没有初始化则初始化
            if (!_inited) { _Init(); return; }

            // 显示
            DoShowAnimation();
        }

        private IEnumerator CreateWindow(string pkgstr, string wndstr, Action<GComponent> cb, bool isAsync)
        {
            UIPackage var = UIPackage.GetByName(pkgstr);
            if (var == null)
            {
                if (GUIManager.Instance.ABRes)
                {
                    var abdesc = libx.Versions.LoadAssetBundleFromFileAsync(GUIManager.ABPathDesc(pkgstr));
                    yield return abdesc;
                    if (abdesc == null) yield break;

                    var abres = libx.Versions.LoadAssetBundleFromFileAsync(GUIManager.ABPathRes(pkgstr));
                    yield return abres;
                    if (abres == null) yield break;

                    var = UIPackage.AddPackage(abdesc.assetBundle, abres.assetBundle);
                }
                else
                {
                    var = UIPackage.AddPackage(GUIManager.WndPath(pkgstr));
                }
            }

            if (var == null) yield break;

            if (isAsync)
            {
                var.CreateObjectAsync(wndstr, go => { cb.Invoke(go.asCom); });
                yield break;
            }
            else
            {
                GComponent panel = var.CreateObject(wndstr)?.asCom;
                if (panel == null) yield break;

                cb.Invoke(panel);
            }
        }
    }
    */
}
