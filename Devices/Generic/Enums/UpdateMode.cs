using CUE.NET.Effects;

namespace CUE.NET.Devices.Generic.Enums
{
    /// <summary>
    /// Contains list of available update modes.
    /// </summary>
    public enum UpdateMode
    {
        /// <summary>
        /// The device will not perform automatic updates. Updates will only occur if <see cref="CUE.NET.Devices.ICueDevice.Update" /> is called.
        /// </summary>
        Manual,

        /// <summary>
        /// The device will perform automatic updates at the rate set in <see cref="CUE.NET.Devices.ICueDevice.UpdateFrequency" />
        /// as long as an <see cref="IEffect" /> is attached.
        /// </summary>
        AutoOnEffect,

        /// <summary>
        /// The device will perform automatic updates at the rate set in <see cref="CUE.NET.Devices.ICueDevice.UpdateFrequency" />.
        /// </summary>
        Continuous
    }
}
