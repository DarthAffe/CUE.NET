// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMethodReturnValue.Global

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Generic.EventArgs;
using CUE.NET.Effects;
using CUE.NET.Native;

namespace CUE.NET.Devices.Generic
{
    /// <summary>
    /// Represents a generic CUE-device. (keyboard, mouse, headset, ...)
    /// </summary>
    public abstract class AbstractCueDevice : ICueDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets generic information provided by CUE for the device.
        /// </summary>
        public IDeviceInfo DeviceInfo { get; }

        private UpdateMode _updateMode = UpdateMode.AutoOnEffect;
        /// <summary>
        /// Gets or sets the update-mode for the device.
        /// </summary>
        public UpdateMode UpdateMode
        {
            get { return _updateMode; }
            set
            {
                _updateMode = value;
                CheckUpdateLoop();
            }
        }

        /// <summary>
        /// Gets or sets the update-frequency in seconds. (Calculate by using '1f / updates per second')
        /// </summary>
        public float UpdateFrequency { get; set; } = 1f / 30f;

        /// <summary>
        /// Gets a dictionary containing all LEDs of the device.
        /// </summary>
        protected Dictionary<int, CorsairLed> Leds { get; } = new Dictionary<int, CorsairLed>();

        /// <summary>
        /// Indicates if the device has an active effect to deal with.
        /// </summary>
        protected abstract bool HasEffect { get; }

        /// <summary>
        /// 
        /// </summary>
        protected LinkedList<EffectTimeContainer> Effects { get; } = new LinkedList<EffectTimeContainer>();

        private CancellationTokenSource _updateTokenSource;
        private CancellationToken _updateToken;
        private Task _updateTask;

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

            CheckUpdateLoop();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the LED-Object with the specified id.
        /// </summary>
        /// <param name="ledId">The LED-Id to look for.</param>
        /// <returns></returns>
        protected CorsairLed GetLed(int ledId)
        {
            if (!Leds.ContainsKey(ledId))
                Leds.Add(ledId, new CorsairLed());

            return Leds[ledId];
        }

