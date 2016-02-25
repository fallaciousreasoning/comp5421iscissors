using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace IScissors
{
    public class Input : GameComponent
    {
        private MouseState mouseState;
        private MouseState lastMouseState;

        private KeyboardState keyboardState;
        private KeyboardState lastKeyboardState;

        public Vector2 MousePosition
        {
            get { return new Vector2(mouseState.X, mouseState.Y); }
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
