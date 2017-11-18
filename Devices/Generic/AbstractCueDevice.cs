// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable VirtualMemberNeverOverridden.Global

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Generic.EventArgs;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Effects;
using CUE.NET.Groups;
using CUE.NET.Helper;
using CUE.NET.Native;

namespace CUE.NET.Devices.Generic
{
    /// <summary>
    /// Represents a generic CUE-device. (keyboard, mouse, headset, ...)
    /// </summary>
    public abstract class AbstractCueDevice : ICueDevice
    {
        #region Properties & Fields

        private static DateTime _lastUpdate = DateTime.Now;

        private Dictionary<CorsairLedId, CorsairColor> _colorDataSave;

        /// <summary>
        /// Gets generic information provided by CUE for the device.
        /// </summary>
        public IDeviceInfo DeviceInfo { get; }

        /// <summary>
        /// Gets the rectangle containing all LEDs of the device.
        /// </summary>
        public RectangleF DeviceRectangle { get; protected set; }

        /// <summary>
        /// Gets a dictionary containing all LEDs of the device.
        /// </summary>
        protected Dictionary<CorsairLedId, CorsairLed> LedMapping { get; } = new Dictionary<CorsairLedId, CorsairLed>();

        /// <summary>
        /// Gets a read-only collection containing the LEDs of the device.
        /// </summary>
        public IEnumerable<CorsairLed> Leds => new ReadOnlyCollection<CorsairLed>(LedMapping.Values.ToList());

        /// <summary>
        /// Gets a list of attached ledgroups.
        /// </summary>
        protected LinkedList<ILedGroup> LedGroups { get; } = new LinkedList<ILedGroup>();

        /// <summary>
        /// Gets or sets the background brush of the keyboard.
        /// If this is null the last saved color-data is used as background.
        /// </summary>
        public IBrush Brush { get; set; }

        /// <summary>
        /// Gets or sets the z-index of the background brush of the keyboard.<br />
        /// This value has absolutely no effect.
        /// </summary>
        public int ZIndex { get; set; } = 0;

        #region Indexers

        /// <summary>
        /// Gets the <see cref="CorsairLed" /> with the specified ID.
        /// </summary>
        /// <param name="ledId">The ID of the LED to get.</param>
        /// <returns>The LED with the specified ID or null if no LED is found.</returns>
        public CorsairLed this[CorsairLedId ledId]
        {
            get
            {
                CorsairLed key;
                return LedMapping.TryGetValue(ledId, out key) ? key : null;
            }
        }

        /// <summary>
        /// Gets the <see cref="CorsairLed" /> at the given physical location.
        /// </summary>
        /// <param name="location">The point to get the location from.</param>
        /// <returns>The LED at the given point or null if no location is found.</returns>
        public CorsairLed this[PointF location] => LedMapping.Values.FirstOrDefault(x => x.LedRectangle.Contains(location));

        /// <summary>
        /// Gets a list of <see cref="CorsairLed" /> inside the given rectangle.
        /// </summary>
        /// <param name="referenceRect">The rectangle to check.</param>
        /// <param name="minOverlayPercentage">The minimal percentage overlay a location must have with the <see cref="Rectangle" /> to be taken into the list.</param>
        /// <returns></returns>
        public IEnumerable<CorsairLed> this[RectangleF referenceRect, float minOverlayPercentage = 0.5f] => LedMapping.Values
            .Where(x => RectangleHelper.CalculateIntersectPercentage(x.LedRectangle, referenceRect) >= minOverlayPercentage);

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a catched exception is thrown inside the device.
        /// </summary>
        public event ExceptionEventHandler Exception;

        /// <summary>
        /// Occurs when the device starts updating.
        /// </summary>
        public event UpdatingEventHandler Updating;

        /// <summary>
        /// Occurs when the device update is done.
        /// </summary>
        public event UpdatedEventHandler Updated;

        /// <summary>
        /// Occurs when the device starts to update the leds.
        /// </summary>
        public event LedsUpdatingEventHandler LedsUpdating;

        /// <summary>
        /// Occurs when the device updated the leds.
        /// </summary>
        public event LedsUpdatedEventHandler LedsUpdated;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractCueDevice"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by CUE for the device.</param>
        protected AbstractCueDevice(IDeviceInfo info)
        {
            this.DeviceInfo = info;
        }

        #endregion

