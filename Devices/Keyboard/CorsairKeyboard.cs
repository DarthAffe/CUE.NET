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
using CUE.NET.Groups;
using CUE.NET.Helper;
using CUE.NET.Native;

namespace CUE.NET.Devices.Keyboard
{
    /// <summary>
    /// Represents the SDK for a corsair keyboard.
    /// </summary>
    public class CorsairKeyboard : AbstractCueDevice, IEnumerable<CorsairKey>
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
        public new CorsairKey this[PointF location] => _keys.Values.FirstOrDefault(x => x.Led.LedRectangle.Contains(location));

        /// <summary>
        /// Gets a list of <see cref="CorsairKey" /> inside the given rectangle.
        /// </summary>
        /// <param name="referenceRect">The rectangle to check.</param>
        /// <param name="minOverlayPercentage">The minimal percentage overlay a key must have with the <see cref="Rectangle" /> to be taken into the list.</param>
        /// <returns></returns>
        public new IEnumerable<CorsairKey> this[RectangleF referenceRect, float minOverlayPercentage = 0.5f] => _keys.Values
            .Where(x => RectangleHelper.CalculateIntersectPercentage(x.Led.LedRectangle, referenceRect) >= minOverlayPercentage);

        #endregion

        private Dictionary<CorsairKeyboardKeyId, CorsairKey> _keys = new Dictionary<CorsairKeyboardKeyId, CorsairKey>();

        /// <summary>
        /// Gets a read-only collection containing the keys of the keyboard.
        /// </summary>
        public IEnumerable<CorsairKey> Keys => new ReadOnlyCollection<CorsairKey>(_keys.Values.ToList());

        /// <summary>
        /// Gets specific information provided by CUE for the keyboard.
        /// </summary>
        public CorsairKeyboardDeviceInfo KeyboardDeviceInfo { get; }

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
        }

        #endregion

        #region Methods

        protected override void InitializeLeds()
        {
            _CorsairLedPositions nativeLedPositions = (_CorsairLedPositions)Marshal.PtrToStructure(_CUESDK.CorsairGetLedPositions(), typeof(_CorsairLedPositions));
            int structSize = Marshal.SizeOf(typeof(_CorsairLedPosition));
            IntPtr ptr = nativeLedPositions.pLedPosition;
            for (int i = 0; i < nativeLedPositions.numberOfLed; i++)
            {
                _CorsairLedPosition ledPosition = (_CorsairLedPosition)Marshal.PtrToStructure(ptr, typeof(_CorsairLedPosition));
                CorsairLed led = InitializeLed(ledPosition.ledId, new RectangleF((float)ledPosition.left, (float)ledPosition.top, (float)ledPosition.width, (float)ledPosition.height));
                _keys.Add((CorsairKeyboardKeyId)ledPosition.ledId, new CorsairKey((CorsairKeyboardKeyId)ledPosition.ledId, led));

                ptr = new IntPtr(ptr.ToInt64() + structSize);
            }
        }

        #region IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates over all keys of the keyboard.
        /// </summary>
        /// <returns>An enumerator for all keys of the keyboard.</returns>
        public new IEnumerator<CorsairKey> GetEnumerator()
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
