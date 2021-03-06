﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Priority_Queue;

namespace IScissors.Paths
{
    public enum NodeState
    {
        Initial,
        Active,
        Expanded,
    }

    public class PixelNode : FastPriorityQueueNode
    {
        public int X { get; set; }
        public int Y { get; set; }

        public float[] LinkDerivates { get; set; } = new float[9];
        public float[] LinkCosts { get; set; } = new float[9];
    
        public int Iteration { get; set; }
        public float Cost { get; set; }
        public NodeState State { get; set; }
        public PixelNode Previous { get; set; }
    }
}
