// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace CUE.NET.Devices.Keyboard.Effects
{
    internal class EffectTimeContainer
    {
        #region Properties & Fields

        internal IEffect Effect { get; set; }

        internal long TicksAtLastUpdate { get; set; }

        internal int ZIndex => Effect?.ZIndex ?? 0;

        #endregion

        #region Constructors

        internal EffectTimeContainer(IEffect effect, long ticksAtLastUpdate)
        {
            this.Effect = effect;
            this.TicksAtLastUpdate = ticksAtLastUpdate;
        }

        #endregion
    }
}
