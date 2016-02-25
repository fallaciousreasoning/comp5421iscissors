using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IScissors.Filters
{
    public class Sobel2 : IFilter
    {
        private readonly static float[,] Sobel =
        {
            {-1, 0, 1},
            {-2, 0, 2},
            {-1, 0, 1}
        };

        private int KernelSize
        {
            get { return Sobel.GetLength(0); }
        }

        private int HalfKernelSize
        {
            get { return KernelSize/2; }
        }

        public float[,] LastDirections { get; private set; }
        public float[,] LastMagnitudes { get; private set; }
        public float LastMax { get; private set; }

        public Sobel2()
        {

        }

        public BasicImage Apply(BasicImage input)
        {
            var result = new Color[input.Width, input.Height];
            var direction = new float[input.Width, input.Height];

            var gX = new float[input.Width, input.Height];
            var gY = new float[input.Width, input.Height];
            var total = new float[input.Width, input.Height];

            var sum = 0f;
            var max = 0f;

            for (var x = HalfKernelSize; x < input.Width - HalfKernelSize; ++x)
            {
                for (var y = HalfKernelSize; y < input.Height - HalfKernelSize; ++y)
                {
                    sum = 0;

                    for (var i = 0; i < KernelSize; ++i)
                    {
                        for (var j = 0; j < KernelSize; ++j)
                        {
                            var curX = x - HalfKernelSize + i;
                            var curY = y - HalfKernelSize + j;

                            var color = input.Colors[curX, curY];
                            var intensity = IntensityOf(color);

                            var value = intensity*Sobel[i, j];
                            sum += value;
                        }
                    }

                    gY[x, y] = sum;

                    for (var i = 0; i < KernelSize; ++i)
                    {
                        for (var j = 0; j < KernelSize; ++j)
                        {
                            var curX = x - HalfKernelSize + i;
                            var curY = y - HalfKernelSize + j;

                            var color = input.Colors[curX, curY];
                            var intensity = IntensityOf(color);

                            var value = intensity * Sobel[j, i];
                            sum += value;
                        }
                    }

                    gX[x, y] = sum;
                }
            }

            for (var x = 0; x < input.Width; ++x)
                for (var y = 0; y < input.Height; ++y)
                {
                    total[x, y] = (int) Math.Sqrt(Math.Pow(gX[x, y], 2) + Math.Pow(gY[x, y], 2));
                    direction[x, y] = (float)Math.Atan2(gX[x, y], gY[x, y]);

                    if (max < total[x, y])
                        max = total[x, y];
                }

            var ratio = max/255f;
            for (var x = 0; x <input.Width; ++x)
                for (var y = 0; y < input.Height; ++y)
                {
                    var value = (int) (total[x, y]/ratio);
                    result[x, y] = new Color(value, value, value);
                }

            LastDirections = direction;
            LastMagnitudes = total;
            LastMax = max;

            return new BasicImage(result);
        }

        private byte IntensityOf(Color color)
        {
            return (byte) ((color.R + color.G + color.B)/3);
        }
    }
}
