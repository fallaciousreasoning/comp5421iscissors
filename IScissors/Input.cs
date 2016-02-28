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
        private IMouse mouseState;
        private IMouse lastMouseState;

        private IKeyboard keyboardState;
        private IKeyboard lastKeyboardState;

        public Vector2 LastMousePosition => lastMouseState.MousePosition;

        public Vector2 MousePosition => mouseState.MousePosition;

        public Vector2 MouseMoved
        {
            get { return MousePosition - LastMousePosition; }
        }

        public Input(IMouse mouse, IKeyboard keyboard, Game game = null)
            :base(game)
        {
            mouseState = mouse;
            keyboardState = keyboard;

            //Stop problems with clicking on first frame
            lastMouseState = mouse;
            lastKeyboardState = keyboard;
        }

        public Input(Game game=null) : base(game)
        {
            mouseState = new XnaMouse();
            keyboardState = new XnaKeyboard();
        }

        public Vector2 WorldPosition(Matrix world, Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, world);
        }

        public override void Update(GameTime gameTime)
        {
            lastKeyboardState = keyboardState.Clone();
            lastMouseState = mouseState.Clone();

            keyboardState.Update();
            mouseState.Update();

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

        public bool Released(Keys key)
        {
            return keyboardState.IsKeyUp(key) && lastKeyboardState.IsKeyDown(key);
        }

        public bool Down(MouseButton button)
        {
            return mouseState.IsKeyDown(button);
        }

        public bool Pressed(MouseButton button)
        {
            return mouseState.IsKeyDown(button) && lastMouseState.IsKeyUp(button);
        }

        public bool Released(MouseButton button)
        {
            return mouseState.IsKeyUp(button) && lastMouseState.IsKeyDown(button);
        }
    }
}
