using System;

namespace CUE.NET.Exceptions
{
    /// <summary>
    /// Represents an exception thrown by this SDK-Wrapper.
    /// </summary>
    public class WrapperException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WrapperException"/> class.
        /// </summary>
        /// <param name="message">The message which describes the reason of throwing this exception.</param>
        /// <param name="innerException">Optional inner exception, which lead to this exception.</param>
        public WrapperException(string message, Exception innerException = null)
            : base(message, innerException)
        { }

        #endregion
    }
}
