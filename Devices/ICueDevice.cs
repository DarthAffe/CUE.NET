using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;

namespace CUE.NET.Devices
{
    /// <summary>
    /// Represents the event-handler of the OnException-event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="args">The arguments provided by the event.</param>
    public delegate void OnExceptionEventHandler(object sender, OnExceptionEventArgs args);

    /// <summary>
    /// Represents a generic cue device.
    /// </summary>
    public interface ICueDevice
    {
        /// <summary>
        /// Gets generic information provided by CUE for the device.
        /// </summary>
        IDeviceInfo DeviceInfo { get; }

        /// <summary>
        /// Gets or sets the update-mode for the device.
        /// </summary>
        UpdateMode UpdateMode { get; set; }

        /// <summary>
        /// Gets or sets the update-frequency in seconds. (Calculate by using '1f / updates per second')
        /// </summary>
        float UpdateFrequency { get; set; }

        // ReSharper disable once EventNeverSubscribedTo.Global
        /// <summary>
        /// Occurs when a catched exception is thrown inside the device.
        /// </summary>
        event OnExceptionEventHandler OnException;

        /// <summary>
        /// Perform an update for all dirty keys, or all keys if flushLeds is set to true.
        /// </summary>
        /// <param name="flushLeds">Specifies whether all keys (including clean ones) should be updated.</param>
        void Update(bool flushLeds = false);
    }
}
