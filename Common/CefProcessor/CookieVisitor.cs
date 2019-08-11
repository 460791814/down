using CefSharp;
using Common.model;
using System;
using System.Collections.Generic;

namespace Common.CefProcessor
{
    public class CookieVisitor : ICookieVisitor
    {
        public event Action<List<CsdnCookie>> SendCookie;
        List<CsdnCookie> csdnCookies = new List<CsdnCookie>();

        public void Dispose()
        {

        }

        public bool Visit(CefSharp.Cookie cookie, int count, int total, ref bool deleteCookie)
        {
            if (cookie.Domain == ".csdn.net")
            {
                CsdnCookie csdnCookie = new CsdnCookie();
                csdnCookie.Domain = cookie.Domain;
                csdnCookie.Name = cookie.Name;
                csdnCookie.Value = cookie.Value;
                csdnCookie.Expires = cookie.Expires;

                //csdnCookie.Creation = cookie.Creation;
                //csdnCookie.HttpOnly = cookie.HttpOnly;
                //csdnCookie.LastAccess = csdnCookie.LastAccess;
                //csdnCookie.Path = cookie.Path;
                //csdnCookie.Secure = cookie.Secure;

                csdnCookies.Add(csdnCookie);
            }
            deleteCookie = false;
            if (count == total - 1)
            {
                if (SendCookie != null)
                {
                    SendCookie(csdnCookies);
                }
            }

            return true;
        }
    }
}
