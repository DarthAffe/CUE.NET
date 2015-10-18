// ReSharper disable MemberCanBePrivate.Global

using System.Collections.Generic;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;

namespace CUE.NET.Effects
{
    /// <summary>
    /// Represents a basic effect.
    /// </summary>
    public abstract class AbstractEffect : IEffect
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the list of LEDSs to which the effect applies.
        /// </summary>
        public IEnumerable<CorsairLed> LedList { get; set; }

        /// <summary>
        /// Gets the brush which is drawn by the effect.
        /// </summary>
        public abstract IBrush EffectBrush { get; }

        /// <summary>
        /// Gets or sets the z-index of the brush to allow ordering them before drawing. (lowest first) (default: 0)
        /// </summary>
        public int ZIndex { get; set; } = 0;

        /// <summary>
        /// Gets or sets if this effect has finished all of his work.
        /// </summary>
        public bool IsDone { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the effect.
        /// </summary>
        /// <param name="deltaTime">The elapsed time (in seconds) since the last update.</param>
        public abstract void Update(float deltaTime);

        /// <summary>
        /// Hook which is called when the effect is attached to a device.
        /// </summary>
        public virtual void OnAttach()
        { }

        /// <summary>
        /// Hook which is called when the effect is detached from a device.
        /// </summary>
        public virtual void OnDetach()
        { }

        #endregion
    }
}
