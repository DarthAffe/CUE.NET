using System.Drawing;
using System.Linq;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Helper;

namespace CUE.NET.Devices.Keyboard.Keys
{
    public class RectangleKeyGroup : BaseKeyGroup
    {
        #region Properties & Fields

        public RectangleF RequestedRectangle { get; }
        public float MinOverlayPercentage { get; }

        #endregion

        #region Constructors

        public RectangleKeyGroup(CorsairKeyboard keyboard, CorsairKeyboardKeyId fromKey, CorsairKeyboardKeyId toKey, float minOverlayPercentage = 0.5f)
            : this(keyboard, keyboard[fromKey], keyboard[toKey], minOverlayPercentage)
        { }

        public RectangleKeyGroup(CorsairKeyboard keyboard, CorsairKey fromKey, CorsairKey toKey, float minOverlayPercentage = 0.5f)
            : this(keyboard, RectangleHelper.CreateRectangleFromRectangles(fromKey.KeyRectangle, toKey.KeyRectangle), minOverlayPercentage)
        { }

        public RectangleKeyGroup(CorsairKeyboard keyboard, PointF fromPoint, PointF toPoint, float minOverlayPercentage = 0.5f)
            : this(keyboard, RectangleHelper.CreateRectangleFromPoints(fromPoint, toPoint), minOverlayPercentage)
        { }

        public RectangleKeyGroup(CorsairKeyboard keyboard, RectangleF requestedRectangle, float minOverlayPercentage = 0.5f)
            : base(keyboard)
        {
            this.RequestedRectangle = requestedRectangle;
            this.MinOverlayPercentage = minOverlayPercentage;

            foreach (CorsairKey key in Keyboard.Where(x => RectangleHelper.CalculateIntersectPercentage(x.KeyRectangle, requestedRectangle) >= minOverlayPercentage))
                GroupKeys.Add(key);
        }

        #endregion

        #region Methods

        #endregion
    }
}
