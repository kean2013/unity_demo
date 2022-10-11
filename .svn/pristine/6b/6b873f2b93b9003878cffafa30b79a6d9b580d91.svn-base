using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using FairyGUI;
using System.Text;
using System.Security.Cryptography;
using System.Threading;
using System.Collections.Concurrent;
using System.Net.Http.Headers;
using JEngine.Core;

namespace HA
{
    public static class PhotoHelper
    {
        public static string PREFIX = "qc://";
        private static Task download;
        private static Dictionary<string, NTexture> gallery = new Dictionary<string, NTexture>();
        private static string ApplicationDataPath = Application.persistentDataPath + "/photoes/";
        static string qCloudHost = "ludoc-1306489946.cos.ap-nanjing.myqcloud.com";
        private static string lastest;

        public class AsyncImgRequest
        {
            public string key;
            public Action<NTexture,string> onSuccess;
            public Action<string,string> onFail;
            public string url => PREFIX + key;

            public AsyncImgRequest(string u, Action<NTexture,string> succ, Action<string, string> fail)
            {
                key = u; onSuccess = succ; onFail = fail;
            }

            public static string url4gload(long uid, int vid, string pr = "")
            {
                return PREFIX + pr + uid + "-" + (-vid);
            }

            public static string key2www(string key)
            {
                return key.Substring(0, key.LastIndexOf('-')) + ".jpeg";
            }

            public static string gload2key(string url)
            {
                return url.Substring(PREFIX.Length);
            }

            public static string key2local(string key)
            {
                return key + ".jpeg";
            }
        }

