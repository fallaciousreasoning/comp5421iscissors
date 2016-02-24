using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IScissors.Filters;
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

        public static BasicImage operator +(BasicImage first, BasicImage second)
        {
            if (first.Width != second.Width || first.Height!= second.Height) throw new ArgumentException();

            var resultColors = new Color[first.Width, second.Width];

            for (var i = 0; i < first.Width; ++i)
                for (var j = 0; j < first.Height; ++j)
                {
                    var firstColor = first.Colors[i, j];
                    var secondColor = second.Colors[i, j];

                    byte r, g, b;

                    //Taking the max
                    //r = Math.Max(firstColor.R, secondColor.R);
                    //g = Math.Max(firstColor.G, secondColor.G);
                    //b = Math.Max(firstColor.B, secondColor.B);

                    //Combining
                    r = (byte)Math.Max(0, Math.Min(firstColor.R + secondColor.R, 255));
                    g = (byte)Math.Max(0, Math.Min(firstColor.G + secondColor.G, 255));
                    b = (byte)Math.Max(0, Math.Min(firstColor.B + secondColor.B, 255));

                    var resultColor = new Color(r, g, b); //Averageing Color.Lerp(firstColor, secondColor, 0.5f);
                    resultColors[i, j] = resultColor;
                }

            return new BasicImage(resultColors);
        }
    }
}
