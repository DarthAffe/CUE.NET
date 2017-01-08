// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CUE.NET.Devices;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Helper;

namespace CUE.NET.Groups
{
    /// <summary>
    /// Represents a ledgroup containing LEDs which physically lay inside a rectangle.
    /// </summary>
    public class RectangleLedGroup : AbstractLedGroup
    {
        #region Properties & Fields

        private IList<CorsairLed> _ledCache;

        private RectangleF _rectangle;
        /// <summary>
        /// Gets or sets the rectangle the LEDs should be taken from.
        /// </summary>
        public RectangleF Rectangle
        {
            get { return _rectangle; }
            set
            {
                _rectangle = value;
                _ledCache = null;
            }
        }

        private float _minOverlayPercentage;
        /// <summary>
        /// Gets or sets the minimal percentage overlay a LED must have with the <see cref="Rectangle" /> to be taken into the ledgroup.
        /// </summary>
        public float MinOverlayPercentage
        {
            get { return _minOverlayPercentage; }
            set
            {
                _minOverlayPercentage = value;
                _ledCache = null;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleLedGroup"/> class.
        /// </summary>
        /// <param name="device">The device this ledgroup belongs to.</param>
        /// <param name="fromLed">They ID of the first LED to calculate the rectangle of this ledgroup from.</param>
        /// <param name="toLed">They ID of the second LED to calculate the rectangle of this ledgroup from.</param>
        /// <param name="minOverlayPercentage">(optional) The minimal percentage overlay a LED must have with the <see cref="Rectangle" /> to be taken into the ledgroup. (default: 0.5f)</param>
        /// <param name="autoAttach">(optional) Specifies whether this group should be automatically attached or not. (default: true)</param>
        public RectangleLedGroup(ICueDevice device, CorsairLedId fromLed, CorsairLedId toLed, float minOverlayPercentage = 0.5f, bool autoAttach = true)
            : this(device, device[fromLed], device[toLed], minOverlayPercentage, autoAttach)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleLedGroup"/> class.
        /// </summary>
        /// <param name="device">The device this ledgroup belongs to.</param>
        /// <param name="fromLed">They first LED to calculate the rectangle of this ledgroup from.</param>
        /// <param name="toLed">They second LED to calculate the rectangle of this ledgroup from.</param>
        /// <param name="minOverlayPercentage">(optional) The minimal percentage overlay a LED must have with the <see cref="Rectangle" /> to be taken into the ledgroup. (default: 0.5f)</param>
        /// <param name="autoAttach">(optional) Specifies whether this group should be automatically attached or not. (default: true)</param>
        public RectangleLedGroup(ICueDevice device, CorsairLed fromLed, CorsairLed toLed, float minOverlayPercentage = 0.5f, bool autoAttach = true)
            : this(device, RectangleHelper.CreateRectangleFromRectangles(fromLed.LedRectangle, toLed.LedRectangle), minOverlayPercentage, autoAttach)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleLedGroup"/> class.
        /// </summary>
        /// <param name="device">The device this ledgroup belongs to.</param>
        /// <param name="fromPoint">They first point to calculate the rectangle of this ledgroup from.</param>
        /// <param name="toPoint">They second point to calculate the rectangle of this ledgroup from.</param>
        /// <param name="minOverlayPercentage">(optional) The minimal percentage overlay a LED must have with the <see cref="Rectangle" /> to be taken into the ledgroup. (default: 0.5f)</param>
        /// <param name="autoAttach">(optional) Specifies whether this group should be automatically attached or not. (default: true)</param>
        public RectangleLedGroup(ICueDevice device, PointF fromPoint, PointF toPoint, float minOverlayPercentage = 0.5f, bool autoAttach = true)
            : this(device, RectangleHelper.CreateRectangleFromPoints(fromPoint, toPoint), minOverlayPercentage, autoAttach)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleLedGroup"/> class.
        /// </summary>
        /// <param name="device">The device this ledgroup belongs to.</param>
        /// <param name="rectangle">The rectangle of this ledgroup.</param>
        /// <param name="minOverlayPercentage">(optional) The minimal percentage overlay a LED must have with the <see cref="Rectangle" /> to be taken into the ledgroup. (default: 0.5f)</param>
        /// <param name="autoAttach">(optional) Specifies whether this group should be automatically attached or not. (default: true)</param>
        public RectangleLedGroup(ICueDevice device, RectangleF rectangle, float minOverlayPercentage = 0.5f, bool autoAttach = true)
            : base(device, autoAttach)
        {
            this.Rectangle = rectangle;
            this.MinOverlayPercentage = minOverlayPercentage;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a list containing all LEDs of this group.
        /// </summary>
        /// <returns>The list containing all LEDs of this group.</returns>
        public override IEnumerable<CorsairLed> GetLeds()
        {
            return _ledCache ?? (_ledCache = Device.Where(x => RectangleHelper.CalculateIntersectPercentage(x.LedRectangle, Rectangle) >= MinOverlayPercentage).ToList());
        }

        #endregion
    }
}
