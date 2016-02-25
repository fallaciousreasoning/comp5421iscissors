using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IScissors
{
    public static class TextureUtil
    {
        public static Texture2D CreateTexture(int width, int height, Color color)
        {
            var colors = new Color[width*height];
            for (var i = 0; i < colors.Length; ++i) colors[i] = color;

            var texture = new Texture2D(Game1.Device, width, height);
            texture.SetData(colors);
            return texture;
        }
    }
}
