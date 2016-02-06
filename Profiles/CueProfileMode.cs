using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using CUE.NET.Brushes;
using CUE.NET.Devices.Keyboard.Enums;

namespace CUE.NET.Profiles
{
    internal class CueProfileMode
    {
        #region Properties & Fields

        internal string Name { get; }

        private Dictionary<CorsairKeyboardKeyId, Color> _keyLights;

        #endregion

        #region Brush Conversion

        public static implicit operator ProfileBrush(CueProfileMode profile)
        {
            return profile != null ? new ProfileBrush(profile._keyLights) : null;
        }

        #endregion

        #region Constructors

        private CueProfileMode(string name)
        {
            this.Name = name;
        }

        #endregion

        #region Methods

        internal static CueProfileMode Load(XElement modeRoot)
        {
            // ReSharper disable PossibleNullReferenceException - Just let it fail - no need to check anything here ...
            try
            {
                if (modeRoot == null) return null;

                return new CueProfileMode(modeRoot.Element("name").Value)
                {
                    _keyLights = modeRoot.Element("lightBackgrounds").Element("keyBgLights").Elements("lightBackground")
                        .Select(x =>
                        {
                            string name = x.Attribute("key").Value;
                            if (name.Length == 1 && char.IsDigit(name[0])) // Our enum names can't be digit only so we need to map them
                                name = 'D' + name;

                            return new
                            {
                                key = (CorsairKeyboardKeyId)Enum.Parse(typeof(CorsairKeyboardKeyId), name),
                                color = ColorTranslator.FromHtml(x.Attribute("color").Value)
                            };
                        })
                        .ToDictionary(x => x.key, x => x.color)
                };
            }
            catch (Exception ex) // I have no idea how the factory pattern should handle such a case - time to read :p 
            {
                return null;
            }
            // ReSharper restore PossibleNullReferenceException
        }

        #endregion
    }
}
