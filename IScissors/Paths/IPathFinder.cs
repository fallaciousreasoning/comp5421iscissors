using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IScissors.Paths
{
    public interface IPathFinder
    {
        void SetSeed(int x, int y);
        LinkedList<Point> FindPath(int endX, int endY);
    }
}
