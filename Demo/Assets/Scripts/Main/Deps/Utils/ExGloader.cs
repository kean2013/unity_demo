using FairyGUI;
using JEngine.Core;
using System;

namespace HA
{
    public class ExGloader : GLoader
    {
        public Action<NTexture> onSuccess;
        public Action<string> onFailure;

        protected override void LoadExternal()
        {
            // 开始外部载入，地址在url属性
            // 载入完成后调用OnExternalLoadSuccess, 载入失败调用OnExternalLoadFailed

            /*
             注意：如果是外部载入，在载入结束后，调用OnExternalLoadSuccess或OnExternalLoadFailed前，
             比较严谨的做法是先检查url属性是否已经和这个载入的内容不相符。
             如果不相符，表示loader已经被修改了。
             这种情况下应该放弃调用OnExternalLoadSuccess或OnExternalLoadFailed。
            */

            if (url.StartsWith(EmojiHelper.PREFIX))
            {
                EmojiHelper.Instance.LoadIcon(this.url, OnLoadSuccess, OnLoadFail);
            } else if ( url.StartsWith (PhotoHelper.PREFIX))
            {
                PhotoHelper.LoadPhoto(this.url, OnLoadSuccess, OnLoadFail);
            }
        }

        protected override void FreeExternal(NTexture texture)
        {
            texture.ReleaseRef();
        }

        void OnLoadSuccess(NTexture texture, string exurl)
        {
            if (string.IsNullOrEmpty(this.url)) return;

            if (this.url.CompareTo(exurl) != 0)
            {
                Log.Print(exurl + " is not same as " + url);
                return;
            }

            onSuccess?.Invoke(texture);

            this.onExternalLoadSuccess(texture);
        }

        void OnLoadFail(string error, string exurl)
        {
            if (!string.IsNullOrEmpty(error))
            {
                Log.Print("load " + this.url + " failed: " + error);
            }

            if (this.url.CompareTo(exurl) != 0)
            {
                Log.Print(exurl + " is not same as " + url);
                return;
            }

            onFailure?.Invoke(error);
            this.onExternalLoadFailed();
        }
    }
}
