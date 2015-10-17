// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;

namespace CUE.NET.Devices.Generic
{
    /// <summary>
    /// Represents the information supplied with an OnException-event.
    /// </summary>
    public class OnExceptionEventArgs : EventArgs
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the exception which is responsible for the event-call.
        /// </summary>
        public Exception Exception { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OnExceptionEventArgs"/> class.
        /// </summary>
        /// <param name="exception">The exception which is responsible for the event-call.</param>
        public OnExceptionEventArgs(Exception exception)
        {
            this.Exception = exception;
        }

        #endregion
    }
}
