using System.Collections.Generic;
using System.Collections.ObjectModel;
using CUE.NET.Devices.Keyboard.Brushes;
using CUE.NET.Devices.Keyboard.Extensions;

namespace CUE.NET.Devices.Keyboard.Keys
{
    /// <summary>
    /// Represents a basic keygroup.
    /// </summary>
    public abstract class BaseKeyGroup : IKeyGroup
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the keyboard this keygroup belongs to.
        /// </summary>
        internal CorsairKeyboard Keyboard { get; }

        /// <summary>
        /// Gets a read-only collection containing the keys from this group.
        /// </summary>
        public IEnumerable<CorsairKey> Keys => new ReadOnlyCollection<CorsairKey>(GetGroupKeys());

        /// <summary>
        /// Gets or sets the brush which should be drawn over this group.
        /// </summary>
        public IBrush Brush { get; set; }

        /// <summary>
        /// Gets or sets the z-index of this keygroup to allow ordering them before drawing. (lowest first) (default: 0)
        /// </summary>
        public int ZIndex { get; set; } = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseKeyGroup"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard this keygroup belongs to.</param>
        /// <param name="autoAttach">Specifies whether this group should be automatically attached or not.</param>
        protected BaseKeyGroup(CorsairKeyboard keyboard, bool autoAttach = true)
        {
            this.Keyboard = keyboard;

            if (autoAttach)
                this.Attach();
        }

        /// <summary>
        /// Gets a list containing the keys from this group.
        /// </summary>
        /// <returns>The list containing the keys.</returns>
        protected abstract IList<CorsairKey> GetGroupKeys();

        #endregion
    }
}
