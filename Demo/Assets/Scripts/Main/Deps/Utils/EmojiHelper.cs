using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using System;
using System.IO;
using System.Text;
using JEngine.Core;

#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#endif

namespace HA
{
    /// <summary>
    /// Use to load icons from asset bundle, and pool them
    /// </summary>
    public class EmojiHelper 
    {
        private static EmojiHelper _instance = null;
        public static EmojiHelper Instance { get { return _instance ?? (_instance = new EmojiHelper()); } }
        

        public const int POOL_CHECK_TIME = 120;
        public const int MAX_POOL_SIZE = 20;

        public static string PREFIX = "em://";
        private const string ASSET_PRIFIX = "Assets/Emoji/";

        private AssetBundle abEmoji = null;
        private Queue<LoadItem> _items = new Queue<LoadItem>();
        private Hashtable _pool = new Hashtable();

        public Dictionary<uint, Emoji> emojies = new Dictionary<uint, Emoji>();

        public static string CCode2Emoji(string cc)
        {
            const int emA = 0x1f1e6;
            char[] ccode = cc.ToCharArray();

            StringBuilder sb = new StringBuilder();
            for(int i=0; i<ccode.Length; ++i)
            {
                char C = Char.ToUpper(ccode[i]);
                if (C < 'A' || C > 'Z') continue; 
                int index = C - 'A';

                sb.Append((i==0?string.Empty:"-") + (emA + index).ToString("x"));
            }

            return PREFIX + sb.ToString() + ".png";
        }

        bool _initialized = false;
        bool _started = false;

        EmojiHelper()
        {
            if (!_initialized )
            {
                Loom.Current.StartCoroutine(Initialize(null));
                _initialized = true;
            }
        }

        public void LoadIcon(string url, Action<NTexture, string> onSuccess,  Action<string, string> onFail)
        {
            _items.Enqueue(new LoadItem(url, onSuccess, onFail));
            if (!_started) Run();
        }

        private string url2asset(string url)
        {
            // 将url解析成包中的实际地址
            if (url.StartsWith(PREFIX))
            {
                return ASSET_PRIFIX + url.Substring(PREFIX.Length);
            }
            return null;
        }

        private void makeDict(AssetBundle ab, int size = 36)
        {
            // 维护索引
            foreach (var it in ab.GetAllAssetNames())
            {
                string name = it.Substring(it.LastIndexOf('/') + 1);
                try
                {
                    if (name.IndexOf('-') < 0)
                    {
                        uint id = UInt32.Parse(name.Substring(0, name.IndexOfAny(new char[] { '.', '-' })),
                            System.Globalization.NumberStyles.HexNumber);
                        emojies.Add(id, new Emoji(PREFIX + name, size, size));
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("file:" + name + " msg:" + ex.Message.ToString());
                }
            }
        }

        public IEnumerator Initialize(Action callback)
        {
            if( abEmoji != null)
            {
                if (emojies.Count == 0) makeDict(abEmoji);

                callback?.Invoke();
                yield break;
            }

            yield return LoadFromLocalAB();
            if (abEmoji == null) yield break;

            // 字典
            if (emojies.Count == 0) makeDict(abEmoji, 24);

            // 回调
            callback?.Invoke();
            // 检查引用计数
            // GameHelper.StartCoroutine(FreeIcon());
        }

        IEnumerator LoadFromLocalAB()
        {
            if (abEmoji != null ) yield break;

            string filepath = PathHelper.EmojiABPath("emoji.unity3d");
            // string filepath = Path.Combine(Application.persistentDataPath, "emoji");

            var req = AssetBundle.LoadFromFileAsync(filepath);
            abEmoji = req.assetBundle;
        }

        IEnumerator LoadFromStreamAB()
        {
            if (abEmoji != null || _initialized) yield break;

            _initialized = true;
            string url = Path.Combine(Application.streamingAssetsPath, "emoji");
            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);
            yield return www.SendWebRequest();

            if (!www.isNetworkError && !www.isHttpError)
            {
                abEmoji = DownloadHandlerAssetBundle.GetContent(www);
            }
        }

        void Run()
        {
            _started = true;
            NTexture texture = null;
            while (true)
            {
                if (_items.Count == 0) break;

                // 取出一个
                LoadItem item = _items.Dequeue();
                if(item == null) continue;

                // 从缓冲区获得
                if (_pool.ContainsKey(item.url))
                {
                    texture = (NTexture)_pool[item.url];
                    texture.AddRef();

                    item.onSuccess?.Invoke(texture, item.url);
                    continue;
                }

                // 图片很小,直接同步加载
                Texture2D tex = abEmoji.LoadAsset<Texture2D>(url2asset(item.url));
                if (tex == null)
                {
                    item.onFail?.Invoke(item.url + " does not exist!", item.url);
                    continue;
                }

                // 赋值和计数
                texture = new NTexture(tex);
                texture.AddRef();

                item.onSuccess?.Invoke(texture, item.url);

                //对象池缓冲
                _pool[item.url] = texture;
            }

            _started = false;
        }

        void Dispose()
        {
            abEmoji.Unload(false);
        }

        IEnumerator FreeIcon()
        {
            //TODO 循环
            yield return new WaitForSeconds(POOL_CHECK_TIME);

            int cnt = _pool.Count;
            if (cnt > MAX_POOL_SIZE)
            {
                ArrayList toRemove = null;
                foreach (DictionaryEntry de in _pool)
                {
                    NTexture texture = (NTexture)de.Value;
                    if (texture.refCount == 0)
                    {
                        if (toRemove == null) toRemove = new ArrayList();

                        toRemove.Add((string)de.Key);
                        texture.Dispose();

                        //Debug.Log("free icon " + de.Key);
                        if (--cnt <= 8) break;
                    }
                }

                if (toRemove != null)
                {
                    foreach (string key in toRemove)  _pool.Remove(key);
                }
            }
        }

    }

    class LoadItem
    {
        public string url;
        public Action<NTexture, string> onSuccess;
        public Action<string, string> onFail;

        public LoadItem(string id, Action<NTexture, string> succ, Action<string, string> fail )
        { 
            url = id; onSuccess = succ; onFail = fail; 
        }
    }
}