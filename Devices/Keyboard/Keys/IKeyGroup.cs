using System.Collections.Generic;
using System.Drawing;

namespace CUE.NET.Devices.Keyboard.Keys
{
    public interface IKeyGroup
    {
        //TODO DarthAffe 19.09.2015: This might be not needed/bad
        IList<CorsairKey> Keys { get; }

        void SetColor(Color color);

        void MergeKeys(IKeyGroup groupToMerge);
    }
}
