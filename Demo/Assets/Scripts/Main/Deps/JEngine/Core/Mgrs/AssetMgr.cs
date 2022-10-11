using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BM;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace JEngine.Core
{
    public static class AssetMgr
    {
        public static bool RuntimeMode => AssetComponentConfig.AssetLoadMode != AssetLoadMode.Develop;

        public static Object Load(string path, Type type = null)
        {
            return Load(path, null, type);
        }

        public static T Load<T>(string path, Type type = null)
            where T : Object
        {
            return Load<T>(path, null, type);
        }

        public static Object Load(string path, string package = null, Type type = null)
        {
            AddCache(path, package);
            return AssetComponent.Load(path, package);
        }

        public static T Load<T>(string path, string package = null, Type type = null)
            where T : Object
        {
            AddCache(path, package);
            return AssetComponent.Load<T>(path, package);
        }
        
        public static async Task<Object> LoadAsync(string path, Type type = null)
        {
            return await LoadAsync(path, null, type);
        }

        public static async Task<Object> LoadAsync(string path, string package = null, Type type = null)
        {
            AddCache(path, package);
            return await AssetComponent.LoadAsync(path, package);
        }

        public static async Task<T> LoadAsync<T>(string path, Type type = null)
            where T : Object
        {
            return await LoadAsync<T>(path, null, type);
        }

        public static async Task<T> LoadAsync<T>(string path, string package = null, Type type = null)
            where T : Object
        {
            AddCache(path, package);
            return await AssetComponent.LoadAsync<T>(path, package);
        }

        public static void Unload(string path, string package = null)
        {
            package = string.IsNullOrEmpty(package) ? AssetComponentConfig.DefaultBundlePackageName : package;
            RemoveCache(path, package);
            AssetComponent.UnLoadByPath(path, package);
        }

        public static void LoadScene(string path, bool additive, string package = null)
        {
            AssetComponent.LoadScene(path, package);
            var scene = Path.GetFileName(path).Split('.')[0];
            if (additive)
                SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            else
                SceneManager.LoadScene(scene);
            RemoveUnusedAssets();
        }

        public static async void LoadSceneAsync(string path, bool additive, string package = null,
            Action<float> loadingCallback = null,
            Action<AsyncOperation> finishedCallback = null)
        {
            await AssetComponent.LoadSceneAsync(path, package);
            var scene = Path.GetFileName(path).Split('.')[0];
            AsyncOperation operation = additive
                ? SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive)
                : SceneManager.LoadSceneAsync(scene);
            operation.allowSceneActivation = false;
            while (!operation.isDone && operation.progress < 0.9f)
            {
                loadingCallback?.Invoke(operation.progress);
                await Task.Delay(1);
            }

            loadingCallback?.Invoke(1);
            operation.allowSceneActivation = true;
            operation.completed += asyncOperation =>
            {
                RemoveUnusedAssets();
                finishedCallback?.Invoke(asyncOperation);
            };
        }

        private static List<string> _cacheAssets = new List<string>();

        private static void AddCache(string path, string package)
        {
            package = string.IsNullOrEmpty(package) ? AssetComponentConfig.DefaultBundlePackageName : package;
            var item = $"{package}:{path}";
            if (_cacheAssets.Contains(item)) return;
            _cacheAssets.Add(item);
        }
        private static void RemoveCache(string path, string package)
        {
            package = string.IsNullOrEmpty(package) ? AssetComponentConfig.DefaultBundlePackageName : package;
            _cacheAssets.Remove($"{package}:{path}");
        }

        public static void RemoveUnusedAssets()
        {
            Log.Print($"卸载以下资源(分包名:资源路径)，{string.Join(",", _cacheAssets)}");
            for (int i = 0, cnt = _cacheAssets.Count; i < cnt; i++)
            {
                var fullname = _cacheAssets[i];
                var bundle = fullname.Split(':')[0];
                var path = fullname.Split(':')[1];
                Unload(path, bundle);
                i--;
                cnt--;
            }
        }
    }
}