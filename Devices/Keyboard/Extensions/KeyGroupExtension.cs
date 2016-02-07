// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Linq;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Devices.Keyboard.Keys;

namespace CUE.NET.Devices.Keyboard.Extensions
{
    /// <summary>
    /// Offers some extensions and helper-methods for keygroup related things.
    /// </summary>
    public static class KeyGroupExtension
    {
        /// <summary>
        /// Converts the given <see cref="BaseKeyGroup" /> to a <see cref="ListKeyGroup" />.
        /// </summary>
        /// <param name="keyGroup">The <see cref="BaseKeyGroup" /> to convert.</param>
        /// <returns>The converted <see cref="ListKeyGroup" />.</returns>
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

        /// <summary>
        /// Returns a new <see cref="ListKeyGroup" /> which contains all keys from the given keygroup excluding the specified ones.
        /// </summary>
        /// <param name="keyGroup">The base keygroup.</param>
        /// <param name="keyIds">The ids of the keys to exclude.</param>
        /// <returns>The new <see cref="ListKeyGroup" />.</returns>
        public static ListKeyGroup Exclude(this BaseKeyGroup keyGroup, params CorsairKeyboardKeyId[] keyIds)
        {
            ListKeyGroup simpleKeyGroup = keyGroup.ToSimpleKeyGroup();
            foreach (CorsairKeyboardKeyId keyId in keyIds)
                simpleKeyGroup.RemoveKey(keyId);
            return simpleKeyGroup;
        }

        /// <summary>
        /// Returns a new <see cref="ListKeyGroup" /> which contains all keys from the given keygroup excluding the specified ones.
        /// </summary>
        /// <param name="keyGroup">The base keygroup.</param>
        /// <param name="keyIds">The keys to exclude.</param>
        /// <returns>The new <see cref="ListKeyGroup" />.</returns>
        public static ListKeyGroup Exclude(this BaseKeyGroup keyGroup, params CorsairKey[] keyIds)
        {
            ListKeyGroup simpleKeyGroup = keyGroup.ToSimpleKeyGroup();
            foreach (CorsairKey key in keyIds)
                simpleKeyGroup.RemoveKey(key);
            return simpleKeyGroup;
        }

        // ReSharper disable once UnusedMethodReturnValue.Global
        /// <summary>
        /// Attaches the given keygroup to the keyboard.
        /// </summary>
        /// <param name="keyGroup">The keygroup to attach.</param>
        /// <returns><c>true</c> if the keygroup could be attached; otherwise, <c>false</c>.</returns>
        public static bool Attach(this BaseKeyGroup keyGroup)
        {
            return keyGroup.Keyboard?.AttachKeyGroup(keyGroup) ?? false;
        }

        /// <summary>
        /// Detaches the given keygroup from the keyboard.
        /// </summary>
        /// <param name="keyGroup">The keygroup to attach.</param>
        /// <returns><c>true</c> if the keygroup could be detached; otherwise, <c>false</c>.</returns>
        public static bool Detach(this BaseKeyGroup keyGroup)
        {
            return keyGroup.Keyboard?.DetachKeyGroup(keyGroup) ?? false;
        }
    }
}
