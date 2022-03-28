using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace IntelART.Ameria.LoanApplicationRestApi
{
    public class ServiceHelper
    {
        public XmlDocument GetServiceResult(string url
            , string action
            , int queryTimeout
            , string method
            , Dictionary<string, string> parameters
            , string requestNode = null
            , string requestNamespace = null
            , Dictionary<string, string> parentParameters = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "text/xml; charset=utf-8";
            request.Timeout = 1000 * queryTimeout;
            request.ReadWriteTimeout = request.Timeout;
            request.Method = "POST";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Headers.Add("SOAPAction", action);
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(GenerateSOAPBody(method, parameters, requestNode, requestNamespace, parentParameters));
            }
            WebResponse response = request.GetResponse();
            XmlDocument document = new XmlDocument();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                document.LoadXml(reader.ReadToEnd());
            }
            return document;
        }

        public string DecodeResponseXML(string source)
        {
            return source.Replace("&lt;", "<").Replace("&gt;", ">")
                .Replace("&amp;", "&")
                .Replace("&quot;", @"""")
                .Replace("&apos;", "'");
        }

        public string GetNodeValue(XmlDocument document, string nodePath, bool decorated = true)
        {
            string result = string.Empty;
            XmlNode node = document.SelectSingleNode(nodePath);
            if (node != null)
            {
                if (decorated)
                    result = RetrieveValue(node.InnerXml);
                else
                    result = node.InnerXml;
            }
            return result;
        }

        private string RetrieveValue(string value)
        {
            string output = value.Replace("<![CDATA[", string.Empty).Replace("]]>", string.Empty).Trim();
            output = output.Replace("<", "'");
            output = output.Replace(">", "'");
            return output;
        }

        private string GenerateSOAPBody(string method
                , Dictionary<string, string> parameters
                , string requestNode = null
                , string requestNamespace = null
                , Dictionary<string, string> parentParameters = null)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(@"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">");
            builder.Append(@"<s:Body>");
            builder.AppendFormat(@"<{0} xmlns=""http://tempuri.org/"">", method);
            if (!string.IsNullOrEmpty(requestNode))
            {
                builder.AppendFormat(@"<{0} xmlns:a=""{1}"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">", requestNode, requestNamespace);
            }
            foreach (string parameter in parameters.Keys)
            {
                builder.AppendFormat(@"<{2}{0}>{1}</{2}{0}>", parameter, parameters[parameter], string.IsNullOrEmpty(requestNamespace) ? string.Empty : "a:");
            }
            if (!string.IsNullOrEmpty(requestNode))
            {
                builder.AppendFormat(@"</{0}>", requestNode);
            }
            if (parentParameters != null && parentParameters.Count > 0)
            {
                foreach (string parameter in parentParameters.Keys)
                {
                    builder.AppendFormat(@"<{0}>{1}</{0}>", parameter, parentParameters[parameter]);
                }
            }
            builder.AppendFormat(@"</{0}>", method);
            builder.Append(@"</s:Body>");
            builder.Append(@"</s:Envelope>");
            return builder.ToString();
        }
    }
}
