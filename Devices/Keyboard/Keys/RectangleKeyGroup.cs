// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Helper;

namespace CUE.NET.Devices.Keyboard.Keys
{
    /// <summary>
    /// Represents a keygroup containing keys which physically lay inside a rectangle.
    /// </summary>
    public class RectangleKeyGroup : BaseKeyGroup
    {
        #region Properties & Fields

        private IList<CorsairKey> _keyCache;

        private RectangleF _rectangle;
        /// <summary>
        /// Gets or sets the rectangle the keys should be taken from.
        /// </summary>
        public RectangleF Rectangle
        {
            get { return _rectangle; }
            set
            {
                _rectangle = value;
                _keyCache = null;
            }
        }

        /// <summary>
        /// Gets or sets the minimal percentage overlay a key must have with the <see cref="Rectangle" /> to be taken into the keygroup.
        /// </summary>
        public float MinOverlayPercentage { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleKeyGroup"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard this keygroup belongs to.</param>
        /// <param name="fromKey">They ID of the first key to calculate the rectangle of this keygroup from.</param>
        /// <param name="toKey">They ID of the second key to calculate the rectangle of this keygroup from.</param>
        /// <param name="minOverlayPercentage">(optional) The minimal percentage overlay a key must have with the <see cref="Rectangle" /> to be taken into the keygroup. (default: 0.5f)</param>
        /// <param name="autoAttach">(optional) Specifies whether this group should be automatically attached or not. (default: true)</param>
        public RectangleKeyGroup(CorsairKeyboard keyboard, CorsairKeyboardKeyId fromKey, CorsairKeyboardKeyId toKey, float minOverlayPercentage = 0.5f, bool autoAttach = true)
            : this(keyboard, keyboard[fromKey], keyboard[toKey], minOverlayPercentage, autoAttach)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleKeyGroup"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard this keygroup belongs to.</param>
        /// <param name="fromKey">They first key to calculate the rectangle of this keygroup from.</param>
        /// <param name="toKey">They second key to calculate the rectangle of this keygroup from.</param>
        /// <param name="minOverlayPercentage">(optional) The minimal percentage overlay a key must have with the <see cref="Rectangle" /> to be taken into the keygroup. (default: 0.5f)</param>
        /// <param name="autoAttach">(optional) Specifies whether this group should be automatically attached or not. (default: true)</param>
        public RectangleKeyGroup(CorsairKeyboard keyboard, CorsairKey fromKey, CorsairKey toKey, float minOverlayPercentage = 0.5f, bool autoAttach = true)
            : this(keyboard, RectangleHelper.CreateRectangleFromRectangles(fromKey.KeyRectangle, toKey.KeyRectangle), minOverlayPercentage, autoAttach)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleKeyGroup"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard this keygroup belongs to.</param>
        /// <param name="fromPoint">They first point to calculate the rectangle of this keygroup from.</param>
        /// <param name="toPoint">They second point to calculate the rectangle of this keygroup from.</param>
        /// <param name="minOverlayPercentage">(optional) The minimal percentage overlay a key must have with the <see cref="Rectangle" /> to be taken into the keygroup. (default: 0.5f)</param>
        /// <param name="autoAttach">(optional) Specifies whether this group should be automatically attached or not. (default: true)</param>
        public RectangleKeyGroup(CorsairKeyboard keyboard, PointF fromPoint, PointF toPoint, float minOverlayPercentage = 0.5f, bool autoAttach = true)
            : this(keyboard, RectangleHelper.CreateRectangleFromPoints(fromPoint, toPoint), minOverlayPercentage, autoAttach)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleKeyGroup"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard this keygroup belongs to.</param>
        /// <param name="rectangle">The rectangle of this keygroup.</param>
        /// <param name="minOverlayPercentage">(optional) The minimal percentage overlay a key must have with the <see cref="Rectangle" /> to be taken into the keygroup. (default: 0.5f)</param>
        /// <param name="autoAttach">(optional) Specifies whether this group should be automatically attached or not. (default: true)</param>
        public RectangleKeyGroup(CorsairKeyboard keyboard, RectangleF rectangle, float minOverlayPercentage = 0.5f, bool autoAttach = true)
            : base(keyboard, autoAttach)
        {
            this.Rectangle = rectangle;
            this.MinOverlayPercentage = minOverlayPercentage;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a list containing the keys from this group.
        /// </summary>
        /// <returns>The list containing the keys.</returns>
        protected override IList<CorsairKey> GetGroupKeys()
        {
            return _keyCache ?? (_keyCache = Keyboard.Where(x => RectangleHelper.CalculateIntersectPercentage(x.KeyRectangle, Rectangle) >= MinOverlayPercentage).ToList());
        }

        #endregion
    }
}