        #region Methods

        #region Initialize

        /// <summary>
        /// Initializes the device.
        /// </summary>
        public virtual void Initialize()
        {
            DeviceRectangle = RectangleHelper.CreateRectangleFromRectangles((this).Select(x => x.LedRectangle));
            SaveColors();
        }

        /// <summary>
        /// Initializes the LED-Object with the specified id.
        /// </summary>
        /// <param name="ledId">The LED-Id to initialize.</param>
        /// <param name="ledRectangle">The rectangle representing the position of the LED to initialize.</param>
        /// <returns></returns>
        protected CorsairLed InitializeLed(CorsairLedId ledId, RectangleF ledRectangle)
        {
            if (LedMapping.ContainsKey(ledId)) return null;

            CorsairLed led = new CorsairLed(this, ledId, ledRectangle);
            LedMapping.Add(ledId, led);
            return led;
        }

        /// <summary>
        /// Resets all loaded LEDs back to default.
        /// </summary>
        internal void ResetLeds()
        {
            foreach (CorsairLed led in LedMapping.Values)
                led.Reset();
        }

        #endregion

        #region Update

        /// <summary>
        /// Performs an update for all dirty keys, or all keys if flushLeds is set to true.
        /// </summary>
        /// <param name="flushLeds">Specifies whether all keys (including clean ones) should be updated.</param>
        /// <param name="noRender">Only updates the hardware-leds skippin effects and the render-pass. Only use this if you know what that means!</param>
        public void Update(bool flushLeds = false, bool noRender = false)
        {
            OnUpdating();

            if (!noRender)
            {
                // Update effects
                foreach (ILedGroup ledGroup in LedGroups)
                    ledGroup.UpdateEffects();

                // Render brushes
                if (Brush != null)
                    Render(this);
                else
                    ApplyColorData(_colorDataSave);

                foreach (ILedGroup ledGroup in LedGroups.OrderBy(x => x.ZIndex))
                    Render(ledGroup);
            }
            // Device-specific updates
            DeviceUpdate();

            // Send LEDs to SDK
            ICollection<LedUpateRequest> ledsToUpdate = (flushLeds ? LedMapping : LedMapping.Where(x => x.Value.IsDirty)).Select(x => new LedUpateRequest(x.Key, x.Value.RequestedColor)).ToList();
            foreach (LedUpateRequest updateRequest in ledsToUpdate)
                LedMapping[updateRequest.LedId].Update();

            UpdateLeds(ledsToUpdate);

            OnUpdated();
        }

        /// <summary>
        /// Performs device specific updates.
        /// </summary>
        protected virtual void DeviceUpdate()
        { }

        /// <summary>
        /// Renders a ledgroup.
        /// </summary>
        /// <param name="ledGroup">The led group to render.</param>
        // ReSharper disable once MemberCanBeMadeStatic.Local - idc
        protected virtual void Render(ILedGroup ledGroup)
        {
            if (ledGroup == null) return;

            IList<CorsairLed> leds = ledGroup.GetLeds().ToList();

            IBrush brush = ledGroup.Brush;
            if (brush == null) return;

            try
            {
                switch (brush.BrushCalculationMode)
                {
                    case BrushCalculationMode.Relative:
                        RectangleF brushRectangle = RectangleHelper.CreateRectangleFromRectangles(leds.Select(x => x.LedRectangle));
                        float offsetX = -brushRectangle.X;
                        float offsetY = -brushRectangle.Y;
                        brushRectangle.X = 0;
                        brushRectangle.Y = 0;
                        brush.PerformRender(brushRectangle, leds.Select(x => new BrushRenderTarget(x.Id, x.LedRectangle.Move(offsetX, offsetY))));
                        break;
                    case BrushCalculationMode.Absolute:
                        brush.PerformRender(DeviceRectangle, leds.Select(x => new BrushRenderTarget(x.Id, x.LedRectangle)));
                        break;
                    default:
                        throw new ArgumentException();
                }

                brush.UpdateEffects();
                brush.PerformFinalize();

                foreach (KeyValuePair<BrushRenderTarget, CorsairColor> renders in brush.RenderedTargets)
                    this[renders.Key.LedId].Color = renders.Value;
            }
            // ReSharper disable once CatchAllClause
            catch (Exception ex) { OnException(ex); }
        }

