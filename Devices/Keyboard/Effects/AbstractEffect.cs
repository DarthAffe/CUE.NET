// ReSharper disable MemberCanBePrivate.Global

using System.Collections.Generic;
using System.Linq;
using CUE.NET.Devices.Keyboard.Brushes;
using CUE.NET.Devices.Keyboard.Keys;

namespace CUE.NET.Devices.Keyboard.Effects
{
    public abstract class AbstractEffect : IEffect
    {
        #region Properties & Fields

        public IEnumerable<CorsairKey> KeyList { get; protected set; }

        public abstract IBrush EffectBrush { get; }

        public int ZIndex { get; set; } = 0;

        public bool IsDone { get; protected set; }

        #endregion

        #region Methods

        public void SetTarget(IKeyGroup keyGroup)
        {
            KeyList = keyGroup.Keys.ToList();
        }

        public abstract void Update(float deltaTime);

        public virtual void OnAttach()
        { }

        public virtual void OnDetach()
        { }

        #endregion
    }
}
