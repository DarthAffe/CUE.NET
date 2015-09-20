using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace CUE.NET.Devices.Keyboard.Keys
{
    public class BaseKeyGroup : IKeyGroup
    {
        #region Properties & Fields

        protected CorsairKeyboard Keyboard { get; }

        public IEnumerable<CorsairKey> Keys => new ReadOnlyCollection<CorsairKey>(GroupKeys);
        protected IList<CorsairKey> GroupKeys { get; } = new List<CorsairKey>();

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
            foreach (CorsairKey key in GroupKeys)
                key.Led.Color = color;
        }

        public void MergeKeys(IKeyGroup groupToMerge)
        {
            foreach (CorsairKey key in groupToMerge.Keys)
                if (!GroupKeys.Contains(key))
                    GroupKeys.Add(key);
        }

        #endregion
    }
}
