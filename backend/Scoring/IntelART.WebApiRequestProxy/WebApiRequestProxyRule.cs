using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace IntelART.WebApiRequestProxy
{
    public class WebApiRequestProxyRule
    {
        private Func<HttpRequest, bool> filter;
        private Func<HttpRequest, HttpRequestMessage> requestMapper;
        private Action<HttpRequest, HttpClient> prepare;

        public WebApiRequestProxyRule(Func<HttpRequest, bool> filter, Func<HttpRequest, HttpRequestMessage> requestMapper, Action<HttpRequest, HttpClient> prepare)
        {
            this.filter = filter;
            this.requestMapper = requestMapper;
            this.prepare = prepare ?? ((r, c) => { });
        }

        public WebApiRequestProxyRule(Func<HttpRequest, bool> filter, Func<HttpRequest, HttpRequestMessage> requestMapper)
            : this(filter, requestMapper, null)
        {
        }

        public bool ShouldHanldeRequest(HttpRequest request)
        {
            return this.filter(request);
        }

        public HttpRequestMessage MapRequest(HttpRequest request)
        {
            return this.requestMapper(request);
        }

        public void ConfigureClient(HttpRequest request, HttpClient client)
        {
            this.prepare(request, client);
        }
    }
}
