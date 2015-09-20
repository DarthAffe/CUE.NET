using System.Linq;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Devices.Keyboard.Keys;

namespace CUE.NET.Devices.Keyboard.Extensions
{
    public static class KeyGroupExtension
    {
        public static SimpleKeyGroup ToSimpleKeyGroup(this BaseKeyGroup keyGroup)
        {
            SimpleKeyGroup simpleKeyGroup = keyGroup as SimpleKeyGroup;
            if (simpleKeyGroup == null)
            {
                bool wasAttached = keyGroup.Detach();
                simpleKeyGroup = new SimpleKeyGroup(keyGroup.Keyboard, wasAttached, keyGroup.Keys.ToArray()) { Color = keyGroup.Color };
            }
            return simpleKeyGroup;
        }

        public static SimpleKeyGroup Exclude(this BaseKeyGroup keyGroup, params CorsairKeyboardKeyId[] keyIds)
        {
            SimpleKeyGroup simpleKeyGroup = keyGroup.ToSimpleKeyGroup();
            foreach (CorsairKeyboardKeyId keyId in keyIds)
                simpleKeyGroup.RemoveKey(keyId);
            return simpleKeyGroup;
        }

        public static SimpleKeyGroup Exclude(this BaseKeyGroup keyGroup, params CorsairKey[] keyIds)
        {
            SimpleKeyGroup simpleKeyGroup = keyGroup.ToSimpleKeyGroup();
            foreach (CorsairKey key in keyIds)
                simpleKeyGroup.RemoveKey(key);
            return simpleKeyGroup;
        }

        public static bool Attach(this BaseKeyGroup keyGroup)
        {
            return keyGroup.Keyboard?.AttachKeyGroup(keyGroup) ?? false;
        }

        public static bool Detach(this BaseKeyGroup keyGroup)
        {
            return keyGroup.Keyboard?.DetachKeyGroup(keyGroup) ?? false;
        }
    }
}
