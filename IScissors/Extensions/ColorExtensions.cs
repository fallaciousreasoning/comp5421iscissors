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
    }
}
