namespace CUE.NET.Devices.Keyboard.Effects
{
    internal class EffectTimeContainer
    {
        #region Properties & Fields

        internal IEffect Effect { get; set; }

        internal long TicksAtLastUpdate { get; set; }

        #endregion

        #region Constructors

        internal EffectTimeContainer(IEffect effect, long ticksAtLastUpdate)
        {
            Effect = effect;
            TicksAtLastUpdate = ticksAtLastUpdate;
        }

        #endregion
    }
}
