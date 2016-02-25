using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IScissors.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Priority_Queue;

namespace IScissors.Paths
{
    public class LiveWireDP
    {
        public Texture2D CostTexture { get; private set; }
        private int iteration;

        private readonly BasicImage originalImage;
        private readonly BasicImage costImage;
        private readonly PixelNode[,] pixelNodes;

        private float maxDerivative;
        private float maxEdgeCost;

        public LiveWireDP(BasicImage image)
        {
            this.originalImage = image;

            pixelNodes = new PixelNode[image.Width,image.Height];
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    pixelNodes[i,j] = new PixelNode()
                    {
                        X = i,
                        Y = j,
                        LinkDerivates = LoadCosts(i, j)
                    };
                }
            }
            
            for (var x = 0; x < image.Width; ++x)
                for (var y = 0; y < image.Height; ++y)
                {
                    //TODO initialize weights on the pixel node.
                    var pixelNode = pixelNodes[x, y];
                    for (var i = -1; i <= 1; i++)
                    {
                        for (var j = -1; j <= 1; j++)
                        {
                            var length = (float)Math.Sqrt(i*i + j*j);
                            var cost = (maxDerivative - pixelNode.LinkDerivates[(i + 1) + 3*(j + 1)])*length;
                            if (cost > maxEdgeCost)
                                maxEdgeCost = cost;

                            pixelNode.LinkCosts = new float[9];
                            pixelNode.LinkCosts[(i + 1) + 3*(j + 1)] = cost;
                        }
                    }

                    //TODO set the color on the gradient image (maybe..?)
                }

            var costColors = new Color[originalImage.Width*3,originalImage.Height*3];
            for (var i = 0; i < originalImage.Width; i++)
                for (var j = 0; j < originalImage.Height; ++j)
                {
                    var x = i*3;
                    var y = j*3;
                    for (var k = 0; k < 3; ++k)
                        for (var l = 0; l < 3; ++l)
                        {
                            costColors[x + k, y + k] = ColorExtensions.FromIntensity(pixelNodes[i, j].LinkCosts[k + 3*l]/maxEdgeCost);
                        }
                    costColors[x + 1, y + 1] = originalImage.Colors[i, j];
                }
            costImage = new BasicImage(costColors);
            CostTexture = costImage.ToTexture();
        }

        /// <summary>
        /// Costs correspond to the following
        /// |0|1|2|
        /// |3| |5|
        /// |6|7|8|
        /// with invalid links being -1
        /// </summary>
        /// <param name="x">The x position of the pixel</param>
        /// <param name="y">The y position of the pixel</param>
        /// <returns>The costs for getting to the respective nodes</returns>
        private float[] LoadCosts(int x, int y)
        {
            var sqrt2 = (float)Math.Sqrt(2f);
            var costs = new float[9];

            //TODO check the pixel is valid

            //Cost to top left node
            if (x >= 1 && y >= 1)
                costs[0] = Math.Abs(Intensity(x - 1, y) - Intensity(x, y - 1))/sqrt2;

            //Cost to top node
            if (x >= 1 && x + 1 < originalImage.Width && y >= 1)
            costs[1] =
                Math.Abs((Intensity(x - 1, y) + Intensity(x - 1, y - 1))/2f -
                         (Intensity(x + 1, y) + Intensity(x + 1, y - 1))/2f)/2f;

            //Cost to top right node
            if (x + 1 < originalImage.Width && y >= 1)
                costs[2] = Math.Abs(Intensity(x+1, y) - Intensity(x, y - 1))/sqrt2;

            //Cost to left node
            if (x >= 1 && y >= 1 && y + 1 < originalImage.Height)
                costs[3] =
                Math.Abs((Intensity(x - 1, y - 1) + Intensity(x, y - 1))/2f -
                         (Intensity(x - 1, y + 1) + Intensity(x, y + 1))/2f)/2;

            //Ourself
            costs[4] = 0;

            //Cost to right node
            if (x + 1 < originalImage.Width && y >= 1 && y + 1 < originalImage.Height)
                costs[5] =
                Math.Abs((Intensity(x, y - 1) + Intensity(x + 1, y - 1))/2f -
                         (Intensity(x, y + 1) + Intensity(x + 1, y + 1))/2f)/2;

            //Cost to bottom left node
            if (x >= 1 && y + 1 < originalImage.Height)
                costs[6] = Math.Abs(Intensity(x - 1, y) - Intensity(x, y + 1)) / sqrt2;

            //Cost to bottom node
            if (x >= 1 && x + 1 < originalImage.Width && y + 1 < originalImage.Height)
                costs[7] =
                Math.Abs((Intensity(x - 1, y) + Intensity(x - 1, y + 1))/2f -
                         (Intensity(x + 1, y) + Intensity(x + 1, y + 1))/2f)/2f;

            //Cost to bottom right node
            if (x + 1 < originalImage.Width && y + 1 < originalImage.Height)
                costs[8] = Math.Abs(Intensity(x + 1, y) - Intensity(x, y + 1)) / sqrt2;

            foreach (var cost in costs)
                if (cost > maxDerivative)
                    maxDerivative = cost;

            return costs;
        }

        private bool OnImage(int x, int y)
        {
            return x >= 0 && y >= 0 && x < originalImage.Width && y < originalImage.Height;
        }

        private byte R(int x, int y)
        {
            return originalImage.Colors[x, y].R;
        }

        private byte G(int x, int y)
        {
            return originalImage.Colors[x, y].G;
        }

        private byte B(int x, int y)
        {
            return originalImage.Colors[x, y].B;
        }

        private byte Intensity(int x, int y)
        {
            return (byte)originalImage.Colors[x, y].Intensity();
        }

        public LinkedList<Point> LiveWire(int seedX, int seedY, int endX, int endY)
        {
            var priorityQueue = new SimplePriorityQueue<PixelNode>();

            var seed = pixelNodes[seedX, seedY];
            seed.Previous = null;
            priorityQueue.Enqueue(seed, 0);

            do
            {
                var current = priorityQueue.Dequeue();
                current.State = NodeState.Expanded;

                //Examine the node's neighbors
                for (var i = -1; i <= 1; i++)
                {
                    for (var j = -1; j <= 1; j++)
                    {
                        var nX = current.X + i;
                        var nY = current.Y + j;
                        if (!OnImage(nX, nY)) continue;

                        var neighbor = pixelNodes[nX, nY];

                        if (neighbor.Iteration == iteration && neighbor.State == NodeState.Expanded) continue;

                        //Calculate the cost of reaching the node this way
                        var cost = current.Cost + current.LinkDerivates[(i + 1) + 3*(j + 1)];

                        //If we haven't looked at the node this iteration
                        if (neighbor.Iteration != iteration || neighbor.State == NodeState.Initial)
                        {
                            neighbor.Previous = current;
                            neighbor.Cost = cost;
                            neighbor.Iteration = iteration;
                            neighbor.State = NodeState.Active;
                            priorityQueue.Enqueue(neighbor, neighbor.Cost);
                        }
                        //If we have found a shorter path to the node, use that one
                        else if (neighbor.State == NodeState.Active && cost < neighbor.Cost)
                        {
                            neighbor.Previous = current;
                            neighbor.Cost = cost;
                            priorityQueue.UpdatePriority(neighbor, neighbor.Cost);
                        }
                    }
                }

            } while (priorityQueue.Count > 0);
            
            var path = new LinkedList<Point>();

            var c = pixelNodes[endX, endY];
            do
            {
                path.AddFirst(new Point(c.X, c.Y));
                c = c.Previous;
            } while (c != null);

            
            iteration++;
            return path;
        }
    }
}
