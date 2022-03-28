using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace IntelART.WebApiRequestProxy
{
    public class SimleWebApiRequestProxyRule : WebApiRequestProxyRule
    {
        public SimleWebApiRequestProxyRule(PathString pathPrefix, string forwardBaseUrl, Action<HttpRequest, HttpClient> prepare)
            : base(
                  (r) => { return r.Path.StartsWithSegments(pathPrefix); },
                  (r) =>
                  {
                      PathString proxyPath = null;
                      r.Path.StartsWithSegments(pathPrefix, out proxyPath);
                      return new HttpRequestMessage(new HttpMethod(r.Method), string.Format("{0}{1}", proxyPath.Value, r.QueryString));
                  },
                  (r, c) =>
                  {
                      c.BaseAddress = new Uri(forwardBaseUrl);
                      prepare(r, c);
                  })
        {
        }
    }
}
