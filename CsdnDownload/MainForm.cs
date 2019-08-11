using CefSharp;
using CefSharp.WinForms;
using Common.dao;
using Common.model;
using Common.utils;
using Common.CefProcessor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsdnDownload
{
    public partial class MainForm : Form
    {
        UserDao userDao = new UserDao();
        DownDao downDao = new DownDao();
        public ChromiumWebBrowser chromeBrowser;
        System.Timers.Timer timer = new System.Timers.Timer();
        public MainForm()
        {
            InitializeComponent();

        }

        void startDownload()
        {
            //先从数据库获取需要下载的资源地址
            string downUrl = "";
            Down down = downDao.GetDown();
            if (down != null)
            {
                downUrl = down.url;
            }
            //获取可用的账号密码及cookie
            User csdnCookie = userDao.GetUser();
            CookieCollection cookies = new CookieCollection();
            if (csdnCookie != null)
            {
                List<CsdnCookie> cookieList = JsonConvert.DeserializeObject<List<CsdnCookie>>(csdnCookie.cookie);
                foreach (var item in cookieList)
                {
                    System.Net.Cookie cookie = new System.Net.Cookie();
                    cookie.Domain = item.Domain;
                    cookie.Name = item.Name;
                    cookie.Value = item.Value;
                    if (item.Expires != null)
                    {
                        cookie.Expires = item.Expires.Value;
                    }
                    if (item.Domain == ".csdn.net")
                    {
                        cookies.Add(cookie);
                    }

                }

            }
            SetCefCookies("http://csdn.net", cookies);
            //打开需要下载文件的页面
            chromeBrowser.Load(downUrl);


        }



        private void button1_Click(object sender, EventArgs e)
        {
      
        }



        //以下是CefSharp设置Url Cookie 的具体使用方法：
        public static void SetCefCookies(string url, CookieCollection cookies)
        {
            //Cef.GetGlobalCookieManager().SetStoragePath(Environment.CurrentDirectory, true);
            foreach (System.Net.Cookie c in cookies)
            {
                var cookie = new CefSharp.Cookie
                {
                    Creation = DateTime.Now,
                    Domain = c.Domain,
                    Name = c.Name,
                    Value = c.Value,
                    Expires = c.Expires
                };
                Task<bool> task = Cef.GetGlobalCookieManager().SetCookieAsync(url, cookie);
                while (!task.IsCompleted)
                {
                    continue;
                }
                bool b = task.Result;
            }
        }
        public static void DeleteCefCookies(string url, CookieCollection cookies)
        {
            //Cef.GetGlobalCookieManager().SetStoragePath(Environment.CurrentDirectory, true);
            foreach (System.Net.Cookie c in cookies)
            {
                var cookie = new CefSharp.Cookie
                {
                    Creation = DateTime.Now,
                    Domain = c.Domain,
                    Name = c.Name,
                    Value = c.Value,
                    Expires = c.Expires
                };
                Task<int> task = Cef.GetGlobalCookieManager().DeleteCookiesAsync(url, c.Name);
                while (!task.IsCompleted)
                {
                    continue;
                }
                int b = task.Result;
            }
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            string url = e.Url;
            if (url.Contains("https://download.csdn.net/download"))
            {
                //点击立即下载
                String script = "$('.direct_download').click()";
                chromeBrowser.EvaluateScriptAsync(script);
                Thread.Sleep(3000);
                //点击弹窗中的下载
                script = "$('.js_download_btn').click()";
                chromeBrowser.EvaluateScriptAsync(script);
            }

        }




        private void btn_start_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            timer.Interval = 10000;//执行间隔时间,单位为毫秒  
            timer.Start();
            //指定每天时间1执行
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer1_Elapsed);
            this.btn_start.Enabled = false;
            NLogger.Default.Info("开始");
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            timer.Stop();
            this.btn_start.Enabled = true;
            NLogger.Default.Info("结束");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var setting = new CefSettings();
            // 设置语言
            setting.Locale = "zh-CN";
            //cef设置userAgent
            setting.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36";
            //配置浏览器路径
            setting.BrowserSubprocessPath = @"x86\CefSharp.BrowserSubprocess.exe";
            Cef.Initialize(setting, performDependencyCheck: true, browserProcessHandler: null);
            chromeBrowser = new ChromiumWebBrowser();
            //chromeBrowser = new ChromiumWebBrowser("https://passport.csdn.net/login?code=public");
            //// Add it to the form and fill it to the form window.

            chromeBrowser.FrameLoadEnd += Browser_FrameLoadEnd;
            chromeBrowser.RequestHandler = new RequestHandler();
            this.panel1.Controls.Add(chromeBrowser);

            chromeBrowser.Dock = DockStyle.Fill;
        }

        //定时每天执行
        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            startDownload();
        }

 

        private void btn_csdnurl_Click(object sender, EventArgs e)
        {
            chromeBrowser.Load("https://download.csdn.net/download/weixin_39840387/11522558");
        }

        private void btn_clearcookie_Click(object sender, EventArgs e)
        {
            //获取可用的账号密码及cookie
            User csdnCookie = userDao.GetUser();
            CookieCollection cookies = new CookieCollection();
            if (csdnCookie != null)
            {
                List<CsdnCookie> cookieList = JsonConvert.DeserializeObject<List<CsdnCookie>>(csdnCookie.cookie);
                foreach (var item in cookieList)
                {
                    System.Net.Cookie cookie = new System.Net.Cookie();
                    cookie.Domain = item.Domain;
                    cookie.Name = item.Name;
                    cookie.Value = item.Value;
                    if (item.Expires != null)
                    {
                        cookie.Expires = item.Expires.Value;
                    }
                    if (item.Domain == ".csdn.net")
                    {
                        cookies.Add(cookie);
                    }

                }

            }
            //删除cookies
            DeleteCefCookies("http://csdn.net", cookies);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            startDownload();
        }
    }
}
