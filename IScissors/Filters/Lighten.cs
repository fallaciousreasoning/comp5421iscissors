using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IScissors.Filters
{
    public class Lighten : IFilter
    {
        private ConvulutionFilter kernelFilter;

        public Lighten(float multiplier)
        {
            var kernel = new float[,]
            {
                {0, 0, 0},
                {0, multiplier, 0},
                {0, 0, 0}
            };
            kernelFilter = new ConvulutionFilter(kernel);
        }

        public BasicImage Apply(BasicImage input)
        {
            return kernelFilter.Apply(input);
        }
    }
}