        private void UpdateLeds(ICollection<LedUpateRequest> updateRequests)
        {
            updateRequests = updateRequests.Where(x => x.Color != CorsairColor.Transparent).ToList();

            OnLedsUpdating(updateRequests);

            if (updateRequests.Any()) // CUE seems to crash if 'CorsairSetLedsColors' is called with a zero length array
            {
                int structSize = Marshal.SizeOf(typeof(_CorsairLedColor));
                IntPtr ptr = Marshal.AllocHGlobal(structSize * updateRequests.Count);
                IntPtr addPtr = new IntPtr(ptr.ToInt64());
                foreach (LedUpateRequest ledUpdateRequest in updateRequests)
                {
                    _CorsairLedColor color = new _CorsairLedColor
                    {
                        ledId = (int)ledUpdateRequest.LedId,
                        r = ledUpdateRequest.Color.R,
                        g = ledUpdateRequest.Color.G,
                        b = ledUpdateRequest.Color.B
                    };

                    Marshal.StructureToPtr(color, addPtr, false);
                    addPtr = new IntPtr(addPtr.ToInt64() + structSize);
                }
                _CUESDK.CorsairSetLedsColors(updateRequests.Count, ptr);
                Marshal.FreeHGlobal(ptr);
            }

            OnLedsUpdated(updateRequests);
        }

        /// <inheritdoc />
        public void SyncColors()
        {
            Dictionary<CorsairLedId, CorsairColor> colorData = GetColors();
            ApplyColorData(colorData);
            Update(true, true);
        }

        /// <inheritdoc />
        public void SaveColors()
        {
            _colorDataSave = GetColors();
        }

        /// <inheritdoc />
        public void RestoreColors()
        {
            ApplyColorData(_colorDataSave);
            Update(true, true);
        }

        private void ApplyColorData(Dictionary<CorsairLedId, CorsairColor> colorData)
        {
            if (colorData == null) return;

            foreach (KeyValuePair<CorsairLedId, CorsairColor> corsairColor in _colorDataSave)
                LedMapping[corsairColor.Key].Color = corsairColor.Value;
        }

        private Dictionary<CorsairLedId, CorsairColor> GetColors()
        {
            int structSize = Marshal.SizeOf(typeof(_CorsairLedColor));
            IntPtr ptr = Marshal.AllocHGlobal(structSize * LedMapping.Count);
            IntPtr addPtr = new IntPtr(ptr.ToInt64());
            foreach (KeyValuePair<CorsairLedId, CorsairLed> led in LedMapping)
            {
                _CorsairLedColor color = new _CorsairLedColor { ledId = (int)led.Value.Id };
                Marshal.StructureToPtr(color, addPtr, false);
                addPtr = new IntPtr(addPtr.ToInt64() + structSize);
            }
            _CUESDK.CorsairGetLedsColors(LedMapping.Count, ptr);

            IntPtr readPtr = ptr;
            Dictionary<CorsairLedId, CorsairColor> colorData = new Dictionary<CorsairLedId, CorsairColor>();
            for (int i = 0; i < LedMapping.Count; i++)
            {
                _CorsairLedColor ledColor = (_CorsairLedColor)Marshal.PtrToStructure(readPtr, typeof(_CorsairLedColor));
                colorData.Add((CorsairLedId)ledColor.ledId, new CorsairColor((byte)ledColor.r, (byte)ledColor.g, (byte)ledColor.b));

                readPtr = new IntPtr(readPtr.ToInt64() + structSize);
            }

            Marshal.FreeHGlobal(ptr);

            return colorData;
        }

        #endregion

        #region LedGroup

        /// <summary>
        /// Attaches the given ledgroup.
        /// </summary>
        /// <param name="ledGroup">The ledgroup to attach.</param>
        /// <returns><c>true</c> if the ledgroup could be attached; otherwise, <c>false</c>.</returns>
        public bool AttachLedGroup(ILedGroup ledGroup)
        {
            lock (LedGroups)
            {
                if (ledGroup == null || LedGroups.Contains(ledGroup)) return false;

                LedGroups.AddLast(ledGroup);
                return true;
            }
        }

