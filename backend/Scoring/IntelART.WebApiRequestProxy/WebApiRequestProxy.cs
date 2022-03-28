using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace IntelART.WebApiRequestProxy
{
    public class WebApiRequestProxy
    {
        private RequestDelegate nextHandler;
        private WebApiRequestProxyBuilder builder;
        private ILoggerFactory loggerFactory;

        public WebApiRequestProxy(RequestDelegate nextHandler, WebApiRequestProxyBuilder builder, ILoggerFactory loggerFactory)
        {
            this.nextHandler = nextHandler;
            this.builder = builder;
            this.loggerFactory = loggerFactory;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            WebApiRequestProxyRule handlerRule = null;

            ILogger logger = this.loggerFactory.CreateLogger("IntelART.Proxy");

            logger.LogInformation("Invoking URL proxy");

            foreach (WebApiRequestProxyRule rule in this.builder.Rules)
            {
                if (rule.ShouldHanldeRequest(httpContext.Request))
                {
                    handlerRule = rule;
                    break;
                }
            }

            if (handlerRule != null)
            {
                logger.LogInformation(string.Format("Found URL forwarding rule for {0}. Forwarding to {1}.", httpContext.Request.Path, handlerRule.MapRequest(httpContext.Request).RequestUri));

                HttpRequestMessage requestMessage = handlerRule.MapRequest(httpContext.Request);

                if (httpContext.Request.ContentLength > 0)
                {
                    requestMessage.Content = new StreamContent(httpContext.Request.Body);
                }

                foreach (var h in httpContext.Request.Headers)
                {
                    string lowercaseKey = h.Key.ToLower();
                    string[] value = h.Value.ToArray();
                    switch (lowercaseKey)
                    {
                        case "allow":
                        case "content-disposition":
                        case "content-encoding":
                        case "content-language":
                        case "content-length":
                        case "content-location":
                        case "content-md5":
                        case "content-range":
                            if (requestMessage.Content != null)
                            {
                                requestMessage.Content.Headers.Add(h.Key, value);
                            }
                            break;
                        case "content-type":
                            if (requestMessage.Content != null)
                            {
                                if (value.Length > 0
                                && value[0].StartsWith("multipart/form-data"))
                                {
                                    requestMessage.Content.Headers.Add(h.Key, value);
                                }
                                else
                                {
                                    requestMessage.Content.Headers.Add(h.Key, "application/json");
                                }
                            }
                            break;
                        case "host":
                            break;
                        default:
                            requestMessage.Headers.Add(h.Key, value);
                            break;
                    }
                }

                HttpClient client = new HttpClient();
                handlerRule.ConfigureClient(httpContext.Request, client);
                logger.LogInformation(string.Format("Requesting {0} from {1}", requestMessage.RequestUri, client.BaseAddress));

                HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);

                logger.LogInformation(string.Format("Forwarded request returned code {0}", responseMessage.StatusCode));

                httpContext.Response.StatusCode = (int)responseMessage.StatusCode;
                foreach (var header in responseMessage.Headers)
                {
                    if (header.Key.ToLower() != "transfer-encoding")
                    {
                        httpContext.Response.Headers.Add(header.Key, new Microsoft.Extensions.Primitives.StringValues(header.Value.ToArray()));
                    }
                }

                ////httpContext.Response.ContentLength = responseMessage.Content.Headers.ContentLength;
                if (responseMessage.Content.Headers.ContentType != null)
                {
                    httpContext.Response.ContentType = responseMessage.Content.Headers.ContentType.ToString();
                }

                byte[] bytes = new byte[4096];
                int count = 0;
                Stream responseStream = await responseMessage.Content.ReadAsStreamAsync();
                httpContext.Response.RegisterForDispose(responseStream);
                while (true)
                {
                    count = await responseStream.ReadAsync(bytes, 0, 4096);
                    if (count <= 0)
                    {
                        break;
                    }
                    await httpContext.Response.Body.WriteAsync(bytes, 0, count);
                }
            }
            else
            {
                logger.LogInformation(string.Format("Unable to find URL forwarding rule for {0}.", httpContext.Request.Path));
                await nextHandler.Invoke(httpContext);
            }
        }
    }
}
