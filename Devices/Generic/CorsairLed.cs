using System.Drawing;

namespace CUE.NET.Devices.Generic
{
    public class CorsairLed
    {
        #region Properties & Fields

        public bool IsDirty { get; private set; } = false;

        private Color _color = Color.Black;
        public Color Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                    IsDirty = true;

                _color = value;
            }
        }

        //TODO DarthAffe 19.09.2015: Add effects and stuff

        #endregion

        #region Constructors

        internal CorsairLed() { }

        #endregion
    }
}
