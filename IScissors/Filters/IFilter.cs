using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IScissors.Filters
{
    public interface IFilter
    {
        BasicImage Apply(BasicImage input);
    }
}
