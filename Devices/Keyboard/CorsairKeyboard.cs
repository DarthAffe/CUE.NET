using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Keyboard.Brushes;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Devices.Keyboard.Keys;
using CUE.NET.Helper;
using CUE.NET.Native;

namespace CUE.NET.Devices.Keyboard
{
    public class CorsairKeyboard : AbstractCueDevice, IEnumerable<CorsairKey>, IKeyGroup
    {
        #region Properties & Fields

        public CorsairKeyboardDeviceInfo KeyboardDeviceInfo { get; }

        public RectangleF KeyboardRectangle { get; private set; }

        private Dictionary<CorsairKeyboardKeyId, CorsairKey> _keys = new Dictionary<CorsairKeyboardKeyId, CorsairKey>();
        public CorsairKey this[CorsairKeyboardKeyId keyId]
        {
            get
            {
                CorsairKey key;
                return _keys.TryGetValue(keyId, out key) ? key : null;
            }
            private set { throw new NotSupportedException(); }
        }

        public CorsairKey this[char key]
        {
            get { return this[_CUESDK.CorsairGetLedIdForKeyName(key)]; }
            private set { throw new NotSupportedException(); }
        }

        public CorsairKey this[PointF location]
        {
            get { return _keys.Values.FirstOrDefault(x => x.KeyRectangle.Contains(location)); }
            private set { throw new NotSupportedException(); }
        }

        public IEnumerable<CorsairKey> this[RectangleF referenceRect, float minOverlayPercentage = 0.5f]
        {
            get { return _keys.Values.Where(x => RectangleHelper.CalculateIntersectPercentage(x.KeyRectangle, referenceRect) >= minOverlayPercentage); }
            private set { throw new NotSupportedException(); }
        }

        public IEnumerable<CorsairKey> Keys => new ReadOnlyCollection<CorsairKey>(_keys.Values.ToList());

        public IBrush Brush { get; set; }

        private readonly IList<IKeyGroup> _keyGroups = new List<IKeyGroup>();

        #endregion

        #region Constructors

        internal CorsairKeyboard(CorsairKeyboardDeviceInfo info)
            : base(info)
        {
            this.KeyboardDeviceInfo = info;

            InitializeKeys();
            KeyboardRectangle = RectangleHelper.CreateRectangleFromRectangles(this.Select(x => x.KeyRectangle));
        }

        #endregion

        #region Methods

        public override void UpdateLeds(bool forceUpdate = false)
        {
            // Apply all KeyGroups

            if (Brush != null)
                ApplyBrush(this.ToList(), Brush);

            //TODO DarthAffe 20.09.2015: Add some sort of priority
            foreach (IKeyGroup keyGroup in _keyGroups)
                ApplyBrush(keyGroup.Keys.ToList(), keyGroup.Brush);

            // Perform 'real' update
            base.UpdateLeds(forceUpdate);
        }

        private void ApplyBrush(ICollection<CorsairKey> keys, IBrush brush)
        {
            RectangleF brushRectangle = RectangleHelper.CreateRectangleFromRectangles(keys.Select(x => x.KeyRectangle));
            foreach (CorsairKey key in keys)
                key.Led.Color = brush.GetColorAtPoint(brushRectangle, key.KeyRectangle.GetCenter());
        }

        public bool AttachKeyGroup(IKeyGroup keyGroup)
        {
            if (keyGroup == null || _keyGroups.Contains(keyGroup)) return false;

            _keyGroups.Add(keyGroup);
            return true;
        }

        public bool DetachKeyGroup(IKeyGroup keyGroup)
        {
            if (keyGroup == null || !_keyGroups.Contains(keyGroup)) return false;

            _keyGroups.Remove(keyGroup);
            return true;
        }

        private void InitializeKeys()
        {
            _CorsairLedPositions nativeLedPositions = (_CorsairLedPositions)Marshal.PtrToStructure(_CUESDK.CorsairGetLedPositions(), typeof(_CorsairLedPositions));
            int structSize = Marshal.SizeOf(typeof(_CorsairLedPosition));
            IntPtr ptr = nativeLedPositions.pLedPosition;
            for (int i = 0; i < nativeLedPositions.numberOfLed; i++)
            {
                _CorsairLedPosition ledPosition = Marshal.PtrToStructure<_CorsairLedPosition>(ptr);
                CorsairLed led = GetLed((int)ledPosition.ledId);
                _keys.Add(ledPosition.ledId, new CorsairKey(ledPosition.ledId, led,
                    new RectangleF((float)ledPosition.left, (float)ledPosition.top, (float)ledPosition.width, (float)ledPosition.height)));

                ptr = new IntPtr(ptr.ToInt64() + structSize);
            }
        }

        #region IEnumerable

        public IEnumerator<CorsairKey> GetEnumerator()
        {
            return _keys.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #endregion
    }
}
