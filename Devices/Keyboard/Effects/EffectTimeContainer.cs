// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace CUE.NET.Devices.Keyboard.Effects
{
    /// <summary>
    /// Represents a wrapped effect with additional time information.
    /// </summary>
    internal class EffectTimeContainer
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the wrapped effect.
        /// </summary>
        internal IEffect Effect { get; set; }

        /// <summary>
        /// Gets or sets the tick-count from the last time the effect was updated.
        /// </summary>
        internal long TicksAtLastUpdate { get; set; }

        /// <summary>
        /// Gets the z-index of the effect.
        /// </summary>
        internal int ZIndex => Effect?.ZIndex ?? 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectTimeContainer"/> class.
        /// </summary>
        /// <param name="effect">The wrapped effect.</param>
        /// <param name="ticksAtLastUpdate">The tick-count from the last time the effect was updated.</param>
        internal EffectTimeContainer(IEffect effect, long ticksAtLastUpdate)
        {
            this.Effect = effect;
            this.TicksAtLastUpdate = ticksAtLastUpdate;
        }

        #endregion
    }
}
