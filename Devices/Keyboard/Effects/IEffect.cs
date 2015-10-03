using System.Collections.Generic;
using CUE.NET.Devices.Keyboard.Brushes;
using CUE.NET.Devices.Keyboard.Keys;

namespace CUE.NET.Devices.Keyboard.Effects
{
    public interface IEffect
    {
        #region Properties & Fields

        IBrush EffectBrush { get; }

        IEnumerable<CorsairKey> KeyList { get; }

        bool IsDone { get; }

        #endregion

        #region Methods
        
        void Update(float deltaTime);

        void OnAttach();

        void OnDetach();

        #endregion
    }
}
