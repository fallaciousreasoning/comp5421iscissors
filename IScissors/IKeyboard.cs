using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace IScissors
{
    public class XnaKeyboard : IKeyboard
    {
        private KeyboardState state;

        public bool IsKeyDown(Keys key)
        {
            return state.IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return state.IsKeyUp(key);
        }

        public void Update()
        {
            state = Keyboard.GetState();
        }

        public IKeyboard Clone()
        {
            return new XnaKeyboard() {state = state};
        }
    }

    public interface IKeyboard
    {
        bool IsKeyDown(Keys key);
        bool IsKeyUp(Keys key);
        void Update();

        IKeyboard Clone();
    }
}
