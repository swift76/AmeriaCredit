using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace IntelART.WebApiRequestProxy
{
    public class WebApiRequestProxyBuilder
    {
        private List<WebApiRequestProxyRule> rules;

        public IEnumerable<WebApiRequestProxyRule> Rules
        {
            get
            {
                return this.rules;
            }
        }

        public WebApiRequestProxyBuilder()
        {
            this.rules = new List<WebApiRequestProxyRule>();
        }

        public WebApiRequestProxyBuilder AddRule(Func<HttpRequest, bool> filter, Func<HttpRequest, HttpRequestMessage> requestMapper, Action<HttpRequest, HttpClient> prepare)
        {
            return this.AddRule(new WebApiRequestProxyRule(filter, requestMapper, prepare));
        }

        public WebApiRequestProxyBuilder AddRule(WebApiRequestProxyRule rule)
        {
            this.rules.Add(rule);
            return this;
        }
    }
}
