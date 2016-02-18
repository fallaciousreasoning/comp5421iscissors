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
            var horizontal = BasicFilter.SobelHorizontal.Apply(input);
            var vertical = BasicFilter.SobelVertical.Apply(input);

            return horizontal + vertical;
        }
    }
}
