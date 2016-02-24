using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IScissors.Paths
{
    public interface IPathFinder
    {
        PixelNode FindPath(BasicImage image, int startX, int startY, int endX, int endY);
    }
}
