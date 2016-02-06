using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CUE.NET.Brushes;

namespace CUE.NET.Profiles
{
    public class CueProfile
    {
        #region Properties & Fields

        public string Id { get; }
        public string Name { get; }

        private Dictionary<string, CueProfileDevice> _devices;

        public IEnumerable<string> Modes
        {
            get
            {
                string device = _devices.Keys.FirstOrDefault();

                CueProfileDevice cpd;
                return (device != null && _devices.TryGetValue(device, out cpd)) ? cpd.Modes : new string[0];
            }
        }

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
            catch // I have no idea how the factory pattern should handle such a case - time to read :p 
            {
                return null;
            }
            // ReSharper restore PossibleNullReferenceException
        }

        #endregion
    }
}
