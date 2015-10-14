using System.Collections.Generic;
using CUE.NET.Devices.Keyboard.Brushes;
using CUE.NET.Devices.Keyboard.Keys;

namespace CUE.NET.Devices.Keyboard.Effects
{
    public interface IEffect
    {
        #region Properties & Fields

        IEnumerable<CorsairKey> KeyList { get; }

        IBrush EffectBrush { get; }

        int ZIndex { get; set; }

        bool IsDone { get; }

        #endregion

        #region Methods
        
        void Update(float deltaTime);

        void OnAttach();

        void OnDetach();

        #endregion
    }
}
