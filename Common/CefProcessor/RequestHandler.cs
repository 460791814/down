using CefSharp;
using Common.dao;
using Common.model;
using Common.utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Common.CefProcessor
{
   public class RequestHandler : IRequestHandler
    {
        private static string _randomPath = string.Empty;

        private readonly Dictionary<UInt64, MemoryStreamResponseFilter> _responseDictionary =
            new Dictionary<UInt64, MemoryStreamResponseFilter>();

        bool IRequestHandler.OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame,
            string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            return false;
        }

        bool IRequestHandler.OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode,
            string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            //NOTE: If you do not wish to implement this method returning false is the default behaviour
            // We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.
            //callback.Dispose();
            //return false;

            //NOTE: When executing the callback in an async fashion need to check to see if it's disposed
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    //To allow certificate
                    //callback.Continue(true);
                    //return true;
                }
            }

            return false;
        }

        void IRequestHandler.OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
        {
        }

        CefReturnValue IRequestHandler.OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame,
            IRequest request, IRequestCallback callback)
        {
            return CefReturnValue.Continue;
        }

        bool IRequestHandler.GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame,
            bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            callback.Dispose();
            return false;
        }

        bool IRequestHandler.OnSelectClientCertificate(IWebBrowser browserControl, IBrowser browser, bool isProxy,
            string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            callback.Dispose();
            return false;
        }

        void IRequestHandler.OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser,
            CefTerminationStatus status)
        {
        }

        bool IRequestHandler.OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl,
            long newSize, IRequestCallback callback)
        {
            //NOTE: If you do not wish to implement this method returning false is the default behaviour
            // We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.
            //callback.Dispose();
            //return false;

            //NOTE: When executing the callback in an async fashion need to check to see if it's disposed
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    //Accept Request to raise Quota
                    //callback.Continue(true);
                    //return true;
                }
            }

            return false;
        }

        void IRequestHandler.OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame,
            IRequest request, IResponse response, ref string newUrl)
        {
        }

        bool IRequestHandler.OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url)
        {
            return url.StartsWith("mailto");
        }

        void IRequestHandler.OnRenderViewReady(IWebBrowser browserControl, IBrowser browser)
        {
        }

        bool IRequestHandler.OnResourceResponse(IWebBrowser browserControl, IBrowser browser, IFrame frame,
            IRequest request, IResponse response)
        {
            return false;
        }

        public IResponseFilter GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame,
            IRequest request,
            IResponse response)
        {
            if (response.Headers["Content-Type"] == "application/octet-stream")
            {
                var dataFilter = new MemoryStreamResponseFilter();
                _responseDictionary.Add(request.Identifier, dataFilter);
                return dataFilter;
            }

            return null;
        }

        public void OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request,
            IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            if (!request.Url.Contains("dl-download.csdn.net"))
            {
                return;
            }
            if (response.Headers["Content-Type"] == "application/octet-stream")
            {



                if (_responseDictionary.TryGetValue(request.Identifier, out var filter))
                {
                    var path = @"D:\webdata";
                    if (string.IsNullOrEmpty(_randomPath))
                    {
                       _randomPath = DateTime.Now.ToString("yyyyMMdd"); ;
                    }

                    string dirPath = Path.Combine(path, _randomPath);
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }

                    var data = filter.Data;
                    var dataLength = filter.Data.Length;
                    string query = new Uri(request.Url).Query;
                    query = query.Trim(new char[] { '?', ' ' });

                    IDictionary<string, string> queryDict = DataUtils.SplitUrlQuery(query);
                    string fileName = "";
                    string sourceId = "0";
                    int infoId = 0;
                    if (queryDict.TryGetValue("response-content-disposition", out fileName))
                    {
                        fileName = fileName.Replace("attachment;filename=", "").Trim('"');
                    }
                    if (queryDict.TryGetValue("sourceid", out sourceId))
                    {
                        infoId = StringUtils.GetInt(sourceId);
                    }
                    string fileType = System.IO.Path.GetExtension(fileName);

                  string filePath= Path.Combine(dirPath, sourceId+ fileType);
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        fs.Write(data, 0, dataLength);
                    }
                    //更新数据库
                    Down down = new Down();
                    down.filename = fileName;
                    down.infoid = infoId;
                    down.status = 1;
                    new DownDao().UpdateDown(down);

                    //                    NameValueCollection param = ExtractQueryParams(request.Url);
                    //                    if (String.IsNullOrEmpty(param["index"])) return;
                    //                    string fileName = Path.Combine(dirPath, param["index"].PadLeft(5, '0') + ".ts");
                    //                    using (FileStream fs = new FileStream(fileName, FileMode.Create))
                    //                    {
                    //                        fs.Write(data, 0, dataLength);
                    //                    }
                }
            }
        }

        private static NameValueCollection ExtractQueryParams(string url)
        {
            int startIndex = url.IndexOf("?", StringComparison.Ordinal);
            NameValueCollection values = new NameValueCollection();

            if (startIndex <= 0)
                return values;

            string[] nameValues = url.Substring(startIndex + 1).Split('&');

            foreach (string s in nameValues)
            {
                string[] pair = s.Split('=');

                string name = pair[0];
                string value = string.Empty;

                if (pair.Length > 1)
                    value = pair[1];

                values.Add(name, value);
            }

            return values;
        }

        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="filePaths">要合并的文件列表</param>
        /// <param name="combineFile">合并后的文件路径带文件名</param>
        void CombineFiles(string[] filePaths, string combineFile)
        {
            using (FileStream combineStream = new FileStream(combineFile, FileMode.OpenOrCreate))
            {
                using (BinaryWriter combineWriter = new BinaryWriter(combineStream))
                {
                    foreach (string file in filePaths)
                    {
                        using (FileStream fileStream = new FileStream(file, FileMode.Open))
                        {
                            using (BinaryReader fileReader = new BinaryReader(fileStream))
                            {
                                byte[] tempBytes = fileReader.ReadBytes((int)fileStream.Length);
                                combineWriter.Write(tempBytes);
                            }
                        }
                    }
                }
            }
        }

        public bool OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request,
            bool isRedirect)
        {
            return false;
        }

        public bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
        {
            return false;
        }

        public bool CanGetCookies(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            return true;
        }

        public bool CanSetCookie(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, Cookie cookie)
        {
            return true;
        }
    }
}