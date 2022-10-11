// #define ILHotFix

using System;
using System.IO;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Mono.Cecil.Pdb;
using JEngine.Core;
using JEngine.Helper;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

/*
 *  Appdomain 在ILHotFix期间生效
 *  其他时间全部使用Assembly
 *  方便调试
 */

public class InitJEngine : MonoBehaviour
{
    public static InitJEngine Instance;

#if ILHotFix
    public static AppDomain Appdomain;
#else
    public static AppDomain Appdomain = null; // 编译通过，TODO
    private System.Reflection.Assembly assembly = null;
#endif


    public static bool Success;
    
#if UNITY_EDITOR
    public static long EncryptedCounts => ((JStream) (Instance._fs)).EncryptedCounts;
#endif

    private const string HotMainType = "HA.Program";
    private const string RunGameMethod = "RunGame";
	private const string SetupGameMethod = "SetupGame";

    //加密密钥
    [Tooltip("加密密钥，需要16位")] [FormerlySerializedAs("Key")] [SerializeField]
    public string key;

    //寄存器模式
    [Tooltip("ILRuntime寄存器模式")] [SerializeField]
    private ILRuntimeJITFlag useJIT = ILRuntimeJITFlag.JITOnDemand;
    //是否使用pdb，真机禁止
    [Tooltip("是否使用pdb，仅编辑器生效")] [FormerlySerializedAs("UsePdb")] [SerializeField]
    public bool usePdb;
    //是否允许debug（会产生log）
    [Tooltip("是否允许debug，勾选后产生log")] [FormerlySerializedAs("Debug")] [SerializeField]
    public bool debug = true;
    
    private Stream _fs;
    private Stream _pdb;


    private void Awake()
    {
        //单例
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //初始化Debug
        GameStats.Initialize();
        GameStats.Debug = debug;
    }

    /// <summary>
    /// 加载热更
    /// </summary>
    public void LoadHotUpdateCallback()
    {
        //加载热更DLL
        Instance.LoadHotFixAssembly();
        
        //调用SetupGame周期
        Tools.InvokeHotMethod(HotMainType, SetupGameMethod);
        
        //初始化ClassBind
        ClassBindMgr.Instantiate();
        
        //调用RunGame周期
        Tools.InvokeHotMethod(HotMainType, RunGameMethod);

        //调用在主工程的热更代码加载完毕后的周期
#if ILHotFix
        HotUpdateLoadedHelper.Init(Appdomain);
#else
        HotUpdateLoadedHelper.Init(null);
#endif
    }

    public T Instantiate<T>(string typeName)
    {
#if ILHotFix
        return Appdomain.Instantiate<T>(typeName);
#else
        return (T)assembly.CreateInstance(typeName);
#endif
    }

    public object Invoke(string typeName, string funcName, object inst, params object[] p)
    {
#if ILHotFix
        if (Appdomain.GetType(typeName).GetMethod(funcName, p != null ? p.Length : 0) == null)
        {
            Log.PrintError(string.Format("Invoke {0}.{1} not find", typeName, funcName));
        }

        return Appdomain.Invoke(typeName, funcName, inst , p);
#else
        Type type = assembly.GetType(typeName);
        return type?.GetMethod(funcName).Invoke(inst, p);
#endif
    }

    public IType GetType(string typeName)
    {
#if ILHotFix
        return Appdomain.GetType(typeName);
#else
        return (IType) assembly.GetType(typeName);
#endif
    }

    public object Invoke(string typeName, string funcName, params object[] p)
    {
#if ILHotFix
        if (Appdomain.GetType(typeName).GetMethod(funcName, p != null ? p.Length : 0) == null)
        {
            Log.PrintError(string.Format("Invoke {0}.{1} not find", typeName, funcName));
        }

        return Appdomain.Invoke(typeName, funcName, p, p);
#else
        Type type = assembly.GetType(typeName);
        return type?.GetMethod(funcName).Invoke(null, p);
#endif
    }

    public static object InvokeStatic(string typeName, string funcName, params object[] p)
    {
        Type type = Type.GetType(typeName);

        if (type != null)
        {
            return type.GetMethod(funcName).Invoke(null, p);
        }
        
        return Instance?.Invoke(typeName, funcName, p);
    }

