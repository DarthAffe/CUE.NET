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

        public void SetColor(Color color)
        {
            foreach (CorsairKey key in this)
                key.Led.Color = color;
        }

        private void InitializeKeys()
        {
            _CorsairLedPositions nativeLedPositions = (_CorsairLedPositions)Marshal.PtrToStructure(_CUESDK.CorsairGetLedPositions(), typeof(_CorsairLedPositions));
            int structSize = Marshal.SizeOf(typeof(_CorsairLedPosition));
            IntPtr ptr = nativeLedPositions.pLedPosition;
            for (int i = 0; i < nativeLedPositions.numberOfLed; i++)
            {
                _CorsairLedPosition ledPosition = Marshal.PtrToStructure<_CorsairLedPosition>(ptr);
                _keys.Add(ledPosition.ledId, new CorsairKey(ledPosition.ledId, GetLed((int)ledPosition.ledId),
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
