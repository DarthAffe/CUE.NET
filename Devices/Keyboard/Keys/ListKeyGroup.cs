// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using CUE.NET.Devices.Keyboard.Enums;

namespace CUE.NET.Devices.Keyboard.Keys
{
    /// <summary>
    /// Represents a keygroup containing arbitrary keys.
    /// </summary>
    public class ListKeyGroup : AbstractKeyGroup
    {
        #region Properties & Fields

        protected override IKeyGroup EffectTarget => this;

        /// <summary>
        /// Gets the list containing the keys of this keygroup.
        /// </summary>
        protected IList<CorsairKey> GroupKeys { get; } = new List<CorsairKey>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListKeyGroup"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard this keygroup belongs to.</param>
        /// <param name="autoAttach">Specifies whether this keygroup should be automatically attached or not.</param>
        public ListKeyGroup(CorsairKeyboard keyboard, bool autoAttach = true)
            : base(keyboard, autoAttach)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListKeyGroup"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard this keygroup belongs to.</param>
        /// <param name="keys">The initial keys of this keygroup.</param>
        public ListKeyGroup(CorsairKeyboard keyboard, params CorsairKey[] keys)
            : this(keyboard, true, keys)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListKeyGroup"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard this keygroup belongs to.</param>
        /// <param name="keys">The initial keys of this keygroup.</param>
        public ListKeyGroup(CorsairKeyboard keyboard, IEnumerable<CorsairKey> keys)
            : this(keyboard, true, keys)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListKeyGroup"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard this keygroup belongs to.</param>
        /// <param name="autoAttach">Specifies whether this keygroup should be automatically attached or not.</param>
        /// <param name="keys">The initial keys of this keygroup.</param>
        public ListKeyGroup(CorsairKeyboard keyboard, bool autoAttach, IEnumerable<CorsairKey> keys)
            : base(keyboard, autoAttach)
        {
            AddKeys(keys);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListKeyGroup"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard this keygroup belongs to.</param>
        /// <param name="autoAttach">Specifies whether this keygroup should be automatically attached or not.</param>
        /// <param name="keys">The initial keys of this keygroup.</param>
        public ListKeyGroup(CorsairKeyboard keyboard, bool autoAttach, params CorsairKey[] keys)
            : base(keyboard, autoAttach)
        {
            AddKeys(keys);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListKeyGroup"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard this keygroup belongs to.</param>
        /// <param name="keys">The IDs of the initial keys of this keygroup.</param>
        public ListKeyGroup(CorsairKeyboard keyboard, params CorsairKeyboardKeyId[] keys)
            : this(keyboard, true, keys)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListKeyGroup"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard this keygroup belongs to.</param>
        /// <param name="keys">The IDs of the initial keys of this keygroup.</param>
        public ListKeyGroup(CorsairKeyboard keyboard, IEnumerable<CorsairKeyboardKeyId> keys)
            : this(keyboard, true, keys)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListKeyGroup"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard this keygroup belongs to.</param>
        /// <param name="autoAttach">Specifies whether this keygroup should be automatically attached or not.</param>
        /// <param name="keys">The IDs of the initial keys of this keygroup.</param>
        public ListKeyGroup(CorsairKeyboard keyboard, bool autoAttach, params CorsairKeyboardKeyId[] keys)
            : base(keyboard, autoAttach)
        {
            AddKeys(keys);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListKeyGroup"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard this keygroup belongs to.</param>
        /// <param name="autoAttach">Specifies whether this keygroup should be automatically attached or not.</param>
        /// <param name="keys">The IDs of the initial keys of this keygroup.</param>
        public ListKeyGroup(CorsairKeyboard keyboard, bool autoAttach, IEnumerable<CorsairKeyboardKeyId> keys)
            : base(keyboard, autoAttach)
        {
            AddKeys(keys);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the given key(s) to the keygroup.
        /// </summary>
        /// <param name="keys">The key(s) to add.</param>
        public void AddKey(params CorsairKey[] keys)
        {
            AddKeys(keys);
        }

        /// <summary>
        /// Adds the given key(s) to the keygroup.
        /// </summary>
        /// <param name="keyIds">The ID(s) of the key(s) to add.</param>
        public void AddKey(params CorsairKeyboardKeyId[] keyIds)
        {
            AddKeys(keyIds);
        }

        /// <summary>
        /// Adds the given keys to the keygroup.
        /// </summary>
        /// <param name="keys">The keys to add.</param>
        public void AddKeys(IEnumerable<CorsairKey> keys)
        {
            if (keys != null)
                foreach (CorsairKey key in keys)
                    if (key != null && !ContainsKey(key))
                        GroupKeys.Add(key);
        }

        /// <summary>
        /// Adds the given keys to the keygroup.
        /// </summary>
        /// <param name="keyIds">The IDs of the keys to add.</param>
        public void AddKeys(IEnumerable<CorsairKeyboardKeyId> keyIds)
        {
            if (keyIds != null)
                foreach (CorsairKeyboardKeyId keyId in keyIds)
                    AddKey(Keyboard[keyId]);
        }

        /// <summary>
        /// Removes the given key(s) from the keygroup.
        /// </summary>
        /// <param name="keys">The key(s) to remove.</param>
        public void RemoveKey(params CorsairKey[] keys)
        {
            RemoveKeys(keys);
        }

        /// <summary>
        /// Removes the given key(s) from the keygroup.
        /// </summary>
        /// <param name="keyIds">The ID(s) of the key(s) to remove.</param>
        public void RemoveKey(params CorsairKeyboardKeyId[] keyIds)
        {
            RemoveKeys(keyIds);
        }

        /// <summary>
        /// Removes the given keys from the keygroup.
        /// </summary>
        /// <param name="keys">The keys to remove.</param>
        public void RemoveKeys(IEnumerable<CorsairKey> keys)
        {
            if (keys != null)
                foreach (CorsairKey key in keys)
                    if (key != null)
                        GroupKeys.Remove(key);
        }

        /// <summary>
        /// Removes the given keys from the keygroup.
        /// </summary>
        /// <param name="keyIds">The IDs of the keys to remove.</param>
        public void RemoveKeys(IEnumerable<CorsairKeyboardKeyId> keyIds)
        {
            if (keyIds != null)
                foreach (CorsairKeyboardKeyId keyId in keyIds)
                    RemoveKey(Keyboard[keyId]);
        }

        /// <summary>
        /// Checks if a given key is contained by this keygroup.
        /// </summary>
        /// <param name="key">The key which should be checked.</param>
        /// <returns><c>true</c> if the key is contained by this keygroup; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(CorsairKey key)
        {
            return key != null && GroupKeys.Contains(key);
        }

        /// <summary>
        /// Checks if a given key is contained by this keygroup.
        /// </summary>
        /// <param name="keyId">The ID of the key which should be checked.</param>
        /// <returns><c>true</c> if the key is contained by this keygroup; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(CorsairKeyboardKeyId keyId)
        {
            return ContainsKey(Keyboard[keyId]);
        }

        /// <summary>
        /// Merges the keys from the given keygroup in this keygroup. 
        /// </summary>
        /// <param name="groupToMerge">The keygroup to merge.</param>
        public void MergeKeys(IKeyGroup groupToMerge)
        {
            foreach (CorsairKey key in groupToMerge.Keys)
                if (!GroupKeys.Contains(key))
                    GroupKeys.Add(key);
        }

        /// <summary>
        /// Gets a list containing the keys from this group.
        /// </summary>
        /// <returns>The list containing the keys.</returns>
        protected override IList<CorsairKey> GetGroupKeys()
        {
            return GroupKeys;
        }

        #endregion
    }
}
