using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.Models;

namespace KeqingNiuza.Service
{
    internal class ProxyService
    {
        private readonly ProxyServer _proxy;

        private readonly ExplicitProxyEndPoint _endPoint;

        public event EventHandler<string> GotWishlogUrl;

        public bool ProxyRunning => _proxy.ProxyRunning;


        public ProxyService()
        {
            _proxy = new ProxyServer();
            _endPoint = new ExplicitProxyEndPoint(IPAddress.Loopback, 10086);
            _proxy.BeforeRequest += ProxyServer_BeforeRequest;
            
        }

        private Task ProxyServer_BeforeRequest(object sender, Titanium.Web.Proxy.EventArguments.SessionEventArgs e)
        {
            var request = e.HttpClient.Request;
            Debug.WriteLine(request.Url);
            if (((request.Host == "webstatic.mihoyo.com")|| (request.Host == "webstatic-sea.hoyoverse.com")) 
                && ((request.RequestUri.AbsolutePath == "/hk4e/event/e20190909gacha-v2/index.html") 
                        ||(request.RequestUri.AbsolutePath == "/genshin/event/e20190909gacha-v2/index.html")))
            {
                //Regex regex = new Regex(@"lang=.+&device_type");
                //GotWishlogUrl(this, regex.Replace(request.Url, "lang=zh-cn&device_type"));
                GotWishlogUrl(this, request.Url);
            }
            return Task.CompletedTask;
        }
        public async Task<bool> StartProxyAsync()
        {
            bool result = false;
            if (!ProxyRunning)
            {
                _proxy.AddEndPoint(_endPoint);
                _proxy.Start();
                _proxy.SetAsSystemHttpProxy(_endPoint);
                _proxy.SetAsSystemHttpsProxy(_endPoint);
                result = true;
            }
            return result;
        }
        public async Task<bool> StopProxyAsync()
        {
            bool result = false;
            if (ProxyRunning)
            {
                _proxy.Stop();
                result = true;
            }
            _proxy.DisableAllSystemProxies();
            return result;
        }

    }
}
