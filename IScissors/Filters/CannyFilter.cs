using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IScissors.Filters
{
    public class CannyFilter : IFilter
    {
        private BasicFilter guassianBlur;

        public CannyFilter(int blurSize, float sigma)
        {
            if (blurSize % 2 == 0) throw new ArgumentException("The blur size must be odd!", nameof(blurSize));

            var guassianFilter = new float[blurSize, blurSize];
            var k = blurSize/2;
            var sum = 0f;

            for (var i = 0; i < blurSize; ++i)
                for (var j = 0; j < blurSize; ++j)
                {
                    //See the formula here https://en.wikipedia.org/wiki/Canny_edge_detector#Gaussian_filter
                    var value = (float)((1.0/(2*Math.PI*sigma*sigma))*
                                Math.Exp(-(Math.Pow(i - k - 1, 2) + Math.Pow(j - k - 1, 2))/(2*sigma*sigma)));
                    
                    guassianFilter[i, j] = value;
                    sum += value;
                }

            guassianBlur = new BasicFilter(guassianFilter, 0, 1/sum);
        }
        public BasicImage Apply(BasicImage input)
        {
            var blurred = guassianBlur.Apply(input);
            var sobelX = BasicFilter.SobelHorizontal.Apply(blurred);
            var sobelY = BasicFilter.SobelVertical.Apply(blurred);

            //TODO Edge Thinning and Double Threshold
            return sobelY;
        }
    }
}
