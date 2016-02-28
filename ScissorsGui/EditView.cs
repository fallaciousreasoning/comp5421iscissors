using System.Windows;
using System.Windows.Input;
using IScissors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ScissorsGui.Controls;

namespace ScissorsGui
{
    public class EditView : D3D11Host
    {
        private IScissors.Editor editor;

        private readonly Input input;
        private readonly ManualMouse mouse;
        private readonly ManualKeyboard keyboard;

        public EditView()
        {
            Loaded += OnLoaded;

            mouse = new ManualMouse();
            keyboard = new ManualKeyboard();
            input = new Input(mouse, keyboard);
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            //SetValue(FocusableProperty, true);
            Focusable = true;
            Focus();

            var window = this;//Application.Current.MainWindow;
            window.MouseMove += (o, args) => UpdateMouse(args);
            window.MouseDown += (o, args) => UpdateMouse(args);
            window.MouseUp += (o, args) => UpdateMouse(args);
            window.MouseLeftButtonDown += (o, args) => UpdateMouse(args);
            window.MouseLeftButtonUp += (o, args) => UpdateMouse(args);
            window.MouseRightButtonDown += (o, args) => UpdateMouse(args);
            window.KeyUp += (o, e) => UpdateKeyBoard(e, false);
            window.KeyDown += (o, e) => UpdateKeyBoard(e, true);
        }

        #region Handling Input

        private void UpdateKeyBoard(KeyEventArgs e, bool pressed)
        {
            Keys key;
            Keys.TryParse(e.Key.ToString(), out key);
            keyboard.Set(key, pressed);
            e.Handled = true;
        }

        private void UpdateMouse(MouseEventArgs e)
        {
            mouse.LeftButton = FromMouseButtonState(e.LeftButton);
            mouse.RightButton = FromMouseButtonState(e.RightButton);
            mouse.MiddleButton = FromMouseButtonState(e.MiddleButton);

            var point = e.GetPosition(this);
            var pos = new Vector2((float) point.X, (float) point.Y);
            mouse.MousePosition = pos;
            e.Handled = true;
        }

        private ButtonState FromMouseButtonState(MouseButtonState state)
        {
            return state == MouseButtonState.Pressed ? ButtonState.Pressed : ButtonState.Released;
        }

        #endregion


        protected override void Initialize()
        {
            input.Initialize();

            editor = new IScissors.Editor(GraphicsDevice, input);

            base.Initialize();
        }

        protected override void LoadContent()
        {

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            input.Update(gameTime);
            editor.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            //(new SpriteBatch(null)).Begin(0, null, null, null, null, null, Matrix.Identity);
            editor.Draw();
        }
    }
}
