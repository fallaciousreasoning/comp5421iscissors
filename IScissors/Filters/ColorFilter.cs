using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IScissors.Filters
{
    public class ColorFilter : IFilter
    {
        public static readonly ColorFilter GrayScale = new ColorFilter(c =>
        {
            var average = (int)((c.R + c.B + c.G)/3.0f);
            return new Color(average, average,average);
        });

        public static readonly ColorFilter BlackAndWhite = new ColorFilter(c =>
        {
            var blackness = ((c.R + c.G + c.B)/3.0/255.0);
            return blackness < 0.5 ? Color.Black : Color.White;
        });

        public static readonly ColorFilter RGB = new ColorFilter(c =>
        {
            if (c.R >= c.G && c.R >= c.B) return Color.Red;
            if (c.G >= c.B) return Color.Green;
            return Color.Blue;
        });

        private readonly Func<Color, Color> transform; 

        public ColorFilter(Func<Color, Color> transform)
        {
            this.transform = transform;
        }


        public BasicImage Apply(BasicImage input)
        {
            var colors = input.Colors;
            var result = new Color[input.Width, input.Height];

            for (var i = 0; i < input.Width; ++i)
                for (var j = 0; j < input.Height; ++j)
                {
                    result[i, j] = transform(colors[i, j]);
                }

            return new BasicImage(result);
        }
    }
}
