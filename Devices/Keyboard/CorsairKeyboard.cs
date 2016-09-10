// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Devices.Keyboard.Keys;
using CUE.NET.Effects;
using CUE.NET.Helper;
using CUE.NET.Native;

namespace CUE.NET.Devices.Keyboard
{
    /// <summary>
    /// Represents the SDK for a corsair keyboard.
    /// </summary>
    public class CorsairKeyboard : AbstractCueDevice, IEnumerable<CorsairKey>, IKeyGroup
    {
        #region Properties & Fields

        #region Indexer

        /// <summary>
        /// Gets the <see cref="CorsairKey" /> with the specified ID.
        /// </summary>
        /// <param name="keyId">The ID of the key to get.</param>
        /// <returns>The key with the specified ID or null if no key is found.</returns>
        public CorsairKey this[CorsairKeyboardKeyId keyId]
        {
            get
            {
                CorsairKey key;
                return _keys.TryGetValue(keyId, out key) ? key : null;
            }
        }

        /// <summary>
        /// Gets the <see cref="CorsairKey" /> representing the given character by calling the SDK-method 'CorsairGetLedIdForKeyName'.<br />
        /// Note that this currently only works for letters.
        /// </summary>
        /// <param name="key">The character of the key.</param>
        /// <returns>The key representing the given character or null if no key is found.</returns>
        public CorsairKey this[char key]
        {
            get
            {
                CorsairKeyboardKeyId keyId = _CUESDK.CorsairGetLedIdForKeyName(key);
                CorsairKey cKey;
                return _keys.TryGetValue(keyId, out cKey) ? cKey : null;
            }
        }

        /// <summary>
        /// Gets the <see cref="CorsairKey" /> at the given physical location.
        /// </summary>
        /// <param name="location">The point to get the key from.</param>
        /// <returns>The key at the given point or null if no key is found.</returns>
        public CorsairKey this[PointF location] => _keys.Values.FirstOrDefault(x => x.Led.LedRectangle.Contains(location));

        /// <summary>
        /// Gets a list of <see cref="CorsairKey" /> inside the given rectangle.
        /// </summary>
        /// <param name="referenceRect">The rectangle to check.</param>
        /// <param name="minOverlayPercentage">The minimal percentage overlay a key must have with the <see cref="Rectangle" /> to be taken into the list.</param>
        /// <returns></returns>
        public IEnumerable<CorsairKey> this[RectangleF referenceRect, float minOverlayPercentage = 0.5f] => _keys.Values
            .Where(x => RectangleHelper.CalculateIntersectPercentage(x.Led.LedRectangle, referenceRect) >= minOverlayPercentage);

        #endregion

        private readonly LinkedList<IKeyGroup> _keyGroups = new LinkedList<IKeyGroup>();

        private Dictionary<CorsairKeyboardKeyId, CorsairKey> _keys = new Dictionary<CorsairKeyboardKeyId, CorsairKey>();

        /// <summary>
        /// Gets a read-only collection containing the keys of the keyboard.
        /// </summary>
        public IEnumerable<CorsairKey> Keys => new ReadOnlyCollection<CorsairKey>(_keys.Values.ToList());

        /// <summary>
        /// Gets specific information provided by CUE for the keyboard.
        /// </summary>
        public CorsairKeyboardDeviceInfo KeyboardDeviceInfo { get; }

        /// <summary>
        /// Gets the rectangle containing all keys of the keyboard.
        /// </summary>
        public RectangleF KeyboardRectangle { get; }

        /// <summary>
        /// Gets or sets the background brush of the keyboard.
        /// </summary>
        public IBrush Brush { get; set; }

        /// <summary>
        /// Gets or sets the z-index of the background brush of the keyboard.<br />
        /// This value has absolutely no effect.
        /// </summary>
        public int ZIndex { get; set; } = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairKeyboard"/> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the keyboard</param>
        internal CorsairKeyboard(CorsairKeyboardDeviceInfo info)
            : base(info)
        {
            this.KeyboardDeviceInfo = info;

            InitializeKeys();
            KeyboardRectangle = RectangleHelper.CreateRectangleFromRectangles(this.Select(x => x.Led.LedRectangle));
        }

        #endregion

        #region Methods

        #region Update

