using System.Collections.Generic;
using System.Linq;
using CUE.NET.Devices.Keyboard.Brushes;
using CUE.NET.Devices.Keyboard.Keys;

namespace CUE.NET.Devices.Keyboard.Effects
{
    public abstract class AbstractEffect : IEffect
    {
        #region Properties & Fields

        public abstract IBrush EffectBrush { get; }

        public IEnumerable<CorsairKey> KeyList { get; protected set; }

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
