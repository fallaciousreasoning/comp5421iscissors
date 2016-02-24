using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IScissors.Extensions;
using Priority_Queue;

namespace IScissors.Paths
{
    public class LiveWireDP
    {
        private BasicImage originalImage;
        private PixelNode[,] pixelNodes;

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
                        LinkCosts = LoadCosts(i, j)
                    };
                }
            }
        }

        /// <summary>
        /// Costs correspond to the following
        /// |0|1|2|
        /// |3| |4|
        /// |5|6|7|
        /// with invalid links being -1
        /// </summary>
        /// <param name="x">The x position of the pixel</param>
        /// <param name="y">The y position of the pixel</param>
        /// <returns>The costs for getting to the respective nodes</returns>
        private float[] LoadCosts(int x, int y)
        {
            var sqrt2 = (float)Math.Sqrt(2f);
            var costs = new float[8];

            //TODO check the pixel is valid

            //Cost to top left node
            costs[0] = Math.Abs(Intensity(x - 1, y) - Intensity(x, y - 1))/sqrt2;

            //Cost to top node
            costs[1] =
                Math.Abs((Intensity(x - 1, y) + Intensity(x - 1, y - 1))/2f -
                         (Intensity(x + 1, y) + Intensity(x + 1, y - 1))/2f)/2f;

            //Cost to top right node
            costs[2] = Math.Abs(Intensity(x+1, y) - Intensity(x, y - 1))/sqrt2;

            //Cost to left node
            costs[3] =
                Math.Abs((Intensity(x - 1, y - 1) + Intensity(x, y - 1))/2f -
                         (Intensity(x - 1, y + 1) + Intensity(x, y + 1))/2f)/2;

            //Cost to right node
            costs[4] =
                Math.Abs((Intensity(x, y - 1) + Intensity(x + 1, y - 1))/2f -
                         (Intensity(x, y + 1) + Intensity(x + 1, y + 1))/2f)/2;

            //Cost to bottom left node
            costs[5] = Math.Abs(Intensity(x - 1, y) - Intensity(x, y + 1)) / sqrt2;

            //Cost to bottom node
            costs[6] =
                Math.Abs((Intensity(x - 1, y) + Intensity(x - 1, y + 1))/2f -
                         (Intensity(x + 1, y) + Intensity(x + 1, y + 1))/2f)/2f;

            //Cost to bottom right node
            costs[7] = Math.Abs(Intensity(x + 1, y) - Intensity(x, y + 1)) / sqrt2;

            return costs;
        }

        private LinkedList<PixelNode> GetNeighbors(int x, int y)
        {
            var result = new LinkedList<PixelNode>();

            for (int i = -1; i < 1; i++)
            {
                for (int j = -1; j < 1; j++)
                {
                    var nX = x + i;
                    var nY = y + j;
                    if (!OnImage(nX, nY)) continue;

                    result.AddLast(pixelNodes[nX, nY]);
                }
            }

            return result;
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

        public static LinkedList<PixelNode> LiveWire(int seedX, int seedY, int endX, int endY)
        {
            var priorityQueue = new SimplePriorityQueue<PixelNode>();
        }
    }
}
