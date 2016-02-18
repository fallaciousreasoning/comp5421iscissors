using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IScissors
{
    public class BasicImage
    {
        public Color[,] Colors { get; private set; }
        public int Width { get { return Colors.GetLength(0); } }
        public int Height { get { return Colors.GetLength(1); } }

        public BasicImage(Color[,] colors)
        {
            Colors = colors;
        }

        public static BasicImage FromTexture(Texture2D texture)
        {
            var colors = new Color[texture.Width*texture.Height];
            texture.GetData(colors);

            var image = new BasicImage(new Color[texture.Width, texture.Height]);

            for (var i = 0; i < image.Width; ++i)
                for (var j = 0; j < image.Height; ++j)
                {
                    image.Colors[i, j] = colors[i + j*image.Width];
                }

            return image;
        }

        public Texture2D ToTexture()
        {
            var colors= new Color[Width*Height];
            for (var i = 0; i < Width; ++i)
                for (var j = 0; j < Height; ++j)
                {
                    colors[i + j*Width] = Colors[i, j];
                }

            var texture = new Texture2D(Game1.Device, Width, Height);
            texture.SetData(colors);
            return texture;
        }
    }
}
