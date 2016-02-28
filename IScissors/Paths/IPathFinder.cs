using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IScissors.Paths
{
    public interface IPathFinder
    {
        void SetSeed(int x, int y);
        LinkedList<Point> FindPath(int endX, int endY);
    }
}
