using System;

namespace CUE.NET.Devices.Generic
{
    public class OnExceptionEventArgs : EventArgs
    {
        #region Properties & Fields

        public Exception Exception { get; }

        #endregion

        #region Constructors

        public OnExceptionEventArgs(Exception exception)
        {
            this.Exception = exception;
        }

        #endregion
    }
}
