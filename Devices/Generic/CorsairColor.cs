// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

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

        public CorsairColor(CorsairColor color)
            : this(color.A, color.R, color.G, color.B)
        { }

        #endregion

        #region Operators

        public override string ToString()
        {
            return $"[A: {A}, R: {R}, G: {G}, B: {B}]";
        }

        public override bool Equals(object obj)
        {
            CorsairColor compareColor = obj as CorsairColor;
            if (ReferenceEquals(compareColor, null))
                return false;

            if (ReferenceEquals(this, compareColor))
                return true;

            if (GetType() != compareColor.GetType())
                return false;

            return (compareColor.A == A) && (compareColor.R == R) && (compareColor.G == G) && (compareColor.B == B);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = A.GetHashCode();
                hashCode = (hashCode * 397) ^ R.GetHashCode();
                hashCode = (hashCode * 397) ^ G.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(CorsairColor color1, CorsairColor color2)
        {
            return ReferenceEquals(color1, null) ? ReferenceEquals(color2, null) : color1.Equals(color2);
        }
        public static bool operator !=(CorsairColor color1, CorsairColor color2)
        {
            return !(color1 == color2);
        }

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
