using CUE.NET.Devices.Keyboard.Enums;

namespace CUE.NET.Devices.Keyboard.Keys
{
    public class SimpleKeyGroup : BaseKeyGroup
    {
        #region Constructors

        public SimpleKeyGroup(CorsairKeyboard keyboard)
            : base(keyboard)
        { }

        public SimpleKeyGroup(CorsairKeyboard keyboard, params CorsairKey[] keys)
            : base(keyboard)
        {
            AddKey(keys);
        }

        public SimpleKeyGroup(CorsairKeyboard keyboard, params CorsairKeyboardKeyId[] keys)
            : base(keyboard)
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
                        Keys.Add(key);

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
                        Keys.Remove(key);
        }

        public void RemoveKey(params CorsairKeyboardKeyId[] keyIds)
        {
            if (keyIds != null)
                foreach (CorsairKeyboardKeyId keyId in keyIds)
                    RemoveKey(Keyboard[keyId]);
        }

        public bool ContainsKey(CorsairKey key)
        {
            return key != null && Keys.Contains(key);
        }

        public bool ContainsKey(CorsairKeyboardKeyId keyId)
        {
            return ContainsKey(Keyboard[keyId]);
        }

        #endregion
    }
}