        /// <summary>
        /// Detaches the given ledgroup.
        /// </summary>
        /// <param name="ledGroup">The ledgroup to detached.</param>
        /// <returns><c>true</c> if the ledgroup could be detached; otherwise, <c>false</c>.</returns>
        public bool DetachLedGroup(ILedGroup ledGroup)
        {
            lock (LedGroups)
            {
                if (ledGroup == null) return false;

                LinkedListNode<ILedGroup> node = LedGroups.Find(ledGroup);
                if (node == null) return false;

                LedGroups.Remove(node);
                return true;
            }
        }

        /// <summary>
        /// Gets a list containing all LEDs of this group.
        /// </summary>
        /// <returns>The list containing all LEDs of this group.</returns>
        public IEnumerable<CorsairLed> GetLeds()
        {
            return Leds;
        }

        #endregion

        #region Effects

        /// <summary>
        /// Gets a list of all active effects of this target.
        /// For this device this is always null.
        /// </summary>
        public IList<IEffect<ILedGroup>> Effects => null;

        /// <summary>
        /// NOT IMPLEMENTED: Effects can't be applied directly to the device. Add it to the Brush or create a ledgroup instead.
        /// </summary>
        public void UpdateEffects()
        {
            throw new NotSupportedException("Effects can't be applied directly to the device. Add it to the Brush or create a ledgroup instead.");
        }

        /// <summary>
        /// NOT IMPLEMENTED: Effects can't be applied directly to the device. Add it to the Brush or create a ledgroup instead.
        /// </summary>
        /// <param name="effect">The effect to add.</param>
        public void AddEffect(IEffect<ILedGroup> effect)
        {
            throw new NotSupportedException("Effects can't be applied directly to the device. Add it to the Brush or create a ledgroup instead.");
        }

        /// <summary>
        /// NOT IMPLEMENTED: Effects can't be applied directly to the device. Add it to the Brush or create a ledgroup instead.
        /// </summary>
        /// <param name="effect">The effect to remove.</param>
        public void RemoveEffect(IEffect<ILedGroup> effect)
        {
            throw new NotSupportedException("Effects can't be applied directly to the device. Add it to the Brush or create a ledgroup instead.");
        }

        #endregion

        #region EventCaller

        /// <summary>
        /// Handles the needed event-calls for an exception.
        /// </summary>
        /// <param name="ex">The exception previously thrown.</param>
        protected virtual void OnException(Exception ex)
        {
            try
            {
                Exception?.Invoke(this, new ExceptionEventArgs(ex));
            }
            catch
            {
                // Well ... that's not my fault
            }
        }

        /// <summary>
        /// Handles the needed event-calls before updating.
        /// </summary>
        protected virtual void OnUpdating()
        {
            try
            {
                long lastUpdateTicks = _lastUpdate.Ticks;
                _lastUpdate = DateTime.Now;
                Updating?.Invoke(this, new UpdatingEventArgs((DateTime.Now.Ticks - lastUpdateTicks) / 10000000f));
            }
            catch
            {
                // Well ... that's not my fault
            }
        }

        /// <summary>
        /// Handles the needed event-calls after an update.
        /// </summary>
        protected virtual void OnUpdated()
        {
            try
            {
                Updated?.Invoke(this, new UpdatedEventArgs());
            }
            catch
            {
                // Well ... that's not my fault
            }
        }

        /// <summary>
        /// Handles the needed event-calls before the leds are updated.
        /// </summary>
        protected virtual void OnLedsUpdating(ICollection<LedUpateRequest> updatingLeds)
        {
            try
            {
                LedsUpdating?.Invoke(this, new LedsUpdatingEventArgs(updatingLeds));
            }
            catch
            {
                // Well ... that's not my fault
            }
        }

        /// <summary>
        /// Handles the needed event-calls after the leds are updated.
        /// </summary>
        protected virtual void OnLedsUpdated(IEnumerable<LedUpateRequest> updatedLeds)
        {
            try
            {
                LedsUpdated?.Invoke(this, new LedsUpdatedEventArgs(updatedLeds));
            }
            catch
            {
                // Well ... that's not my fault
            }
        }
        #endregion

        #region IEnumerable 

        /// <summary>
        /// Returns an enumerator that iterates over all LEDs of the device.
        /// </summary>
        /// <returns>An enumerator for all LEDs of the device.</returns>
        public IEnumerator<CorsairLed> GetEnumerator()
        {
            return LedMapping.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates over all LEDs of the device.
        /// </summary>
        /// <returns>An enumerator for all LEDs of the device.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #endregion
    }
}
