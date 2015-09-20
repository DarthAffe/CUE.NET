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

        public Color Color { get; set; } = Color.Transparent;

        #endregion

        #region Constructors

        protected BaseKeyGroup(CorsairKeyboard keyboard, bool autoAttach = true)
        {
            this.Keyboard = keyboard;

            if (autoAttach)
                keyboard.AttachKeyGroup(this);
        }

        #endregion

        #region Methods

        public void MergeKeys(IKeyGroup groupToMerge)
        {
            foreach (CorsairKey key in groupToMerge.Keys)
                if (!GroupKeys.Contains(key))
                    GroupKeys.Add(key);
        }

        #endregion
    }
}
