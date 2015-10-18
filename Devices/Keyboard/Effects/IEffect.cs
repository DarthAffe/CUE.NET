using System.Collections.Generic;
using CUE.NET.Devices.Keyboard.Brushes;
using CUE.NET.Devices.Keyboard.Keys;

namespace CUE.NET.Devices.Keyboard.Effects
{
    /// <summary>
    /// Represents a basic effect.
    /// </summary>
    public interface IEffect
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the list of keys to which the effect applies.
        /// </summary>
        IEnumerable<CorsairKey> KeyList { get; }

        /// <summary>
        /// Gets the brush which is drawn by the effect.
        /// </summary>
        IBrush EffectBrush { get; }

        /// <summary>
        /// Gets or sets the z-index of the effect to allow ordering them before drawing. (lowest first) (default: 0)
        /// </summary>
        int ZIndex { get; set; }

        /// <summary>
        /// Gets or sets if this effect has finished all of his work.
        /// </summary>
        bool IsDone { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the effect.
        /// </summary>
        /// <param name="deltaTime">The elapsed time (in seconds) since the last update.</param>
        void Update(float deltaTime);

        /// <summary>
        /// Hook which is called when the effect is attached to a keyboard.
        /// </summary>
        void OnAttach();

        /// <summary>
        /// Hook which is called when the effect is detached from a keyboard.
        /// </summary>
        void OnDetach();

        #endregion
    }
}
