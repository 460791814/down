using CefSharp;
using CefSharp.WinForms;
using Common.CefProcessor;
using Common.dao;
using Common.model;
using Common.utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsdnLogin
{
    public partial class LoginForm : Form
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        private ChromiumWebBrowser chromeBrowser;
        UserDao userDao = new UserDao();
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
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
            //// Add it to the form and fill it to the form window.
            chromeBrowser.FrameLoadEnd += Browser_FrameLoadEnd;
            this.panel1.Controls.Add(chromeBrowser);

            chromeBrowser.Dock = DockStyle.Fill;
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            var cookieManager = Cef.GetGlobalCookieManager();
            string url = e.Url;
            if (url.Contains("https://passport.csdn.net/login"))
            {
                //登陆加载完毕
                chromeBrowser.EvaluateScriptAsync("$('.main-select a')[1].click()");
                //间隔1000等前台渲染完毕
                Thread.Sleep(1000);
                //获取用户密码
                User user = userDao.GetUserExpired();
                if (user != null)
                {
                    //计数
                    userDao.UpdateRetryCount(user.id);
                    String script = "$('#all').val('" + user.username + "'); $('#password').val('" + user.pwd + "');$('button').removeAttr('disabled');$('button').click()";
                    chromeBrowser.EvaluateScriptAsync(script);
                }
                return;

            }
            //登陆后会跳到首页 判断是否是需要获取cookie的页面
            if (url.Contains("https://www.csdn.net"))
            {
                CookieVisitor visitor = new CookieVisitor();
                visitor.SendCookie += visitor_SendCookie;
                cookieManager.VisitAllCookies(visitor);
            }
        }
        /// 回调事件
        private void visitor_SendCookie(List<CsdnCookie> csdnCookies)
        {
            if (csdnCookies != null)
            {
                CsdnCookie userCookie = csdnCookies.Find(a => a.Name == "UserName");
                if (userCookie != null)
                {
                    string cookieStr = JsonConvert.SerializeObject(csdnCookies);
                    User user = new User();
                    user.username = userCookie.Value;
                    user.cookie = cookieStr;
                    user.status = 0;
                    user.retrycount = 0;
                    userDao.UpdateCookie(user);
                    NLogger.Default.Info("[获取到cookies] username:" + userCookie.Value + " cookies:" + cookieStr);
                }
                else
                {
                    NLogger.Default.Info("[没有获取到cookies]");
                }

            }
   
        }

        //定时每天执行
        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            User user = userDao.GetUserExpired();
            if (user != null)
            {
                chromeBrowser.Load("https://passport.csdn.net/login?code=public");
            }
        }

        private void btn_start_Click(object sender, EventArgs e)
        {

            //定时器
            timer.Enabled = true;
            timer.Interval = 10000;//执行间隔时间,单位为毫秒  
            timer.Start();
            //指定每天时间1执行
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer1_Elapsed);
            this.btn_start.Enabled = false;
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            timer.Stop();
            this.btn_start.Enabled = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            chromeBrowser.Load("https://passport.csdn.net/login?code=public");
        }
    }
}
