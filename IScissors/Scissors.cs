using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IScissors.Paths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color =Microsoft.Xna.Framework.Color;
using Point = System.Drawing.Point;

namespace IScissors
{
    public class Scissors
    {
        public Color ConfirmedPathColor = Color.Red;
        public Color UnconfirmedPathColor = Color.Orange;
        public Color SeedPointColor = Color.Green;

        private Texture2D point = TextureUtil.CreateTexture(1, 1, Color.White);
         
        //The seed points the user has picked
        private LinkedList<Point> seedPoints = new LinkedList<Point>();

        //The path the user has confirmed so far.
        private LinkedList<LinkedList<Point>> solidPath = new LinkedList<LinkedList<Point>>();

        public LinkedList<Point> Contour
        {
            get
            {
                var result = new LinkedList<Point>();
                foreach (var segment in solidPath)
                    foreach (var point in segment)
                        result.AddLast(point);

                return result;
            }
        } 
        //The path from the last seed point to the users mouse.
        private LinkedList<Point> unconfirmedPath = new LinkedList<Point>();

        private BasicImage originalImage;
        private BasicImage gradientImage;
        private BasicImage costImage;

        private Texture2D originalTexture;
        private Texture2D gradientTexture;
        private Texture2D costTexture;

        private LiveWireDP pathFinder;

        private Point mousePos = new Point(0,0);
        private Point lastMousePos = new Point(0,0);
        private bool updated;

        //Indicates whether the path to the mouse point should be drawn
        public bool Active { get; private set; }
        public bool Closed { get; private set; }

        /// <summary>
        /// The first point on the contour
        /// </summary>
        public Vector2? FirstPoint
        {
            get
            {
                var p = seedPoints.First?.Value;
                return p.HasValue ? new Vector2(p.Value.X, p.Value.Y) : new Vector2?();
            }
        }

        public Action OnPlace { get; set; }

        public Scissors()
        {
        }

        public BasicImage Mask()
        {
            return Masker.GetMask(originalImage, Contour);
        }

        public BasicImage ImageWithContour()
        {
            var image = originalImage.Duplicate();
            foreach (var point in Contour)
                image.Colors[point.X, point.Y] = ConfirmedPathColor;
            return image;
        }

        public void Load(Texture2D texture)
        {
            Clear();

            originalTexture = texture;
            originalImage = BasicImage.FromTexture(texture);

            pathFinder = new LiveWireDP(originalImage);
        }

        public bool CanClose()
        {
            return !Closed && seedPoints.Count >= 3;
        }

        public bool CanStart()
        {
            return !Closed && !Active;
        }

        public bool CanStop()
        {
            return !Closed && Active;
        }

        public void Clear()
        {
            Closed = false;
            seedPoints.Clear();
            solidPath.Clear();
            unconfirmedPath.Clear();
            Active = false;

            OnPlace?.Invoke();
        }

        public void AddSeed(int x, int y)
        {
            if (originalTexture == null) return;

            var seed = new Point(x,y);
            
            if (seedPoints.Count > 0)
            {
                //TODO add the path between the last seed and this one
                var previous = seedPoints.Last.Value;
                var path = pathFinder.FindPath(seed.X, seed.Y);
                solidPath.AddLast(path);
            }

            seedPoints.AddLast(seed);
            pathFinder.SetSeed(x, y);

            Active = true;

            OnPlace?.Invoke();
        }

        public bool CanUndo()
        {
            return seedPoints.Count > 0;
        }

        public void Undo()
        {
            if (seedPoints.Count == 0) return;

            var seed = seedPoints.Last.Value;
            var node = solidPath.Last;
            
            //TODO remove all the points added since the last seed
            if (solidPath.Count > 0)
                solidPath.RemoveLast();
            seedPoints.RemoveLast();

            seed = seedPoints.Count == 0 ? new Point(0) : seedPoints.Last.Value;

            if (seedPoints.Count == 0)
            {
                unconfirmedPath.Clear();
            }
            else
            {
                pathFinder.SetSeed(seed.X, seed.Y);
            }

            if (Closed)
            {
                Closed = false;
                Active = true;
                pathFinder.SetSeed(seedPoints.Last.Value.X, seedPoints.Last.Value.Y);
            }
        }

        public void Close()
        {
            if (seedPoints.Count < 3) return;

            var first = seedPoints.First.Value;
            AddSeed(first.X, first.Y);

            Active = false;
            Closed = true;

            unconfirmedPath.Clear();
        }

        public void SetMousePos(int x, int y)
        {
            mousePos = new Point(x, y);
            updated = true;
        }

        public void Update()
        {
            if (originalTexture == null || seedPoints.Count == 0 || !updated ||
                (lastMousePos.X == mousePos.X && lastMousePos.Y == mousePos.Y)
                || mousePos.X < 0 || mousePos.Y < 0 || mousePos.X >= originalImage.Width ||
                mousePos.Y > originalImage.Height || !Active) return;
            
            var start = seedPoints.Last.Value;
            unconfirmedPath = pathFinder.FindPath(mousePos.X, mousePos.Y);
            updated = false;
            lastMousePos = mousePos;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (originalTexture == null) return;

            spriteBatch.Draw(originalTexture, Vector2.Zero, Color.White);
            //spriteBatch.Draw(pathFinder.CostTexture, Vector2.Zero, Color.White);

            //Draw the confirmed path
            foreach(var segment in solidPath)
                foreach (var p in segment)
                spriteBatch.Draw(point, new Vector2(p.X, p.Y), ConfirmedPathColor);

            //Draw the unconfirmed path
            foreach (var p in unconfirmedPath)
                spriteBatch.Draw(point, new Vector2(p.X, p.Y), UnconfirmedPathColor);

            foreach (var p in seedPoints)
                spriteBatch.Draw(point, new Vector2(p.X, p.Y), null, SeedPointColor, MathHelper.PiOver4, new Vector2(0.5f), new Vector2(8), SpriteEffects.None, 0);
        }

        public void Resume()
        {
            if (Closed) return;

            Active = true;
        }

        public void Pause()
        {
            Active = false;
            unconfirmedPath.Clear();
        }

        public void SetImageMode(ImageMode imageMode)
        {
            throw new NotImplementedException();
            //TODO
        }
    }
}