        /// <summary>
        /// Updates all brushes and groups on the keyboard.
        /// </summary>
        protected override void DeviceUpdate()
        {
            lock (_keyGroups)
            {
                foreach (IKeyGroup keyGroup in _keyGroups)
                    keyGroup.UpdateEffects();
            }

            if (Brush != null)
                ApplyBrush(this.ToList(), Brush);

            lock (_keyGroups)
            {
                foreach (IKeyGroup keyGroup in _keyGroups.OrderBy(x => x.ZIndex))
                    ApplyBrush(keyGroup.Keys.ToList(), keyGroup.Brush);
            }
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local - idc
        private void ApplyBrush(ICollection<CorsairKey> keys, IBrush brush)
        {
            try
            {
                switch (brush.BrushCalculationMode)
                {
                    case BrushCalculationMode.Relative:
                        RectangleF brushRectangle = RectangleHelper.CreateRectangleFromRectangles(keys.Select(x => x.Led.LedRectangle));
                        float offsetX = -brushRectangle.X;
                        float offsetY = -brushRectangle.Y;
                        brushRectangle.X = 0;
                        brushRectangle.Y = 0;
                        brush.PerformRender(brushRectangle, keys.Select(x => new BrushRenderTarget(x.KeyId, x.Led.LedRectangle.GetCenter(offsetX, offsetY))));
                        break;
                    case BrushCalculationMode.Absolute:
                        brush.PerformRender(KeyboardRectangle, keys.Select(x => new BrushRenderTarget(x.KeyId, x.Led.LedRectangle.GetCenter())));
                        break;
                    default:
                        throw new ArgumentException();
                }

                brush.UpdateEffects();
                brush.PerformFinalize();

                foreach (KeyValuePair<BrushRenderTarget, Color> renders in brush.RenderedTargets)
                    _keys[renders.Key.Key].Led.Color = renders.Value;
            }
            // ReSharper disable once CatchAllClause
            catch (Exception ex) { OnException(ex); }
        }

        /// <summary>
        /// Gets a list containing all LEDs of this group.
        /// </summary>
        /// <returns>The list containing all LEDs of this group.</returns>
        public IEnumerable<CorsairLed> GetLeds()
        {
            return this.Select(x => x.Led);
        }

        #endregion

        /// <summary>
        /// Attaches the given keygroup.
        /// </summary>
        /// <param name="keyGroup">The keygroup to attach.</param>
        /// <returns><c>true</c> if the keygroup could be attached; otherwise, <c>false</c>.</returns>
        public bool AttachKeyGroup(IKeyGroup keyGroup)
        {
            lock (_keyGroups)
            {
                if (keyGroup == null || _keyGroups.Contains(keyGroup)) return false;

                _keyGroups.AddLast(keyGroup);
                return true;
            }
        }

        /// <summary>
        /// Detaches the given keygroup.
        /// </summary>
        /// <param name="keyGroup">The keygroup to detached.</param>
        /// <returns><c>true</c> if the keygroup could be detached; otherwise, <c>false</c>.</returns>
        public bool DetachKeyGroup(IKeyGroup keyGroup)
        {
            lock (_keyGroups)
            {
                if (keyGroup == null) return false;

                LinkedListNode<IKeyGroup> node = _keyGroups.Find(keyGroup);
                if (node == null) return false;

                _keyGroups.Remove(node);
                return true;
            }
        }

        private void InitializeKeys()
        {
            _CorsairLedPositions nativeLedPositions = (_CorsairLedPositions)Marshal.PtrToStructure(_CUESDK.CorsairGetLedPositions(), typeof(_CorsairLedPositions));
            int structSize = Marshal.SizeOf(typeof(_CorsairLedPosition));
            IntPtr ptr = nativeLedPositions.pLedPosition;
            for (int i = 0; i < nativeLedPositions.numberOfLed; i++)
            {
                _CorsairLedPosition ledPosition = (_CorsairLedPosition)Marshal.PtrToStructure(ptr, typeof(_CorsairLedPosition));
                CorsairLed led = InitializeLed((int)ledPosition.ledId, new RectangleF((float)ledPosition.left, (float)ledPosition.top, (float)ledPosition.width, (float)ledPosition.height));
                _keys.Add(ledPosition.ledId, new CorsairKey(ledPosition.ledId, led));

                ptr = new IntPtr(ptr.ToInt64() + structSize);
            }
        }

        #region Effects

        /// <summary>
        /// NOT IMPLEMENTED: Effects can't be applied directly to the keyboard. Add it to the Brush or create a keygroup instead.
        /// </summary>
        public void UpdateEffects()
        {
            throw new NotSupportedException("Effects can't be applied directly to the keyboard. Add it to the Brush or create a keygroup instead.");
        }

        /// <summary>
        /// NOT IMPLEMENTED: Effects can't be applied directly to the keyboard. Add it to the Brush or create a keygroup instead.
        /// </summary>
        /// <param name="effect">The effect to add.</param>
        public void AddEffect(IEffect<IKeyGroup> effect)
        {
            throw new NotSupportedException("Effects can't be applied directly to the keyboard. Add it to the Brush or create a keygroup instead.");
        }

        /// <summary>
        /// NOT IMPLEMENTED: Effects can't be applied directly to the keyboard. Add it to the Brush or create a keygroup instead.
        /// </summary>
        /// <param name="effect">The effect to remove.</param>
        public void RemoveEffect(IEffect<IKeyGroup> effect)
        {
            throw new NotSupportedException("Effects can't be applied directly to the keyboard. Add it to the Brush or create a keygroup instead.");
        }

        #endregion

        #region IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates over all keys of the keyboard.
        /// </summary>
        /// <returns>An enumerator for all keys of the keyboard.</returns>
        public IEnumerator<CorsairKey> GetEnumerator()
        {
            return _keys.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates over all keys of the keyboard.
        /// </summary>
        /// <returns>An enumerator for all keys of the keyboard.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #endregion
    }
}
