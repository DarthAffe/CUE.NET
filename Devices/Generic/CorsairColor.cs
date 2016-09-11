using System.Drawing;

namespace CUE.NET.Devices.Generic
{
    public class CorsairColor
    {
        #region Constants

        public static CorsairColor Transparent => Color.Transparent;

        #endregion

        #region Properties & Fields

        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        #endregion

        #region Constructors

        public CorsairColor()
        { }

        public CorsairColor(byte r, byte g, byte b) : this(255, r, g, b)
        { }

        public CorsairColor(byte a, byte r, byte g, byte b)
        {
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
        }

        #endregion

        #region Operators

        public static implicit operator CorsairColor(Color color)
        {
            return new CorsairColor(color.A, color.R, color.G, color.B);
        }

        public static implicit operator Color(CorsairColor color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        #endregion
    }
}
