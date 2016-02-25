using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IScissors.Extensions
{
    public static class ColorExtensions
    {
        public static float Intensity(this Color color)
        {
            return (color.R + color.B + color.G)/3f;
        }

        public static Color FromIntensity(float intensity)
        {
            var b = (byte) (intensity*255);
            return new Color(b,b,b);
        }
    }
}
