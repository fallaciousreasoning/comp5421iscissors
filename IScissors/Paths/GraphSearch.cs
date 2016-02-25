using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IScissors.Filters;

namespace IScissors.Paths
{
    public class GraphSearch
    {
        private float[,] directions;
        private float[,] magnitudes;

        private BasicImage edgeImage;
        private BasicImage originalImage;
        public PixelNode FindPath(BasicImage image, int startX, int startY, int endX, int endY)
        {
            originalImage = image;

            var sobel = new Sobel2();
            edgeImage = sobel.Apply(image);

            directions = sobel.LastDirections;
            magnitudes = sobel.LastMagnitudes;

            return null;
        }

        /// <summary>
        /// Returns all the nodes bordering another node
        /// </summary>
        /// <param name="image"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private List<PixelNode> Neighbours(BasicImage image, PixelNode node)
        {
            var result = new List<PixelNode>();

            for (var i = -1; i < 1; ++i)
            {
                for (var j = -1; j < 1; ++j)
                {
                    if (i == 0 && j == 0) continue;

                    var x = node.X + i;
                    var y = node.Y + j;

                    if (x < 0 || y < 0 || x >= image.Width || y >= image.Height) continue;

                    var neighbor = new PixelNode();
                    neighbor.X = x;
                    neighbor.Y = y;

                    //TODO load weights
                    result.Add(neighbor);
                }
            }

            return result;
        }

        /// <summary>
        /// Weight indices are as follows
        /// |0|1|2|
        /// |3| |4|
        /// |5|6|7|
        /// </summary>
        /// <param name="node"></param>
        private void LoadNode(PixelNode node)
        {
            var index = 0;
            for (var i = -1; i < 1; ++i)
                for (var j = -1; j < 1; ++j)
                {
                    if (i == j && i == 0) continue;

                    var x = i + node.X;
                    var y = i + node.Y;

                    index ++;
                    if (x < 0 || y < 0 || x >= originalImage.Width || y >= originalImage.Height)
                    {
                        node.LinkDerivates[index - 1] = -1;
                        continue;
                    }

                    var direction = directions[x, y];
                    var magnitude = directions[x, y];
                }
        }

        private float GetWeight(int pX, int pY, int qX, int qY)
        {
            //TODO Work out what this actually should be
            return 0;
        }
    }
}
