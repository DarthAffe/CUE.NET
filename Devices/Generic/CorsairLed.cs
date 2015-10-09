// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Drawing;
using CUE.NET.Helper;

namespace CUE.NET.Devices.Generic
{
    public class CorsairLed
    {
        #region Properties & Fields

        public bool IsDirty => RequestedColor != _color;
        public bool IsUpdated { get; private set; }

        public Color RequestedColor { get; private set; } = Color.Transparent;

        private Color _color = Color.Transparent;
        public Color Color
        {
            get { return _color; }
            set
            {
                if (!IsLocked)
                {
                    RequestedColor = RequestedColor.Blend(value);
                    IsUpdated = true;
                }
            }
        }

        public bool IsLocked { get; set; } = false;

        #endregion

        #region Constructors

        internal CorsairLed() { }

        #endregion

        #region Methods

        internal void Update()
        {
            _color = RequestedColor;
            IsUpdated = false;
        }

        #endregion
    }
}