    void LoadHotFixAssembly()
    {
        _pdb = null;
        byte[] dll = null, pdb = null; // 二进制

        if (!AssetMgr.RuntimeMode)
        {
            dll = DLLMgr.FileToByte(DLLMgr.DllPath);
        } else
        {
            var dllFile = (TextAsset)AssetMgr.Load(DLLMgr.ABPath, typeof(TextAsset));
            if (dllFile == null) { Log.PrintError("DLL文件不存在"); return; }
            dll = dllFile.bytes;
        }

        //校验 pdb
        if (usePdb)
        {
            
            #if UNITY_EDITOR
            if ((File.GetLastWriteTime(DLLMgr.DllPath) - File.GetLastWriteTime(DLLMgr.PdbPath)).Seconds > 30)
            {
                Log.PrintWarning("PDB 文件可能存在异常");
            }
            #endif

            #if UNITY_EDITOR
            pdb = DLLMgr.FileToByte(DLLMgr.PdbPath);
			#else
            var pdbFile = (TextAsset)AssetMgr.Load(DLLMgr.PdbPath, typeof(TextAsset));
            if (pdbFile != null) pdb = pdbFile.bytes;
			#endif

            if (pdb != null) _pdb = new MemoryStream(pdb); else Log.PrintWarning("pdb 文件不存在！");

        }

        
        if (!AssetMgr.RuntimeMode)
        {
#if ILHotFix
            // 开发模式，使用ILRunTime，模拟加密
            dll = CryptoHelper.AesEncrypt(dll, key); 
#else
            //开发模式, 未打开ILRunTime, 返回程序集
            assembly = System.Reflection.Assembly.Load(dll, pdb);
            return;
#endif
        }

        // 生成缓冲区，复制加密dll
        var buffer = new byte[dll.Length];
        Array.Copy(dll, buffer, dll.Length);

        // AssetMgr 使用的是全路径, 卸载资源
        if (!AssetMgr.RuntimeMode)
        {
            AssetMgr.Unload(AssetMgr.RuntimeMode ? DLLMgr.ABPath : DLLMgr.DllPath);
        }

        try
        {
#if ILHotFix
            //这里默认用分块解密，JStream
            _fs = new JStream(buffer, key);

            /*
             * 如果一定要直接解密然后不进行分块解密加载Dll，可以这样：
             * var original = CryptoHelper.AesDecrypt(dll.bytes, Key);
             * _fs = new JStream(original, Key);
             * _fs.Encrypted = false;
             */

            Appdomain = new AppDomain((int)useJIT);
            Appdomain.LoadAssembly(_fs, _pdb, new PdbReaderProvider());
#else
           var original = CryptoHelper.AesDecrypt(buffer, key);
           assembly = System.Reflection.Assembly.Load(original, pdb);
#endif
        }
        catch (Exception e)
        {
            Log.PrintError("加载热更DLL错误：\n" + e);
            if (!usePdb)
            {
                Log.PrintError(
                    "加载热更DLL失败，请确保HotUpdateResources/Dll里面有HotUpdateScripts.bytes文件，并且Build Bundle后将DLC传入服务器");
                Log.PrintError("也有可能是密码不匹配或密码包含特殊字符导致的");
            }
            else if (Application.isEditor && usePdb)
            {
                Log.PrintError("PDB不可用，可能是DLL和PDB版本不一致，可能DLL是Release，如果是Release出包，请取消UsePdb选项，本次已跳过使用PDB");
                usePdb = false;
                LoadHotFixAssembly();
            }

            return;
        }

        Success = true;

#if ILHotFix
        LoadILRuntime.InitializeILRuntime(Appdomain);
#endif
   }

    public static void HotReload()
    {
        Destroy(Instance.gameObject);
        SceneManager.LoadScene(0);
        AssetMgr.RemoveUnusedAssets();
        Success = false;
    }

    [Serializable]
    private enum ILRuntimeJITFlag
    {
        None = 0,

        /// <summary>
        /// Method will be JIT when method is called multiple time
        /// </summary>
        JITOnDemand = 1,

        /// <summary>
        /// Method will be JIT immediately when called, instead of progressively warm up
        /// </summary>
        JITImmediately = 2,

        /// <summary>
        /// Method will not be JIT when called
        /// </summary>
        NoJIT = 4,

        /// <summary>
        /// Method will always be inlined when called
        /// </summary>
        ForceInline = 8
    }
}
