// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.Drawing;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Generic.EventArgs;
using CUE.NET.Groups;

namespace CUE.NET.Devices
{
    #region EventHandler

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

    #endregion

    /// <summary>
    /// Represents a generic cue device.
    /// </summary>
    public interface ICueDevice : ILedGroup, IEnumerable<CorsairLed>
    {
        #region Properties

        /// <summary>
        /// Gets generic information provided by CUE for the device.
        /// </summary>
        IDeviceInfo DeviceInfo { get; }

        /// <summary>
        /// Gets the rectangle containing all LEDs of the device.
        /// </summary>
        RectangleF DeviceRectangle { get; }

        /// <summary>
        /// Gets a read-only collection containing the LEDs of the device.
        /// </summary>
        IEnumerable<CorsairLed> Leds { get; }

        #endregion

        #region Indexer

        /// <summary>
        /// Gets the <see cref="CorsairLed" /> with the specified ID.
        /// </summary>
        /// <param name="ledId">The ID of the LED to get.</param>
        /// <returns>The LED with the specified ID or null if no LED is found.</returns>
        CorsairLed this[CorsairLedId ledId] { get; }

        /// <summary>
        /// Gets the <see cref="CorsairLed" /> at the given physical location.
        /// </summary>
        /// <param name="location">The point to get the location from.</param>
        /// <returns>The LED at the given point or null if no location is found.</returns>
        CorsairLed this[PointF location] { get; }

        /// <summary>
        /// Gets a list of <see cref="CorsairLed" /> inside the given rectangle.
        /// </summary>
        /// <param name="referenceRect">The rectangle to check.</param>
        /// <param name="minOverlayPercentage">The minimal percentage overlay a location must have with the <see cref="Rectangle" /> to be taken into the list.</param>
        /// <returns></returns>
        IEnumerable<CorsairLed> this[RectangleF referenceRect, float minOverlayPercentage = 0.5f] { get; }

        #endregion

        #region Events

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

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the device.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Perform an update for all dirty keys, or all keys if flushLeds is set to true.
        /// </summary>
        /// <param name="flushLeds">Specifies whether all keys (including clean ones) should be updated.</param>
        void Update(bool flushLeds = false);

        /// <summary>
        /// Attaches the given ledgroup.
        /// </summary>
        /// <param name="ledGroup">The ledgroup to attach.</param>
        /// <returns><c>true</c> if the ledgroup could be attached; otherwise, <c>false</c>.</returns>
        bool AttachLedGroup(ILedGroup ledGroup);

        /// <summary>
        /// Detaches the given ledgroup.
        /// </summary>
        /// <param name="ledGroup">The ledgroup to detached.</param>
        /// <returns><c>true</c> if the ledgroup could be detached; otherwise, <c>false</c>.</returns>
        bool DetachLedGroup(ILedGroup ledGroup);

        #endregion
    }
}
