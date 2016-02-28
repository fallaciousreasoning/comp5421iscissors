using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace IScissors
{
    public class XnaMouse : IMouse
    {
        private MouseState state;

        public Vector2 MousePosition { get { return new Vector2(state.X, state.Y); } }

        public bool IsKeyDown(MouseButton button)
        {
            switch (button)
            {
                    case MouseButton.Left:
                    return state.LeftButton == ButtonState.Pressed;
                    case MouseButton.Middle:
                    return state.MiddleButton == ButtonState.Pressed;
                    case MouseButton.Right:
                    return state.RightButton == ButtonState.Pressed;
            }
            return false;
        }

        public bool IsKeyUp(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return state.LeftButton == ButtonState.Released;
                case MouseButton.Middle:
                    return state.MiddleButton == ButtonState.Released;
                case MouseButton.Right:
                    return state.RightButton == ButtonState.Released;
            }
            return false;
        }

        public void Update()
        {
            state = Mouse.GetState();
        }

        public IMouse Clone()
        {
            return new XnaMouse() {state = state};
        }
    }

    public interface IMouse
    {
        Vector2 MousePosition { get; }
        bool IsKeyDown(MouseButton button);
        bool IsKeyUp(MouseButton button);
        void Update();
        IMouse Clone();
    }
}