        /// <summary>
        /// Checks if automatic updates should occur and starts/stops the update-loop if needed.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the requested update-mode is not available.</exception>
        protected async void CheckUpdateLoop()
        {
            bool shouldRun;
            switch (UpdateMode)
            {
                case UpdateMode.Manual:
                    shouldRun = false;
                    break;
                case UpdateMode.AutoOnEffect:
                    shouldRun = HasEffect;
                    break;
                case UpdateMode.Continuous:
                    shouldRun = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (shouldRun && _updateTask == null) // Start task
            {
                _updateTokenSource?.Dispose();
                _updateTokenSource = new CancellationTokenSource();
                _updateTask = Task.Factory.StartNew(UpdateLoop, (_updateToken = _updateTokenSource.Token));
            }
            else if (!shouldRun && _updateTask != null) // Stop task
            {
                _updateTokenSource.Cancel();
                await _updateTask;
                _updateTask.Dispose();
                _updateTask = null;
            }
        }

        private void UpdateLoop()
        {
            while (!_updateToken.IsCancellationRequested)
            {
                long preUpdateTicks = DateTime.Now.Ticks;
                Update();
                int sleep = (int)((UpdateFrequency * 1000f) - ((DateTime.Now.Ticks - preUpdateTicks) / 10000f));
                if (sleep > 0)
                    Thread.Sleep(sleep);
            }
        }

        /// <summary>
        /// Performs an update for all dirty keys, or all keys if flushLeds is set to true.
        /// </summary>
        /// <param name="flushLeds">Specifies whether all keys (including clean ones) should be updated.</param>
        public void Update(bool flushLeds = false)
        {
            OnUpdating();

            DeviceUpdate();
            UpdateEffects();

            IList<KeyValuePair<int, CorsairLed>> ledsToUpdate = (flushLeds ? Leds : Leds.Where(x => x.Value.IsDirty)).ToList();

            foreach (CorsairLed led in Leds.Values)
                led.Update();

            UpdateLeds(ledsToUpdate);

            OnUpdated();
        }

        /// <summary>
        /// Empty method which can be overwritten to perform device specific updates.
        /// </summary>
        protected virtual void DeviceUpdate()
        { }

        private void UpdateEffects()
        {
            List<IEffect> effectsToRemove = new List<IEffect>();
            lock (Effects)
            {
                long currentTicks = DateTime.Now.Ticks;
                foreach (EffectTimeContainer effect in Effects.OrderBy(x => x.ZIndex))
                {
                    try
                    {
                        float deltaTime;
                        if (effect.TicksAtLastUpdate < 0)
                        {
                            effect.TicksAtLastUpdate = currentTicks;
                            deltaTime = 0f;
                        }
                        else
                            deltaTime = (currentTicks - effect.TicksAtLastUpdate) / 10000000f;

                        effect.TicksAtLastUpdate = currentTicks;
                        effect.Effect.Update(deltaTime);

                        ApplyEffect(effect.Effect);

                        if (effect.Effect.IsDone)
                            effectsToRemove.Add(effect.Effect);
                    }
                    // ReSharper disable once CatchAllClause
                    catch (Exception ex) { OnException(ex); }
                }
            }

            foreach (IEffect effect in effectsToRemove)
                DetachEffect(effect);
        }

        /// <summary>
        /// Applies the given effect to the device LEDs.
        /// </summary>
        /// <param name="effect">The effect to apply.</param>
        protected abstract void ApplyEffect(IEffect effect);

        private void UpdateLeds(ICollection<KeyValuePair<int, CorsairLed>> ledsToUpdate)
        {
            ledsToUpdate = ledsToUpdate.Where(x => x.Value.Color != Color.Transparent).ToList();

            OnLedsUpdating(ledsToUpdate);

            if (ledsToUpdate.Any()) // CUE seems to crash if 'CorsairSetLedsColors' is called with a zero length array
            {
                int structSize = Marshal.SizeOf(typeof(_CorsairLedColor));
                IntPtr ptr = Marshal.AllocHGlobal(structSize * ledsToUpdate.Count);
                IntPtr addPtr = new IntPtr(ptr.ToInt64());
                foreach (KeyValuePair<int, CorsairLed> led in ledsToUpdate)
                {
                    _CorsairLedColor color = new _CorsairLedColor
                    {
                        ledId = led.Key,
                        r = led.Value.Color.R,
                        g = led.Value.Color.G,
                        b = led.Value.Color.B
                    };

                    Marshal.StructureToPtr(color, addPtr, false);
                    addPtr = new IntPtr(addPtr.ToInt64() + structSize);
                }
                _CUESDK.CorsairSetLedsColors(ledsToUpdate.Count, ptr);
                Marshal.FreeHGlobal(ptr);
            }

            OnLedsUpdated(ledsToUpdate);
        }

        /// <summary>
        /// Attaches the given effect.
        /// </summary>
        /// <param name="effect">The effect to attach.</param>
        /// <returns><c>true</c> if the effect could be attached; otherwise, <c>false</c>.</returns>
        public bool AttachEffect(IEffect effect)
        {
            bool retVal = false;
            lock (Effects)
            {
                if (effect != null && Effects.All(x => x.Effect != effect))
                {
                    effect.OnAttach();
                    Effects.AddLast(new EffectTimeContainer(effect, -1));
                    retVal = true;
                }
            }

            CheckUpdateLoop();
            return retVal;
        }

        /// <summary>
        /// Detaches the given effect.
        /// </summary>
        /// <param name="effect">The effect to detached.</param>
        /// <returns><c>true</c> if the effect could be detached; otherwise, <c>false</c>.</returns>
        public bool DetachEffect(IEffect effect)
        {
            bool retVal = false;
            lock (Effects)
            {
                if (effect != null)
                {
                    EffectTimeContainer val = Effects.FirstOrDefault(x => x.Effect == effect);
                    if (val != null)
                    {
                        effect.OnDetach();
                        Effects.Remove(val);
                        retVal = true;
                    }
                }
            }
            CheckUpdateLoop();
            return retVal;
        }

        /// <summary>
        /// Resets all loaded LEDs back to default.
        /// </summary>
        internal void ResetLeds()
        {
            foreach (CorsairLed led in Leds.Values)
                led.Reset();
        }

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
                Updating?.Invoke(this, new UpdatingEventArgs());
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
        protected virtual void OnLedsUpdating(ICollection<KeyValuePair<int, CorsairLed>> updatingLeds)
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
        protected virtual void OnLedsUpdated(IEnumerable<KeyValuePair<int, CorsairLed>> updatedLeds)
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

        #endregion
    }
}
