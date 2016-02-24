using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IScissors.Filters
{
    public class SobelFilter : IFilter
    {
        public BasicImage Apply(BasicImage input)
        {
            var horizontal = ConvulutionFilter.SobelHorizontal.Apply(input);
            var vertical = ConvulutionFilter.SobelVertical.Apply(input);

            return horizontal + vertical;
        }
    }
}
