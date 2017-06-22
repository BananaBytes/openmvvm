namespace OpenMVVM.Web
{
    using System;
    using System.Threading.Tasks;

    public class ServiceResult<T>
    {
        protected ServiceResult(T value, DateTime valueTime, bool successful, int errorCode, string errorMessage, string errorDescription)
        {
            this.Value = value;
            this.ValueTime = valueTime;
            this.Successful = successful;
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
            this.ErrorDescription = errorDescription;
        }

        protected ServiceResult(Exception xcp)
        {
            this.Exception = xcp;
            this.ErrorCode = xcp.HResult;
            this.ErrorMessage = xcp.Message;
            this.ErrorDescription = xcp.StackTrace;
            this.Successful = false;
        }

        protected ServiceResult(int errorCode, string errorMessage, string errorDescription)
        {
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
            this.ErrorDescription = errorDescription;
            this.Successful = false;
        }

        public T Value { get; protected set; }

        public DateTime ValueTime { get; protected set; }

        public bool Successful { get; protected set; }

        public Exception Exception { get; protected set; }

        public int ErrorCode { get; protected set; }

        public string ErrorMessage { get; protected set; }

        public string ErrorDescription { get; protected set; }

        public static implicit operator ServiceResult<T>(T value)
        {
            return Create(value);
        }

        public static implicit operator T(ServiceResult<T> result)
        {
            return result.Value;
        }

        public static ServiceResult<T> Create(T value, DateTime? valueTime = null, bool successful = true, int errorCode = 0, string errorMessage = null, string errorDescription = null)
        {
            return new ServiceResult<T>(value, valueTime ?? DateTime.Now, successful, errorCode, errorMessage, errorDescription);
        }

        public static ServiceResult<T> CreateError(Exception xcp)
        {
            return new ServiceResult<T>(xcp);
        }

        public static ServiceResult<T> CreateError(int errorCode, string errorMessage, string errorDescription)
        {
            return new ServiceResult<T>(errorCode, errorMessage, errorDescription);
        }
    }
}