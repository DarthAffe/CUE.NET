using System.Collections.Generic;
using CUE.NET.Brushes;
using CUE.NET.Devices;
using CUE.NET.Devices.Generic;
using CUE.NET.Effects;
using CUE.NET.Groups.Extensions;

namespace CUE.NET.Groups
{
    /// <summary>
    /// Represents a basic keygroup.
    /// </summary>
    public abstract class AbstractLedGroup : AbstractEffectTarget<ILedGroup>, ILedGroup
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the strongly-typed target used for the effect.
        /// </summary>
        protected override ILedGroup EffectTarget => this;

        /// <summary>
        /// Gets the device this ledgroup belongs to.
        /// </summary>
        public ICueDevice Device { get; }

        /// <summary>
        /// Gets or sets the brush which should be drawn over this group.
        /// </summary>
        public IBrush Brush { get; set; }

        /// <summary>
        /// Gets or sets the z-index of this keygroup to allow ordering them before drawing. (lowest first) (default: 0)
        /// </summary>
        public int ZIndex { get; set; } = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractLedGroup"/> class.
        /// </summary>
        /// <param name="device">The device this ledgroup belongs to.</param>
        /// <param name="autoAttach">Specifies whether this group should be automatically attached or not.</param>
        protected AbstractLedGroup(ICueDevice device, bool autoAttach = true)
        {
            this.Device = device;

            if (autoAttach)
                this.Attach();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a list containing all LEDs of this group.
        /// </summary>
        /// <returns>The list containing all LEDs of this group.</returns>
        public abstract IEnumerable<CorsairLed> GetLeds();

        #endregion
    }
}
