// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Generic.EventArgs;

namespace CUE.NET.Devices
{
    /// <summary>
    /// Represents the event-handler of the Exception-event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="args">The arguments provided by the event.</param>
    public delegate void ExceptionEventHandler(object sender, ExceptionEventArgs args);

    /// <summary>
    /// Represents the event-handler of the Updating-event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="args">The arguments provided by the event.</param>
    public delegate void UpdatingEventHandler(object sender, UpdatingEventArgs args);

    /// <summary>
    /// Represents the event-handler of the Updated-event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="args">The arguments provided by the event.</param>
    public delegate void UpdatedEventHandler(object sender, UpdatedEventArgs args);

    /// <summary>
    /// Represents the event-handler of the LedsUpdating-event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="args">The arguments provided by the event.</param>
    public delegate void LedsUpdatingEventHandler(object sender, LedsUpdatingEventArgs args);

    /// <summary>
    /// Represents the event-handler of the LedsUpdated-event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="args">The arguments provided by the event.</param>
    public delegate void LedsUpdatedEventHandler(object sender, LedsUpdatedEventArgs args);

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

        // ReSharper disable EventNeverSubscribedTo.Global

        /// <summary>
        /// Occurs when a catched exception is thrown inside the device.
        /// </summary>
        event ExceptionEventHandler Exception;

        /// <summary>
        /// Occurs when the device starts updating.
        /// </summary>
        event UpdatingEventHandler Updating;

        /// <summary>
        /// Occurs when the device update is done.
        /// </summary>
        event UpdatedEventHandler Updated;

        /// <summary>
        /// Occurs when the device starts to update the leds.
        /// </summary>
        event LedsUpdatingEventHandler LedsUpdating;

        /// <summary>
        /// Occurs when the device updated the leds.
        /// </summary>
        event LedsUpdatedEventHandler LedsUpdated;

        // ReSharper restore EventNeverSubscribedTo.Global

        /// <summary>
        /// Perform an update for all dirty keys, or all keys if flushLeds is set to true.
        /// </summary>
        /// <param name="flushLeds">Specifies whether all keys (including clean ones) should be updated.</param>
        void Update(bool flushLeds = false);
    }
}
