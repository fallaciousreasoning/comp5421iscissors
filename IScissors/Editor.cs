using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IScissors.Filters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IScissors
{
    public class Editor
    {
        private Input Input { get; set; }
        private GraphicsDevice Device { get; set; }

        private Vector2 cameraPos = new Vector2(0);
        public float Zoom = 1;

        private Matrix world = Matrix.Identity;

        private readonly SpriteBatch spriteBatch;
        public Scissors Scissors { get; private set; }

        public float ScreenWidth { get { return TextureUtil.Device.Viewport.Width; } }
        public float ScreenHeight { get { return TextureUtil.Device.Viewport.Height; } }
        public float ImageWidth { get { return texture.Width; } }
        public float ImageHeight { get { return texture.Height; } }

        private Texture2D texture;

        public Editor(GraphicsDevice device, Input input)
        {
            Device = device;
            Input = input;
            TextureUtil.Device = device;

            spriteBatch = new SpriteBatch(Device);
            texture = Texture2D.FromStream(Device, File.OpenRead("Content\\ferry.bmp"));

            Scissors = new Scissors();
            Scissors.Load(texture);
        }

        public void Load(Texture2D texture)
        {
            this.texture = texture;
            Scissors.Load(texture);

            Zoom = 1;
            cameraPos = Vector2.Zero;
        }

        public void Reset()
        {
            Scissors.Clear();
        }

        public void Update()
        {
            if (Game1.Menu?.HasMouse ?? false)
                return;

            world = Matrix.CreateTranslation(new Vector3(cameraPos, 0))*Matrix.CreateScale(Zoom);

            var mouseWorldPos = Input.MousePosition/Zoom - cameraPos;
            var mousePoint = new Point((int)mouseWorldPos.X, (int)mouseWorldPos.Y);

            Scissors.SetMousePos(mousePoint.X, mousePoint.Y);
            Scissors.Update();

            if (Input.Pressed(MouseButton.Left) && mousePoint.X >= 0 && mousePoint.Y >= 0 && mousePoint.X < ImageWidth && mousePoint.Y < ImageHeight && Scissors.Active) {
                if(!Scissors.FirstPoint.HasValue || Vector2.Distance(Scissors.FirstPoint.Value, mouseWorldPos) > 5)
                    Scissors.AddSeed(mousePoint.X, mousePoint.Y);
                else Scissors.Close();
            }

            if (Input.Pressed(Keys.OemPlus))
                Zoom += 1;
            if (Input.Pressed(Keys.OemMinus))
                Zoom -= 1;

            Zoom = MathHelper.Clamp(Zoom, 0.25f, 4);

            if (Input.Pressed(Keys.Left))
                cameraPos.X -= 10;
            if (Input.Pressed(Keys.Right))
                cameraPos.X += 10;
            if (Input.Pressed(Keys.Up))
                cameraPos.Y -= 10;
            if (Input.Pressed(Keys.Down))
                cameraPos.Y += 10;

            if (Input.Down(MouseButton.Right))
                cameraPos += Input.MouseMoved/Zoom;

            var imageSize = new Vector2(ImageWidth, ImageHeight);
            cameraPos = Vector2.Clamp(cameraPos, -imageSize,
                new Vector2(ScreenWidth, ScreenHeight)/Zoom);
        }

        public void Draw()
        {
            spriteBatch.Begin(0, null, null, null, null, null, world);
            Scissors.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void ResetView()
        {
            Zoom = 1;
            cameraPos =Vector2.Zero;
        }
    }
}
