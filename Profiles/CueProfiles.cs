// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CUE.NET.Profiles
{
    /// <summary>
    /// Represents the SDK for CUE profiles.
    /// </summary>
    [Obsolete("Only works with CUE 1.")]
    public static class CueProfiles
    {
        #region Constants

        private const string PROFILE_EXTENSION = ".prf";
        private static readonly string PROFILE_FOLDER = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Corsair", "HID", "Profiles");
        private static readonly string CONFIG_FILE = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Corsair", "HID", "config.cfg");

        #endregion

        #region Properties & Fields

        private static Dictionary<string, string> _profileNameMapping = new Dictionary<string, string>();

        /// <summary>
        /// Gets a list containing the names of all existing profiles.
        /// </summary>
        public static List<string> ProfileNames
        {
            get
            {
                LoadProfileNames();
                return _profileNameMapping.Keys.ToList();
            }
        }

        /// <summary>
        /// Gets a list containing the ids of all existing profiles.
        /// </summary>
        public static List<string> ProfileIds
        {
            get
            {
                LoadProfileNames();
                return _profileNameMapping.Values.ToList();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the profile with the given name.
        /// </summary>
        /// <param name="name">The name (the one given in CUE, not the filename) of the profile.</param>
        /// <returns>The loaded <see cref="CueProfile" /> or null if it couldn't be loaded.</returns>
        public static CueProfile LoadProfileByName(string name = null)
        {
            string id = null;
            if (name != null && !_profileNameMapping.TryGetValue(name, out id))
            {
                LoadProfileNames(); // Reload and try again
                if (!_profileNameMapping.TryGetValue(name, out id))
                    return null;
            }

            return LoadProfileByID(id);
        }

        /// <summary>
        /// Loads the profile with the given id.
        /// </summary>
        /// <param name="id">The id of the profile.</param>
        /// <returns>The loaded <see cref="CueProfile" /> or null if it couldn't be loaded.</returns>
        public static CueProfile LoadProfileByID(string id = null)
        {
            if (id == null) id = GetDefaultProfileId();
            return CueProfile.Load(Path.Combine(PROFILE_FOLDER, id + PROFILE_EXTENSION));
        }

        private static string GetDefaultProfileId()
        {
            try
            {
                return XDocument.Load(CONFIG_FILE).Root?.Elements("value").FirstOrDefault(x => string.Equals(x.Attribute("name")?.Value, "InitialProfile", StringComparison.OrdinalIgnoreCase))?.Value;
            }
            // ReSharper disable once CatchAllClause - This shouldn't happen but you never know ...
            catch
            {
                return null;
            }
        }

        private static void LoadProfileNames()
        {
            try
            {
                IEnumerable<string> profileFiles = Directory.GetFiles(PROFILE_FOLDER).Where(x => x.EndsWith(PROFILE_EXTENSION));
                foreach (string profileFile in profileFiles)
                {
                    XElement profileNode = XDocument.Load(profileFile).Root;
                    if (profileNode == null) continue;

                    string name = profileNode.Element("name")?.Value;
                    string id = profileNode.Element("id")?.Value;

                    if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(id) && !_profileNameMapping.ContainsKey(name)) // I think duplicates are an error case
                        _profileNameMapping.Add(name, id);
                }
            }
            // ReSharper disable once CatchAllClause - This shouldn't happen but you never know ... 
            catch
            {
                _profileNameMapping.Clear();
            }
        }

        #endregion
    }
}
