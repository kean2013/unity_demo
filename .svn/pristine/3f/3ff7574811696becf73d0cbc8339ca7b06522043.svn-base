using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace JEngine.Core
{
    public class JAwaiter<T> : INotifyCompletion
    {
        public JAwaiter<T> GetAwaiter()
        {
            return this;
        }

        bool _isDone;
        Exception _exception;
        Action _continuation;
        T _result;

        public bool IsCompleted
        {
            get { return _isDone; }
        }

        public T GetResult()
        {
            return _result;
        }

        public void Complete(T result, Exception e)
        {
            _isDone = true;
            _exception = e;
            _result = result;

            _continuation?.Invoke();
        }
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            _continuation = continuation;
        }
    }

    public class TestAwatier
    {
        public JAwaiter<Texture2D> GetTexture2D(string url)
        {
            var awaiter = new JAwaiter<Texture2D>();
            HA.JUIManager.Instance.StartCoroutine(LoadTexture2D(url, awaiter));
            return awaiter;
        }

        IEnumerator LoadTexture2D(string url, JAwaiter<Texture2D> awaiter)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
                
                awaiter.Complete(null, new Exception(request.error));
                yield break;
            }

            var texture = DownloadHandlerTexture.GetContent(request);
            awaiter.Complete(texture, null);
        }

        async public void Test()
        {
            const string prefix = "http://ludoc-1306489946.cos.ap-nanjing.myqcloud.com/";
            try
            {
                Texture2D tex = await GetTexture2D(prefix + "1011.jpeg");
                await HA.PhotoHelper.SaveTexture2File(Application.persistentDataPath + "/1011.jpeg", tex.EncodeToJPG(100));
            }catch(Exception e)
            {
                Log.PrintError(e.Message);
            }
        }
    }
}
