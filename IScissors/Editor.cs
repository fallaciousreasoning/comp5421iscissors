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
        private Input Input => Game1.Input;

        private Vector2 cameraPos = new Vector2(20);
        private float zoom = 2;

        private Matrix world = Matrix.Identity;

        private readonly SpriteBatch spriteBatch;
        private readonly Scissors scissors;

        public float ScreenWidth { get { return Game1.Device.Viewport.Width; } }
        public float ScreenHeight { get { return Game1.Device.Viewport.Height; } }
        public float ImageWidth { get { return texture.Width; } }
        public float ImageHeight { get { return texture.Height; } }

        private Texture2D texture;
        public Editor()
        {
            spriteBatch = new SpriteBatch(Game1.Device);

            texture = Texture2D.FromStream(Game1.Device, File.OpenRead("Content\\ferry.bmp"));

            scissors = new Scissors();
            scissors.Load(texture);
        }

        public void Update()
        {
            world = Matrix.CreateTranslation(new Vector3(cameraPos, 0))*Matrix.CreateScale(zoom);

            var mouseWorldPos = Input.MousePosition/zoom - cameraPos;
            var mousePoint = new Point((int)mouseWorldPos.X, (int)mouseWorldPos.Y);

            scissors.SetMousePos(mousePoint.X, mousePoint.Y);
            scissors.Update();

            if (Input.MouseClicked())
                scissors.AddSeed(mousePoint.X, mousePoint.Y);

            if (Input.Pressed(Keys.OemPlus))
                zoom += 1;
            if (Input.Pressed(Keys.OemMinus))
                zoom -= 1;

            zoom = MathHelper.Clamp(zoom, 0.25f, 4);

            if (Input.Pressed(Keys.Left))
                cameraPos.X -= 10;
            if (Input.Pressed(Keys.Right))
                cameraPos.X += 10;
            if (Input.Pressed(Keys.Up))
                cameraPos.Y -= 10;
            if (Input.Pressed(Keys.Down))
                cameraPos.Y += 10;

            if (Input.Down(MouseButton.Right))
                cameraPos += Input.MouseMoved/zoom;

            var imageSize = new Vector2(ImageWidth, ImageHeight);
            cameraPos = Vector2.Clamp(cameraPos, -imageSize,
                new Vector2(ScreenWidth, ScreenHeight)/zoom);
        }

        public void Draw()
        {
            spriteBatch.Begin(0, null, null, null, null, null, world);

            scissors.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
