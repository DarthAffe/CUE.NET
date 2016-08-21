// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace CUE.NET.Effects
{
    /// <summary>
    /// Represents a wrapped effect with additional time information.
    /// </summary>
    public class EffectTimeContainer
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the wrapped effect.
        /// </summary>
        public IEffect Effect { get; }

        /// <summary>
        /// Gets or sets the tick-count from the last time the effect was updated.
        /// </summary>
        public long TicksAtLastUpdate { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectTimeContainer"/> class.
        /// </summary>
        /// <param name="effect">The wrapped effect.</param>
        /// <param name="ticksAtLastUpdate">The tick-count from the last time the effect was updated.</param>
        public EffectTimeContainer(IEffect effect, long ticksAtLastUpdate)
        {
            this.Effect = effect;
            this.TicksAtLastUpdate = ticksAtLastUpdate;
        }

        #endregion
    }
}
