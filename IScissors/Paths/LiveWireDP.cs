using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using IScissors.Extensions;
using Microsoft.Xna.Framework.Graphics;
using Priority_Queue;
using Color=Microsoft.Xna.Framework.Color;

namespace IScissors.Paths
{
    public class LiveWireDP : IPathFinder
    {
        public Texture2D CostTexture { get; private set; }
        private int iteration;

        private readonly BasicImage originalImage;
        private readonly BasicImage costImage;
        private readonly PixelNode[,] pixelNodes;

        private int seedX;
        private int seedY;

        private float maxDerivative;
        private float maxEdgeCost;

        public LiveWireDP(BasicImage image)
        {
            this.originalImage = image;

            //Generate the costs for the pixels
            pixelNodes = new PixelNode[image.Width,image.Height];
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    pixelNodes[i,j] = new PixelNode()
                    {
                        X = i,
                        Y = j
                    };
                    LoadDerivatives(i, j);
                }
            }

            LoadCosts();
            //Here we are building the cost image
            var costColors = new Color[originalImage.Width*3,originalImage.Height*3];
            for (var i = 0; i < originalImage.Width; i++)
                for (var j = 0; j < originalImage.Height; ++j)
                {
                    var x = i*3;
                    var y = j*3;
                    for (var k = 0; k < 3; ++k)
                        for (var l = 0; l < 3; ++l)
                        {
                            costColors[x + k, y + k] = ColorExtensions.FromIntensity(pixelNodes[i, j].LinkCosts[k + 3*l]/255);
                        }
                    costColors[x + 1, y + 1] = originalImage.Colors[i, j];
                }
            costImage = new BasicImage(costColors);
            CostTexture = costImage.ToTexture();
        }

        private void LoadCosts()
        {
            var lengths = new float[9];
            for(var i = -1; i <= 1; ++i)
                for (var j = -1; j <= 1; ++j)
                    lengths[(i + 1) + (j+1)*3] = (float)Math.Sqrt(i*i + j*j);

            for (var x = 0; x < originalImage.Width; ++x)
                for (var y = 0; y < originalImage.Height; ++y)
                {
                    var pixelNode = pixelNodes[x, y];

                    for (var k = 0; k < pixelNode.LinkDerivates.Length; ++k)
                        pixelNode.LinkCosts[k] = (maxDerivative - pixelNode.LinkDerivates[k])*lengths[k];
                }
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
        private void LoadDerivatives(int x, int y)
        {
            var costs = new float[9];
            var start = new Point(x, y);

            costs[0] = DiagonalDerivative(start, new Point(start.X -1, start.Y));
            costs[1] = VerticalDerivative(start, new Point(start.X, start.Y-1));
            costs[2] = DiagonalDerivative(start,  new Point(start.X + 1, start.Y-1));
            costs[3] = HorizontalDerivative(start, new Point(start.X -1, start.Y));
            costs[4] = 0;
            costs[5] = HorizontalDerivative(start, new Point(start.X +1, start.Y));
            costs[6] = DiagonalDerivative(start, new Point(start.X -1, start.Y+1));
            costs[7] = VerticalDerivative(start, new Point(start.X, start.Y+1));
            costs[8] = DiagonalDerivative(start, new Point(start.X+1,start.Y));
            //for (var i = -1; i <= 1; ++i)
            //    for (var j = -1; j <= 1; ++j)
            //    {
            //        if (!OnImage(x + i, y + j)) continue;

            //        var c1 = originalImage.Colors[x, y];
            //        var c2 = originalImage.Colors[x + i, y + j];
            //        var dr = c1.R - c2.R;
            //        var dg = c1.G - c2.G;
            //        var db = c1.B - c2.B;

            //        costs[(i + 1) + 3*(j + 1)] = (float)Math.Sqrt(dr*dr + dg*dg + db*db);
            //    }

            foreach (var cost in costs)
            {
                if (cost > maxDerivative)
                    maxDerivative = cost;
                if (cost != 0)
                {
                    int foo = 8;
                }
            }

            pixelNodes[x, y].LinkDerivates = costs;
        }

        private bool NearSameAndNotEdge(int x, int y)
        {
            var color = originalImage.Colors[x, y];
            for (var i = -1; i <=1;++i)
                for (var j = -1; j <= 1; ++j)
                {
                    if (!OnImage(x + i, y + j)) return false;
                    if (color != originalImage.Colors[x + i, y + j]) return false;
                }
            return true;
        }

        private float HorizontalDerivative(Point start, Point end)
        {
            if (!OnImage(new Point(end.X, start.Y + 1)) || !OnImage(new Point(end.X, start.Y - 1))) return 0;

            var color1 = originalImage.Colors[start.X, start.Y - 1].ToByteArray();
            var color2 = originalImage.Colors[end.X, start.Y - 1].ToByteArray();
            var color3 = originalImage.Colors[start.X, start.Y + 1].ToByteArray();
            var color4 = originalImage.Colors[end.X, start.Y + 1].ToByteArray();

            var dd = 0f;
            var channelDerivatives = new int[3];
            for (var i = 0; i < channelDerivatives.Length; ++i)
            {
                channelDerivatives[i] = color1[i] + color2[i] - color3[i] - color4[i];
                dd += channelDerivatives[i]*channelDerivatives[i];
            }

            dd = (float)Math.Sqrt(dd/48.0);
            return dd;
        }

        private float VerticalDerivative(Point start, Point end)
        {
            if (!OnImage(new Point(start.X - 1, end.Y)) || !OnImage(new Point(start.X + 1, end.Y))) return 0;

            var color1 = originalImage.Colors[start.X-1, start.Y].ToByteArray();
            var color2 = originalImage.Colors[end.X-1, start.Y].ToByteArray();
            var color3 = originalImage.Colors[start.X+1, start.Y].ToByteArray();
            var color4 = originalImage.Colors[end.X+1, start.Y].ToByteArray();

            var dd = 0f;
            var channelDerivatives = new int[3];
            for (var i = 0; i < channelDerivatives.Length; ++i)
            {
                channelDerivatives[i] = color1[i] + color2[i] - color3[i] - color4[i];
                dd += channelDerivatives[i] * channelDerivatives[i];
            }

            dd = (float)Math.Sqrt(dd / 48.0);
            return dd;
        }

        private float DiagonalDerivative(Point start, Point end)
        {
            if (!OnImage(end)) return 0;

            var color1 = originalImage.Colors[end.X, start.Y];
            var color2 = originalImage.Colors[start.X, end.Y];

            var dr = color1.R - color2.R;
            var dg = color1.G - color2.G;
            var db = color1.B - color2.B;

            return (float)Math.Sqrt((dr*dr + dg*dg + db*db) / 6.0);
        }

        private bool OnImage(int x, int y)
        {
            return x >= 0 && y >= 0 && x < originalImage.Width && y < originalImage.Height;
        }

        private bool OnImage(Point p)
        {
            return OnImage(p.X, p.Y);
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

        public void SetSeed(int seedX, int seedY)
        {
            if (!OnImage(seedX, seedY)) return;

            this.seedX = seedX;
            this.seedY = seedY;

            var priorityQueue = new FastPriorityQueue<PixelNode>(originalImage.Width * originalImage.Height);

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
                        var cost = current.Cost + current.LinkCosts[(i + 1) + 3 * (j + 1)];

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
        }

        public LinkedList<Point> FindPath(int endX, int endY)
        {
            //If the point isn't on the image we shouldn't be returning anything
            if(!OnImage(endX, endY)) return new LinkedList<Point>();
            var path = new LinkedList<Point>();

            var c = pixelNodes[endX, endY];
            pixelNodes[seedX, seedY].Previous = null;
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
