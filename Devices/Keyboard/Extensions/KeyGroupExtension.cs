using System.Linq;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Devices.Keyboard.Keys;

namespace CUE.NET.Devices.Keyboard.Extensions
{
    public static class KeyGroupExtension
    {
        public static ListKeyGroup ToSimpleKeyGroup(this BaseKeyGroup keyGroup)
        {
            ListKeyGroup simpleKeyGroup = keyGroup as ListKeyGroup;
            if (simpleKeyGroup == null)
            {
                bool wasAttached = keyGroup.Detach();
                simpleKeyGroup = new ListKeyGroup(keyGroup.Keyboard, wasAttached, keyGroup.Keys.ToArray()) { Brush = keyGroup.Brush };
            }
            return simpleKeyGroup;
        }

        public static ListKeyGroup Exclude(this BaseKeyGroup keyGroup, params CorsairKeyboardKeyId[] keyIds)
        {
            ListKeyGroup simpleKeyGroup = keyGroup.ToSimpleKeyGroup();
            foreach (CorsairKeyboardKeyId keyId in keyIds)
                simpleKeyGroup.RemoveKey(keyId);
            return simpleKeyGroup;
        }

        public static ListKeyGroup Exclude(this BaseKeyGroup keyGroup, params CorsairKey[] keyIds)
        {
            ListKeyGroup simpleKeyGroup = keyGroup.ToSimpleKeyGroup();
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
