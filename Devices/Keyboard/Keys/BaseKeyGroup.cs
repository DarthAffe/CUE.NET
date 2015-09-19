using System.Collections.Generic;
using System.Drawing;

namespace CUE.NET.Devices.Keyboard.Keys
{
    public class BaseKeyGroup : IKeyGroup
    {
        #region Properties & Fields

        protected CorsairKeyboard Keyboard { get; }

        public IList<CorsairKey> Keys { get; } = new List<CorsairKey>();

        #endregion

        #region Constructors

        protected BaseKeyGroup(CorsairKeyboard keyboard)
        {
            this.Keyboard = keyboard;
        }

        #endregion

        #region Methods

        public virtual void SetColor(Color color)
        {
            foreach (CorsairKey key in Keys)
                key.Led.Color = color;
        }

        public void MergeKeys(IKeyGroup groupToMerge)
        {
            foreach (CorsairKey key in groupToMerge.Keys)
                if (!Keys.Contains(key))
                    Keys.Add(key);
        }

        #endregion
    }
}
