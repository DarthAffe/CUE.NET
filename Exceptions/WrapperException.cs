using System;

namespace CUE.NET.Exceptions
{
    public class WrapperException : ApplicationException
    {
        #region Constructors

        public WrapperException(string message, Exception innerException = null)
            : base(message, innerException)
        { }

        #endregion
    }
}
