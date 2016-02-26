using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace IScissors
{
    public enum MouseButton
    {
        Left,
        Right,
        Middle
    }

    public class Input : GameComponent
    {
        private MouseState mouseState;
        private MouseState lastMouseState;

        private KeyboardState keyboardState;
        private KeyboardState lastKeyboardState;

        public Vector2 LastMousePosition
        {
            get { return new Vector2(lastMouseState.X, lastMouseState.Y); }
        }

        public Vector2 MousePosition
        {
            get { return new Vector2(mouseState.X, mouseState.Y); }
        }

        public Vector2 MouseMoved
        {
            get { return MousePosition - LastMousePosition; }
        }

        public Input(Game game) : base(game)
        {
        }

        public Vector2 WorldPosition(Matrix world, Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, world);
        }

        public override void Update(GameTime gameTime)
        {
            lastKeyboardState = keyboardState;
            lastMouseState = mouseState;

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            base.Update(gameTime);
        }

        public bool Down(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public bool Pressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
        }

        public bool Down(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return mouseState.LeftButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return mouseState.RightButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    return mouseState.MiddleButton == ButtonState.Pressed;
            }
            return false;
        }

        public bool Pressed(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return lastMouseState.LeftButton == ButtonState.Released &&
                           mouseState.LeftButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return lastMouseState.RightButton == ButtonState.Released &&
                           mouseState.RightButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    return lastMouseState.MiddleButton == ButtonState.Released &&
                           mouseState.MiddleButton == ButtonState.Pressed;
            }
            return false;
        }

        public bool Released(MouseButton button)
        {
            switch (button)
            {
                    case MouseButton.Left:
                    return lastMouseState.LeftButton == ButtonState.Pressed &&
                           mouseState.LeftButton == ButtonState.Released;
                case MouseButton.Right:
                    return lastMouseState.RightButton == ButtonState.Pressed &&
                           mouseState.RightButton == ButtonState.Released;
                case MouseButton.Middle:
                    return lastMouseState.MiddleButton == ButtonState.Pressed &&
                           mouseState.MiddleButton == ButtonState.Released;
            }
            return false;
        }

        public bool MouseDown()
        {
            return mouseState.LeftButton == ButtonState.Pressed;
        }

        public bool MouseClicked()
        {
            return mouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed;
        }
    }
}
