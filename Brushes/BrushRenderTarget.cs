// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Drawing;
using CUE.NET.Devices.Keyboard.Enums;

namespace CUE.NET.Brushes
{
    public class BrushRenderTarget
    {
        #region Properties & Fields

        public CorsairKeyboardKeyId Key { get; }

        public PointF Point { get; }

        #endregion

        #region Constructors

        public BrushRenderTarget(CorsairKeyboardKeyId key, PointF point)
        {
            this.Point = point;
            this.Key = key;
        }

        #endregion
    }
}
