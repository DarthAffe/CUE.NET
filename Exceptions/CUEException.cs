// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

using System;
using CUE.NET.Enums;

namespace CUE.NET.Exceptions
{
    public class CUEException : ApplicationException
    {
        #region Properties & Fields

        public CorsairError Error { get; }

        #endregion

        #region Constructors

        public CUEException(CorsairError error)
        {
            this.Error = error;
        }

        #endregion
    }
}
