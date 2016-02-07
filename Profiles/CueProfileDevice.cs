using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CUE.NET.Brushes;

namespace CUE.NET.Profiles
{
    internal class CueProfileDevice
    {
        #region Properties & Fields

        internal string Name { get; }

        internal IEnumerable<string> Modes => _modes.Keys.ToList();

        private Dictionary<string, CueProfileMode> _modes;

        #endregion

        #region Brush Conversion

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
