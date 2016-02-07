using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CUE.NET.Brushes;

namespace CUE.NET.Profiles
{
    /// <summary>
    /// Represents a device of a CUE profile.
    /// </summary>
    internal class CueProfileDevice
    {
        #region Properties & Fields

        /// <summary>
        /// The name of the device.
        /// </summary>
        internal string Name { get; }

        /// <summary>
        /// Returns a list of strings containing the name of all modes available for this device.
        /// </summary>
        internal IEnumerable<string> Modes => _modes.Keys.ToList();

        private Dictionary<string, CueProfileMode> _modes;

        #endregion

        #region Brush Conversion

        /// <summary>
        /// Returns the <see cref="ProfileBrush"/> for the given mode.
        /// </summary>
        /// <param name="mode">The mode to select.</param>
        /// <returns>The <see cref="ProfileBrush"/> of the given mode.</returns>
        internal ProfileBrush this[string mode]
        {
            get
            {
                if (mode == null)
                    mode = _modes.Keys.FirstOrDefault();

                CueProfileMode cpm;
                return (mode != null && _modes.TryGetValue(mode, out cpm)) ? cpm : null;
            }
        }

        #endregion

        #region Constructors

        private CueProfileDevice(string name)
        {
            this.Name = name;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads a device of a CUE profile from the given XML-node.
        /// </summary>
        /// <param name="deviceRoot">The node containing the device.</param>
        /// <returns>The loaded <see cref="CueProfileDevice" /> or null.</returns>
        internal static CueProfileDevice Load(XElement deviceRoot)
        {
            // ReSharper disable PossibleNullReferenceException - Just let it fail - no need to check anything here ...
            try
            {
                if (deviceRoot == null) return null;

                return new CueProfileDevice(deviceRoot.Element("modelName").Value)
                {
                    _modes = deviceRoot.Element("modes").Elements("mode")
                        .Select(CueProfileMode.Load)
                        .Where(x => x != null)
                        .ToDictionary(x => x.Name)
                };
            }
            // ReSharper disable once CatchAllClause - I have no idea how the factory pattern should handle such a case - time to read :p 
            catch
            {
                return null;
            }
            // ReSharper restore PossibleNullReferenceException
        }

        #endregion
    }
}