        public static void TakePhoto(int maxSize, string destname, Action<Texture2D> callback)
        {
            NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
            {
                if (path != null)
                {
                    Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize);
                    if (texture == null)
                    {
                        Log.Print("Couldn't load texture from " + path);
                        return;
                    }

                    // Assign texture to a temporary quad and destroy it after 5 seconds
                    GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                    quad.transform.forward = Camera.main.transform.forward;
                    quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                    Material material = quad.GetComponent<Renderer>().material;
                    if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                        material.shader = Shader.Find("Legacy Shaders/Diffuse");

                    material.mainTexture = texture;

                    GameObject.Destroy(quad, 5f);

                    // If a procedural texture is not destroyed manually,
                    // it will only be freed after a scene change

                    CropImage(texture, destname, callback);
                    // GameObject.Destroy(texture, 5f);
                }
            }, maxSize);
        }

        public static void PickImage(int maxSize, string destname, Action<Texture2D> callback)
        {
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
            {
                if (path != null)
                {
                    // Create Texture from selected image
                    Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                    if (texture == null)
                    {
                        Log.Print("Couldn't load texture from " + path);
                        return;
                    }

                    // Assign texture to a temporary quad and destroy it after 5 seconds
                    GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                    quad.transform.forward = Camera.main.transform.forward;
                    quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                    Material material = quad.GetComponent<Renderer>().material;
                    if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                        material.shader = Shader.Find("Legacy Shaders/Diffuse");

                    material.mainTexture = texture;

                    GameObject.Destroy(quad, 5f);

                    // If a procedural texture is not destroyed manually,
                    // it will only be freed after a scene change

                    CropImage(texture, destname, callback);

                    // GameObject.Destroy(texture, 5f);
                }
            });
        }

        public static void CropImage(Texture2D screenshot, string destname, Action<Texture2D> callback)
        {
            bool ovalSelection = false;
            bool autoZoom = true;

            float minAspectRatio = 1.0f;
            float maxAspectRatio = 1.0f;

            ImageCropper.Instance.Show(screenshot,  async (bool result, Texture originalImage, Texture2D croppedImage) =>
            {
                // If screenshot was cropped successfully
                if (result)
                {
                    bool upload = await UploadHeaderImageAsync(destname, croppedImage.EncodeToJPG(100));
                    if (upload)
                    {
                        callback?.Invoke(croppedImage);
                    }

                    return;
                }

                // Destroy the screenshot as we no longer need it in this case
                GameObject.Destroy(screenshot);
            },
            settings: new ImageCropper.Settings()
            {
                ovalSelection = ovalSelection,
                autoZoomEnabled = autoZoom,
                imageBackground = Color.clear, // transparent background
                selectionMinAspectRatio = minAspectRatio,
                selectionMaxAspectRatio = maxAspectRatio,
                markTextureNonReadable = false
            },
            croppedImageResizePolicy: (ref int width, ref int height) =>
            {
                // uncomment lines below to save cropped image at half resolution
                //width /= 2;
                //height /= 2;
            }); ;
        }

        private static string qCloudUrlEncode(string content)
        {
            StringBuilder result = new StringBuilder(content.Length * 2);
            byte[] strToBytes = Encoding.UTF8.GetBytes(content);
            const string URLAllowChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
            foreach (byte b in strToBytes)
            {
                char ch = (char)b;
                if (URLAllowChars.IndexOf(ch) != -1)
                    result.Append(ch);
                else
                    result.Append('%').Append(String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:X2}", (int)b));
            }
            return result.ToString();
        }

        private static string qCloudHMACSHA1(string key, string content)
        {
            HMACSHA1 myHMACSHA1 = new HMACSHA1(Encoding.UTF8.GetBytes(key));
            byte[] signkey = myHMACSHA1.ComputeHash(Encoding.UTF8.GetBytes(content));

            var hexSign = new StringBuilder();
            foreach (byte b in signkey) hexSign.Append(b.ToString("x2"));
            return hexSign.ToString();
        }

        private static string qCloudSHA1(string content)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] result = sha1.ComputeHash(Encoding.UTF8.GetBytes(content));

            var hex = new StringBuilder();
            foreach (byte b in result) hex.Append(b.ToString("x2"));
            return hex.ToString();
        }

        private static string getQCloudSign(string host, string pathname, long lendata, string md5data, string method = "put")
        {
            string secretId = "AKIDKQGqAE1YoshFLDqDLD5k9QRBeroVw72h";
            string secretKey = "PYQ2PVLvEGPIvIOEn56qJWUBcmfNoAJB";

            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            string keytime = now + ";" + (now + 120);
            string signkey = qCloudHMACSHA1(secretKey, keytime);

            StringBuilder httpstr = new StringBuilder();
            httpstr.AppendFormat("{0}\n{1}\n\n", method, pathname);
            httpstr.AppendFormat("content-length={0}&content-md5={1}&host={2}\n", lendata, qCloudUrlEncode(md5data), host);

            StringBuilder str2sign = new StringBuilder();
            str2sign.AppendFormat("sha1\n{0}\n{1}\n", keytime, qCloudSHA1(httpstr.ToString()));
            string sign = qCloudHMACSHA1(signkey, str2sign.ToString());

            StringBuilder auth = new StringBuilder();
            auth.Append ("q-sign-algorithm=sha1&")
                .AppendFormat("q-ak={0}&", secretId)
                .AppendFormat("q-sign-time={0}&", keytime)
                .AppendFormat("q-key-time={0}&", keytime)
                .Append("q-header-list=content-length;content-md5;host&")
                .Append("q-url-param-list=&")
                .AppendFormat("q-signature={0}", sign);

            return auth.ToString();
        }

        
        public static NTexture Bytes2NTexture(byte[] data)
        {
            Texture2D tex2d = new Texture2D(0, 0, TextureFormat.RGBA32, false);
            tex2d.LoadImage(data, true);
            if (tex2d == null) return null;
            return new NTexture(tex2d);
        }

        private static readonly ConcurrentQueue<AsyncImgRequest> queue = new ConcurrentQueue<AsyncImgRequest>();

        public static void LoadPhoto(string url4gload, Action<NTexture, string> onSuccess, Action<string, string> onFail)
        {
            string key = AsyncImgRequest.gload2key(url4gload);
            if (gallery.TryGetValue(key, out NTexture tex))
            {
                onSuccess?.Invoke(tex, url4gload);
                tex.AddRef();
                return;
            }

            // 任务为空或者是任务已经完成，则开启新任务
            if (download == null || download.IsCompleted )
            {
                download = new Task(async () =>
                {
                    while (queue.Count > 0)
                    {
                        if (queue.TryDequeue(out AsyncImgRequest one))
                        {

                           // 如果已经下载过了
                            if (gallery.TryGetValue(one.key, out NTexture ntex))
                            {
                                Loom.QueueOnMainThread(() =>
                                {
                                    if (ntex == null)
                                    {
                                        one.onFail?.Invoke("queue find it, but it's null", one.url);
                                        return;
                                    }

                                    ntex.AddRef();
                                    one.onSuccess?.Invoke(ntex, one.url);
                                });

                                return;
                            }

                            // 下载失败或者是未下载的
                            try
                            {
                                byte[] data = await getQCloudTextureAsync(one.key);
                                bool waiter = false;
                                Loom.QueueOnMainThread(() =>
                                {
                                    NTexture texture;

                                    if (data == null || (texture = Bytes2NTexture(data)) == null)
                                    {
                                        one.onFail?.Invoke("qcloud get null!", one.url);
                                        return;
                                    }

                                    // 主线程维护gallery，有可能会出现同一个文件下载两次的情况
                                    if (!gallery.ContainsKey(one.key)) gallery.Add(one.key, texture);
                                    waiter = true;

                                    texture.AddRef();
                                    one.onSuccess?.Invoke(texture, one.url);

                                });

                                if (data != null)
                                {
                                    SpinWait.SpinUntil(() => waiter, 50);
                                }

                            }
                            catch (Exception e)
                            {
                                Log.PrintError("Exception: " + e.Message);
                            }
                        }

                       // 7秒中之内是否有新任务，没有则退出
                       SpinWait.SpinUntil(() => queue.Count > 0, 7000);
                    }
                });
            }

            // 重复的任务
            queue.Enqueue(new AsyncImgRequest(key, onSuccess, onFail));
            
            //AsyncImgRequest item = new AsyncImgRequest(key, onSuccess, onFail);
            //if (item.key.CompareTo(lastest) != 0) {queue.Enqueue(item); lastest = item.key; }

            // 任务未执行，则开启
            if (download.Status == TaskStatus.Created || download.Status == TaskStatus.WaitingForActivation)
            {
                download.Start();
            }
        }

        public static async Task<byte[]> getQCloudTextureAsync(string key)
        {
            string pathname = ApplicationDataPath + AsyncImgRequest.key2local(key);
            if (!Directory.Exists(ApplicationDataPath))
            {
                Directory.CreateDirectory(ApplicationDataPath);
            }

            if (File.Exists(pathname))
            {
                FileStream file = new FileStream(pathname, FileMode.Open, FileAccess.Read);
                byte[] data = new byte[file.Length];

                await file.ReadAsync(data, 0, (int)file.Length);

                return data;
            }

            try
            {
                byte[] data = await DownloadHeaderImgAsyncByStyle(ApplicationDataPath, 
                    AsyncImgRequest.key2www(key), AsyncImgRequest.key2local(key));
                return data;
            }
            catch (Exception ex)
            {
                Log.PrintWarning("getQCloudTextureAsync Error: " +  key + ", Msg: " + ex.Message);
            }

            return null;
        }

        public static async Task<byte[]> DownloadHeaderImgAsyncByStyle(string localpath, string remotename, string localname)
        {
            string url = "https://" + qCloudHost + "/" + remotename + "!256";
            HttpClient httpClient = new HttpClient();
            byte[] data = await httpClient.GetByteArrayAsync(url);
            await SaveTexture2File(localpath + localname, data);
            return data;
        }


        public static async Task<bool> UploadHeaderImageAsync(string remoteFile, byte[] data)
        {
            MD5 md5 = MD5.Create();
            byte[] md5byte = md5.ComputeHash(data);

            HttpClient httpClient = new HttpClient();
            ByteArrayContent arrayContent = new ByteArrayContent(data);
            HttpRequestHeaders header = httpClient.DefaultRequestHeaders;

            arrayContent.Headers.ContentMD5 = md5byte;
            arrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            arrayContent.Headers.ContentLength = data.Length;

            header.Host = qCloudHost;
            string token = getQCloudSign(qCloudHost, "/" + remoteFile, data.Length, Convert.ToBase64String(md5byte));
            header.TryAddWithoutValidation("Authorization", token);

            string url = "https://" + qCloudHost + "/" + remoteFile;
            HttpResponseMessage response = await httpClient.PutAsync(url, arrayContent);
            if ( !response.IsSuccessStatusCode)
            {
                Log.Print("PutAsync file " + remoteFile + " Failure! Code:" + response.StatusCode + ", Msg:" + response.ToString());
            }

            return response.IsSuccessStatusCode;
        }

        public static async Task SaveTexture2File(string localpath, byte[] data)
        {
            FileStream file = new FileStream(localpath, FileMode.OpenOrCreate, FileAccess.Write);
            await file.WriteAsync(data, 0, (int)data.Length);
            file.Close();
        }
    }
}
