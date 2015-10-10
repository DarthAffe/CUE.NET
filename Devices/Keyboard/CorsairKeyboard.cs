// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Keyboard.Brushes;
using CUE.NET.Devices.Keyboard.Effects;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Devices.Keyboard.Keys;
using CUE.NET.Helper;
using CUE.NET.Native;

namespace CUE.NET.Devices.Keyboard
{
    public class CorsairKeyboard : AbstractCueDevice, IEnumerable<CorsairKey>, IKeyGroup
    {
        #region Properties & Fields

        #region Indexer

        public CorsairKey this[CorsairKeyboardKeyId keyId]
        {
            get
            {
                CorsairKey key;
                return _keys.TryGetValue(keyId, out key) ? key : null;
            }
        }

        public CorsairKey this[char key] => this[_CUESDK.CorsairGetLedIdForKeyName(key)];

        public CorsairKey this[PointF location] => _keys.Values.FirstOrDefault(x => x.KeyRectangle.Contains(location));

        public IEnumerable<CorsairKey> this[RectangleF referenceRect, float minOverlayPercentage = 0.5f] => _keys.Values.Where(x => RectangleHelper.CalculateIntersectPercentage(x.KeyRectangle, referenceRect) >= minOverlayPercentage);

        #endregion

        private readonly LinkedList<IKeyGroup> _keyGroups = new LinkedList<IKeyGroup>();
        private readonly LinkedList<EffectTimeContainer> _effects = new LinkedList<EffectTimeContainer>();

        private Dictionary<CorsairKeyboardKeyId, CorsairKey> _keys = new Dictionary<CorsairKeyboardKeyId, CorsairKey>();
        public IEnumerable<CorsairKey> Keys => new ReadOnlyCollection<CorsairKey>(_keys.Values.ToList());

        public CorsairKeyboardDeviceInfo KeyboardDeviceInfo { get; }
        public RectangleF KeyboardRectangle { get; private set; }
        public IBrush Brush { get; set; }

        protected override bool HasEffect
        {
            get
            {
                lock (_effects)
                    return _effects.Any();
            }
        }

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

        #region Update

        public override void Update(bool flushLeds = false)
        {
            UpdateKeyGroups();
            UpdateEffects();

            // Perform 'real' update
            base.Update(flushLeds);
        }

        private void UpdateEffects()
        {
            List<IEffect> effectsToRemove = new List<IEffect>();
            lock (_effects)
            {
                long currentTicks = DateTime.Now.Ticks;
                foreach (EffectTimeContainer effect in _effects)
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

                        ApplyBrush((effect.Effect.KeyList ?? this).ToList(), effect.Effect.EffectBrush);

                        if (effect.Effect.IsDone)
                            effectsToRemove.Add(effect.Effect);
                    }
                    catch (Exception ex) { ManageException(ex); }
                }
            }

            foreach (IEffect effect in effectsToRemove)
                DetachEffect(effect);
        }

        private void UpdateKeyGroups()
        {
            if (Brush != null)
                ApplyBrush(this.ToList(), Brush);

            lock (_keyGroups)
            {
                //TODO DarthAffe 20.09.2015: Add some sort of priority
                foreach (IKeyGroup keyGroup in _keyGroups)
                    ApplyBrush(keyGroup.Keys.ToList(), keyGroup.Brush);
            }
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local - idc
        private void ApplyBrush(ICollection<CorsairKey> keys, IBrush brush)
        {
            try
            {
                RectangleF brushRectangle = RectangleHelper.CreateRectangleFromRectangles(keys.Select(x => x.KeyRectangle));
                foreach (CorsairKey key in keys)
                    key.Led.Color = brush.GetColorAtPoint(brushRectangle, key.KeyRectangle.GetCenter());
            }
            catch (Exception ex) { ManageException(ex); }
        }

        #endregion

        public bool AttachKeyGroup(IKeyGroup keyGroup)
        {
            lock (_keyGroups)
            {
                if (keyGroup == null || _keyGroups.Contains(keyGroup)) return false;

                _keyGroups.AddLast(keyGroup);
                return true;
            }
        }

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

        public bool AttachEffect(IEffect effect)
        {
            bool retVal = false;
            lock (_effects)
            {
                if (effect != null && _effects.All(x => x.Effect != effect))
                {
                    effect.OnAttach();
                    _effects.AddLast(new EffectTimeContainer(effect, -1));
                    retVal = true;
                }
            }

            CheckUpdateLoop();
            return retVal;
        }

        public bool DetachEffect(IEffect effect)
        {
            bool retVal = false;
            lock (_effects)
            {
                if (effect != null)
                {
                    EffectTimeContainer val = _effects.FirstOrDefault(x => x.Effect == effect);
                    if (val != null)
                    {
                        effect.OnDetach();
                        _effects.Remove(val);
                        retVal = true;
                    }
                }
            }
            CheckUpdateLoop();
            return retVal;
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
