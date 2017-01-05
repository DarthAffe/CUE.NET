// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

using System.Diagnostics;
using System.Drawing;

namespace CUE.NET.Devices.Generic
{
    /// <summary>
    /// Represents an ARGB (alpha, red, green, blue) color used by CUE.NET.
    /// </summary>
    [DebuggerDisplay("[A: {A}, R: {R}, G: {G}, B: {B}]")]
    public class CorsairColor
    {
        #region Constants

        /// <summary>
        /// Gets an transparent color [A: 0, R: 0, G: 0, B: 0]
        /// </summary>
        public static CorsairColor Transparent => Color.Transparent;

        #endregion

        #region Properties & Fields

        /// <summary>
        /// Gets or sets the alpha component value of this <see cref="CorsairColor"/>.
        /// </summary>
        public byte A { get; set; }

        /// <summary>
        /// Gets or sets the red component value of this <see cref="CorsairColor"/>.
        /// </summary>
        public byte R { get; set; }

        /// <summary>
        /// Gets or sets the green component value of this <see cref="CorsairColor"/>.
        /// </summary>
        public byte G { get; set; }

        /// <summary>
        /// Gets or sets the blue component value of this <see cref="CorsairColor"/>.
        /// </summary>
        public byte B { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairColor"/> class.
        /// The class created by this constructor equals <see cref="Transparent"/>.
        /// </summary>
        public CorsairColor()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairColor"/> class using only RGB-Values. 
        /// Alpha defaults to 255.
        /// </summary>
        /// <param name="r">The red component value of this <see cref="CorsairColor"/>.</param>
        /// <param name="g">The green component value of this <see cref="CorsairColor"/>.</param>
        /// <param name="b">The blue component value of this <see cref="CorsairColor"/>.</param>
        public CorsairColor(byte r, byte g, byte b)
            : this(255, r, g, b)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairColor"/> class using ARGB-values.
        /// </summary>
        /// <param name="a">The alpha component value of this <see cref="CorsairColor"/>.</param>
        /// <param name="r">The red component value of this <see cref="CorsairColor"/>.</param>
        /// <param name="g">The green component value of this <see cref="CorsairColor"/>.</param>
        /// <param name="b">The blue component value of this <see cref="CorsairColor"/>.</param>
        public CorsairColor(byte a, byte r, byte g, byte b)
        {
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairColor"/> class by cloning a existing <see cref="CorsairColor"/>.
        /// </summary>
        /// <param name="color">The <see cref="CorsairColor"/> the values are copied from.</param>
        public CorsairColor(CorsairColor color)
            : this(color.A, color.R, color.G, color.B)
        { }

        #endregion

        #region Operators

        /// <summary>
        /// Converts the individual byte-values of this <see cref="CorsairColor"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that contains the individual byte-values of this <see cref="CorsairColor"/>. For example "[A: 255, R: 255, G: 0, B: 0]".</returns>
        public override string ToString()
        {
            return $"[A: {A}, R: {R}, G: {G}, B: {B}]";
        }
        /// <summary>
        /// Tests whether the specified object is a <see cref="CorsairColor" /> and is equivalent to this <see cref="CorsairColor" />.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>true if <paramref name="obj" /> is a <see cref="CorsairColor" /> equivalent to this <see cref="CorsairColor" /> ; otherwise, false.</returns>
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

        /// <summary>
        /// Returns a hash code for this <see cref="CorsairColor" />.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this <see cref="CorsairColor" />.</returns>
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

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="CorsairColor" /> are equal.
        /// </summary>
        /// <param name="color1">The first <see cref="CorsairColor" /> color to compare.</param>
        /// <param name="color2">The second <see cref="CorsairColor" /> color to compare.</param>
        /// <returns>true if <paramref name="color1" /> and <paramref name="color2" /> are equal; otherwise, false.</returns>
        public static bool operator ==(CorsairColor color1, CorsairColor color2)
        {
            return ReferenceEquals(color1, null) ? ReferenceEquals(color2, null) : color1.Equals(color2);
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="CorsairColor" /> are equal.
        /// </summary>
        /// <param name="color1">The first <see cref="CorsairColor" /> color to compare.</param>
        /// <param name="color2">The second <see cref="CorsairColor" /> color to compare.</param>
        /// <returns>true if <paramref name="color1" /> and <paramref name="color2" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(CorsairColor color1, CorsairColor color2)
        {
            return !(color1 == color2);
        }

        /// <summary>
        /// Converts a <see cref="Color" /> to a <see cref="CorsairColor" />.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to convert.</param>
        public static implicit operator CorsairColor(Color color)
        {
            return new CorsairColor(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Converts a <see cref="CorsairColor" /> to a <see cref="Color" />.
        /// </summary>
        /// <param name="color">The <see cref="CorsairColor"/> to convert.</param>
        public static implicit operator Color(CorsairColor color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        #endregion
    }
}
