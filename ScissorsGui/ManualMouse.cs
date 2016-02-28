using IScissors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ScissorsGui
{
    public class ManualMouse : IMouse
    {
        public Vector2 MousePosition { get; set; }
        public ButtonState LeftButton { get; set; }
        public ButtonState RightButton { get; set; }
        public ButtonState MiddleButton { get; set; }
        public float ScrollWheelValue { get; set; }

        public bool IsKeyDown(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return LeftButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    return MiddleButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return RightButton == ButtonState.Pressed;
            }
            return false;
        }

        public bool IsKeyUp(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return LeftButton == ButtonState.Released;
                case MouseButton.Middle:
                    return MiddleButton == ButtonState.Released;
                case MouseButton.Right:
                    return RightButton == ButtonState.Released;
            }
            return false;
        }

        public void Update()
        {
            
        }

        public IMouse Clone()
        {
            return new ManualMouse()
            {
                LeftButton = LeftButton,
                RightButton = RightButton,
                MiddleButton = MiddleButton,
                MousePosition = MousePosition,
                ScrollWheelValue = ScrollWheelValue
            };
        }
    }
}
