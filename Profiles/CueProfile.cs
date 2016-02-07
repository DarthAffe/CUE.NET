// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CUE.NET.Brushes;

namespace CUE.NET.Profiles
{
    /// <summary>
    /// Represents a CUE profile.
    /// </summary>
    public class CueProfile
    {
        #region Properties & Fields

        private Dictionary<string, CueProfileDevice> _devices;

        /// <summary>
        /// Gets the Id of the profile.
        /// </summary>
        public string Id { get; }
        
        /// <summary>
        /// Gets the Name of the profile.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Returns a list of strings containing the name of all modes available.
        /// </summary>
        public IEnumerable<string> Modes
        {
            get
            {
                string device = _devices.Keys.FirstOrDefault();
                CueProfileDevice cpd;
                return (device != null && _devices.TryGetValue(device, out cpd)) ? cpd.Modes : new string[0];
            }
        }

        /// <summary>
        /// Returns the <see cref="ProfileBrush"/> for the given mode.
        /// </summary>
        /// <param name="mode">The mode to select.</param>
        /// <returns>The <see cref="ProfileBrush"/> of the given mode.</returns>
        public ProfileBrush this[string mode]
        {
            get
            {
                string device = CueSDK.KeyboardSDK?.KeyboardDeviceInfo?.Model;
                CueProfileDevice cpd;
                return (device != null && _devices.TryGetValue(device, out cpd)) ? cpd[mode] : null;
            }
        }

        #endregion

        #region Constructors

        private CueProfile(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads a CUE profile from the given file.
        /// </summary>
        /// <param name="file">The profile-file.</param>
        /// <returns>The loaded <see cref="CueProfile" /> or null.</returns>
        internal static CueProfile Load(string file)
        {
            // ReSharper disable PossibleNullReferenceException - Just let it fail - no need to check anything here ...
            try
            {
                if (!File.Exists(file)) return null;

                XElement profileRoot = XDocument.Load(file).Root;
                return new CueProfile(profileRoot.Element("id").Value, profileRoot.Element("name").Value)
                {
                    _devices = profileRoot.Element("devices").Elements("device")
                                .Select(CueProfileDevice.Load)
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
