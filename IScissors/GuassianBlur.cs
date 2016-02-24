using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IScissors.Filters;

namespace IScissors
{
    public class GuassianBlur : IFilter
    {
        private ConvulutionFilter kernelFilter;

        public GuassianBlur(int radius, float sigma)
        {
            var n = radius*2 + 1;
            var guassianFilter = new float[n, n];
            var sum = 0f;

            for (var i = 0; i < n; ++i)
                for (var j = 0; j < n; ++j)
                {
                    //See the formula here https://en.wikipedia.org/wiki/Canny_edge_detector#Gaussian_filter
                    var value = (float)((1.0 / (2 * Math.PI * sigma * sigma)) *
                                Math.Exp(-(Math.Pow(i - radius - 1, 2) + Math.Pow(j - radius - 1, 2)) / (2 * sigma * sigma)));

                    guassianFilter[i, j] = value;
                    sum += value;
                }

            kernelFilter = new ConvulutionFilter(guassianFilter, 0, 1 / sum);
        }

        public BasicImage Apply(BasicImage input)
        {
            return kernelFilter.Apply(input);
        }
    }
}
