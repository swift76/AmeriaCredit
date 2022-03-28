using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntelART.WebApiRequestProxy
{
    public static class WebApiRequestProxyExtensions
    {
        public static WebApiRequestProxyBuilder UseWebApiRequestProxy(this IApplicationBuilder builder)
        {
            WebApiRequestProxyBuilder requestProxyBuilder = new WebApiRequestProxyBuilder();
            builder.UseMiddleware<WebApiRequestProxy>(requestProxyBuilder);
            return requestProxyBuilder;
        }
    }
}
