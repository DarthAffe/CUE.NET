using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using CUE.NET.Devices.Generic;
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
        
        public Color Color { get; set; } = Color.Transparent;

        private readonly IList<IKeyGroup> _keyGroups = new List<IKeyGroup>();

        #endregion

        #region Constructors

        internal CorsairKeyboard(CorsairKeyboardDeviceInfo info)
            : base(info)
        {
            this.KeyboardDeviceInfo = info;

            InitializeKeys();
            CalculateKeyboardRectangle();
        }

        #endregion

        #region Methods

        public override void UpdateLeds(bool forceUpdate = false)
        {
            // Apply all KeyGroups first
            // Update only 'clean' leds, manual set should always override groups
            IEnumerable<CorsairKey> cleanKeys = this.Where(x => !x.Led.IsUpdated).ToList();

            if (Color != Color.Transparent)
                foreach (CorsairKey key in cleanKeys)
                    key.Led.Color = Color;

            //TODO DarthAffe 20.09.2015: Add some sort of priority
            foreach (IKeyGroup keyGroup in _keyGroups)
                foreach (CorsairKey key in keyGroup.Keys.Where(key => cleanKeys.Contains(key)))
                    key.Led.Color = keyGroup.Color;

            // Perform 'real' update
            base.UpdateLeds(forceUpdate);
        }

        public void AttachKeyGroup(IKeyGroup keyGroup)
        {
            if (keyGroup == null) return;

            if (!_keyGroups.Contains(keyGroup))
                _keyGroups.Add(keyGroup);
        }

        public void DetachKeyGroup(IKeyGroup keyGroup)
        {
            if (keyGroup == null) return;

            _keyGroups.Remove(keyGroup);
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
                //TODO DarthAffe 19.09.2015: Is something like RectangleD needed? I don't think so ...
                    new RectangleF((float)ledPosition.left, (float)ledPosition.top, (float)ledPosition.width, (float)ledPosition.height)));

                ptr = new IntPtr(ptr.ToInt64() + structSize);
            }
        }

        private void CalculateKeyboardRectangle()
        {
            float posX = float.MaxValue;
            float posY = float.MaxValue;
            float posX2 = float.MinValue;
            float posY2 = float.MinValue;

            foreach (CorsairKey key in this)
            {
                posX = Math.Min(posX, key.KeyRectangle.X);
                posY = Math.Min(posY, key.KeyRectangle.Y);
                posX2 = Math.Max(posX2, key.KeyRectangle.X + key.KeyRectangle.Width);
                posY2 = Math.Max(posY2, key.KeyRectangle.Y + key.KeyRectangle.Height);
            }

            KeyboardRectangle = RectangleHelper.CreateRectangleFromPoints(new PointF(posX, posY), new PointF(posX2, posY2));
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
