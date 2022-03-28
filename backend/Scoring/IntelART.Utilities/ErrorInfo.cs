using System;
using System.Collections.Generic;

namespace IntelART.Utilities
{
    public class ErrorInfo
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Data { get; set; }
        public string StackTrace { get; set; }
        public ErrorInfo Inner { get; set; }

        private ErrorInfo()
        {
        }

        public static ErrorInfo For(Exception ex)
        {
            ErrorInfo errorInfo = null;

            if (ex != null)
            {
                errorInfo = new ErrorInfo();

                if (ex is ApplicationException)
                {
                    errorInfo.ErrorCode = (ex as ApplicationException).ErrorCode;
                }
                errorInfo.Message = ex.Message;
                //errorInfo.StackTrace = ex.StackTrace;
                //if (ex.Data.Count > 0)
                //{
                //    List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                //    foreach (KeyValuePair<object, object> item in ex.Data)
                //    {
                //        if (item.Key != null)
                //        {
                //            list.Add(new KeyValuePair<string, string>(item.Key.ToString(), item.Value == null ? null : item.Value.ToString()));
                //        }
                //    }

                //    if (list.Count > 0)
                //    {
                //        errorInfo.Data = list;
                //    }
                //}

                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                list.Add(new KeyValuePair<string, string>("Message", ex.Message));
                errorInfo.Data = list;

                errorInfo.Inner = For(ex.InnerException);
            }

            return errorInfo;
        }
    }
}
