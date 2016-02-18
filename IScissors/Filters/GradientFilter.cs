using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IScissors.Filters
{
    public class GradientFilter : IFilter
    {

        public BasicImage Apply(BasicImage input)
        {
            var result = new Color[input.Width, input.Height];
            var colors = input.Colors;

            for (var i = 0; i < input.Width; ++i)
                for (var j = 0; j < input.Height; ++j)
                {
                    
                }

            return new BasicImage(result);
        }

        private byte GetGradient(BasicImage input, int x, int y)
        {

            for (var i= -1; i <= 1; ++i)
                for (var j = -1; j <= 1; ++j)
                {
                    Color color;
                    var curX = x + i;
                    var curY = y + j;

                    if (curY < 0 || curX < 0 || curY >= input.Height || curX >= input.Height)
                        color = input.Colors[x, y];
                    else color = input.Colors[curX, curY];

                    var intensity = IntensityOf(color);
                }
            return 0;
        }

        private byte IntensityOf(Color color)
        {
            return (byte)((color.R + color.G + color.B)/3.0f);
        }
    }
}
