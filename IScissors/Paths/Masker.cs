using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Color=Microsoft.Xna.Framework.Color;

namespace IScissors.Paths
{
    public class Masker
    {
        /// <summary>
        /// This is my own masking algorithm. It finds the lowest point on the contour and assumes that that some point above that
        /// must be on the image. It then proceeds to fill the area surrounded by the (presumably closed) contour using a depth first
        /// search. This will not work very well for complex polygons (it will only fill one of the areas formed by the self intersecting
        /// polygon).
        /// </summary>
        /// <param name="input">The source image</param>
        /// <param name="contour">The contour to fill</param>
        /// <returns>The masked image</returns>
        public static BasicImage GetMask(BasicImage input, IEnumerable<Point> contour)
        {
            var colors = new Color[input.Width, input.Height];

            var max = new Point(0);
            //Find the maximum point. The point above this one should be inside the polygon (right..?)
            foreach (var point in contour)
            {
                if (point.Y > max.Y) max = point;
                colors[point.X, point.Y] = Color.Black;
            }

            //Find the first point above the max point that is not part of the contour
            while (max.Y >= 0 && colors[max.X, max.Y] != Color.Transparent)
                max.Y--;
            
            if (max.Y != -1) //If nothing went wrong, fill the image
                Fill(input.Colors, colors, max);

            //Remove the contour
            foreach (var point in contour)
                colors[point.X, point.Y] = input.Colors[point.X, point.Y];

            return new BasicImage(colors);
        }

        private static void Fill(Color[,] original, Color[,] mask, Point start)
        {
            var visited = new HashSet<Point>();
            var frontier = new Stack<Point>();
            frontier.Push(start);
            
            do
            {
                var current = frontier.Pop();

                //If we've already seen this point, skip it
                if (visited.Contains(current)) continue;

                visited.Add(current);

                mask[current.X, current.Y] = original[current.X, current.Y];
                
                for (var i = -1; i <= 1; ++i)
                    for (var j = -1; j <= 1; ++j)
                    {
                        //No diagonals
                        if (i == j || i == -j) continue;

                        var x = i + current.X;
                        var y = j + current.Y;

                        //If the pixel isn't on the image, continue
                        if (x < 0 || y < 0 || x >= original.GetLength(0) || y >= original.GetLength(1)) continue;

                        //If the pixel has been filled, ignore it
                        if (mask[x, y] != Color.Transparent) continue;

                        //Add the pixel to the frontier
                        frontier.Push(new Point(x, y));
                    }
            } while (frontier.Count > 0);
        }
    }
}
