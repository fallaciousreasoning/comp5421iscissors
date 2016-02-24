using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IScissors.Filters
{
    public class ConvulutionFilter : IFilter
    {
        public static readonly ConvulutionFilter SobelHorizontal = new ConvulutionFilter(new float[,]
        {
            { -1, 0, 1 },
            {-2, 0, 2 },
            {-1, 0, 1 }
        });

        public static readonly ConvulutionFilter SobelVertical = new ConvulutionFilter(new float[,]
        {
            {-1, -2, -1},
            {0, 0, 0},
            {1, 2, 1}
        });

        public static readonly ConvulutionFilter Emboss = new ConvulutionFilter(new[,]
        {
            {-1, -1, 0f },
            {-1, 0, 1f },
            {0f, 1f, 1f }
        }, 128);

        public static readonly ConvulutionFilter MeanFilter = new ConvulutionFilter(new [,]
        {
            {1,1,1 },
            {1,1f,1 },
            {1,1,1 }
        }, 0 , 1/9.0f);

        public static readonly ConvulutionFilter Sharpen = new ConvulutionFilter(new[,]
        {
            {-1f, -1f, -1f},
            {-1f, 9f, -1f},
            {-1f, -1f, -1f}
        });

        public static readonly ConvulutionFilter LaplacianOfTheGuassian = new ConvulutionFilter(new[,]
        {
            {-1f, -1f, -1f},
            {-1f, 8f, -1f},
            {-1f, -1f, -1f}
        });

        public static readonly ConvulutionFilter MotionBlur = new ConvulutionFilter(new[,]
        {
            {1, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 1, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 1, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 1f, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 1, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 1}
        }, 0, 1/9.0f);

        public static readonly ConvulutionFilter Blur = new ConvulutionFilter(new[,]
        {
            {0, 0.2f, 0},
            {0.2f, 0.2f, 0.2f},
            {0, 0.2f, 0}
        });

        public static readonly ConvulutionFilter Identity = new ConvulutionFilter(new [,]
        {
            {0,0,0 },
            {0,1f,0 },
            {0,0,0 }
        });

        private readonly float[,] filter;

        private readonly float bias;
        private readonly float factor;

        private int FilterWidth { get { return filter.GetLength(0); } }
        private int FilterHeight { get { return filter.GetLength(1); } }

        public ConvulutionFilter(float[,] filter, float bias=0.0f, float factor=1.0f)
        {
            this.filter = filter;
            this.bias = bias;
            this.factor = factor;
        }

        public BasicImage Apply(BasicImage input)
        {
            var colors = input.Colors;
            var imageWidth = colors.GetLength(0);
            var imageHeight = colors.GetLength(1);

            var result = new Color[colors.GetLength(0), colors.GetLength(1)];

            for (var x = 0; x < colors.GetLength(0); ++x)
            {
                for (var y = 0; y < colors.GetLength(1); ++y)
                {
                    float red = 0, green =0, blue=0;

                    for (var filterX = 0; filterX < filter.GetLength(0); ++filterX)
                    {
                        for (var filterY = 0; filterY < filter.GetLength(1); ++filterY)
                        {
                            var imageX = x - FilterWidth/2 + filterX;
                            var imageY = y - FilterHeight/2 + filterY;

                            if (imageX < 0 || imageX >= imageWidth) imageX = x;
                            if (imageY < 0 || imageY >= imageHeight) imageY = y;

                            var filterValue = filter[filterX, filterY];
                            var color = colors[imageX, imageY];

                            red += color.R*filterValue;
                            green += color.G*filterValue;
                            blue += color.B*filterValue;
                        }
                    }

                    red = Math.Max(0, Math.Min(red*factor + bias, 255));
                    green = Math.Max(0, Math.Min(green*factor + bias, 255));
                    blue = Math.Max(0, Math.Min(blue*factor + bias, 255));

                    result[x,y] = new Color((byte)red, (byte)green, (byte)blue);
                }
            }

            return new BasicImage(result);
        }
    }
}
