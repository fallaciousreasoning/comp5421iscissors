using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IScissors.Paths
{
    public enum NodeState
    {
        Initial,
        Active,
        Expanded,
    }

    public class PixelNode
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float[] LinkCosts { get; set; }
    }
}
