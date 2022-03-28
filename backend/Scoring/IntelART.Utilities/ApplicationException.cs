using System;

namespace IntelART
{
    public class ApplicationException : Exception
    {
        public string ErrorCode { get; private set; }

        public ApplicationException()
            : this(null)
        {
        }

        public ApplicationException(string errorCode)
            : base()
        {
            this.ErrorCode = errorCode;
        }

        public ApplicationException(string errorCode, string message)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public ApplicationException(string errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            this.ErrorCode = errorCode;
        }
    }
}
