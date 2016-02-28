using System.Collections.Generic;
using IScissors;
using Microsoft.Xna.Framework.Input;

namespace ScissorsGui
{
    public class ManualKeyboard : IKeyboard
    {
        private Dictionary<Keys, bool> keyStates = new Dictionary<Keys, bool>();

        public void Set(Keys key, bool value)
        {
            if (!keyStates.ContainsKey(key))
                keyStates.Add(key, false);

            keyStates[key] = value;
        }

        public bool IsKeyDown(Keys key)
        {
            if (!keyStates.ContainsKey(key)) return false;
            return keyStates[key];
        }

        public bool IsKeyUp(Keys key)
        {
            if (!keyStates.ContainsKey(key)) return true;
            return !keyStates[key];
        }

        public void Update()
        {
        }

        public IKeyboard Clone()
        {
            return new ManualKeyboard() {keyStates = keyStates};
        }
    }
}
