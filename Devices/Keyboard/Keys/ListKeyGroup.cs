using System.Collections.Generic;
using CUE.NET.Devices.Keyboard.Enums;

namespace CUE.NET.Devices.Keyboard.Keys
{
    public class ListKeyGroup : BaseKeyGroup
    {
        #region Properties & Fields

        protected IList<CorsairKey> GroupKeys { get; } = new List<CorsairKey>();

        #endregion

        #region Constructors

        public ListKeyGroup(CorsairKeyboard keyboard, bool autoAttach = true)
            : base(keyboard, autoAttach)
        { }

        public ListKeyGroup(CorsairKeyboard keyboard, params CorsairKey[] keys)
            : this(keyboard, true, keys)
        { }

        public ListKeyGroup(CorsairKeyboard keyboard, bool autoAttach, params CorsairKey[] keys)
            : base(keyboard, autoAttach)
        {
            AddKey(keys);
        }

        public ListKeyGroup(CorsairKeyboard keyboard, params CorsairKeyboardKeyId[] keys)
            : this(keyboard, true, keys)
        { }

        public ListKeyGroup(CorsairKeyboard keyboard, bool autoAttach, params CorsairKeyboardKeyId[] keys)
            : base(keyboard, autoAttach)
        {
            AddKey(keys);
        }

        #endregion

        #region Methods

        public void AddKey(params CorsairKey[] keys)
        {
            if (keys != null)
                foreach (CorsairKey key in keys)
                    if (key != null && !ContainsKey(key))
                        GroupKeys.Add(key);

        }

        public void AddKey(params CorsairKeyboardKeyId[] keyIds)
        {
            if (keyIds != null)
                foreach (CorsairKeyboardKeyId keyId in keyIds)
                    AddKey(Keyboard[keyId]);
        }

        public void RemoveKey(params CorsairKey[] keys)
        {
            if (keys != null)
                foreach (CorsairKey key in keys)
                    if (key != null)
                        GroupKeys.Remove(key);
        }

        public void RemoveKey(params CorsairKeyboardKeyId[] keyIds)
        {
            if (keyIds != null)
                foreach (CorsairKeyboardKeyId keyId in keyIds)
                    RemoveKey(Keyboard[keyId]);
        }

        public bool ContainsKey(CorsairKey key)
        {
            return key != null && GroupKeys.Contains(key);
        }

        public bool ContainsKey(CorsairKeyboardKeyId keyId)
        {
            return ContainsKey(Keyboard[keyId]);
        }

        public void MergeKeys(IKeyGroup groupToMerge)
        {
            foreach (CorsairKey key in groupToMerge.Keys)
                if (!GroupKeys.Contains(key))
                    GroupKeys.Add(key);
        }


        protected override IList<CorsairKey> GetGroupKeys()
        {
            return GroupKeys;
        }

        #endregion
    }
}
