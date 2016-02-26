using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IScissors.Paths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IScissors
{
    public class Scissors
    {
        private Texture2D point = TextureUtil.CreateTexture(1, 1, Color.White);
         
        //The seed points the user has picked
        private LinkedList<Point> seedPoints = new LinkedList<Point>();

        //The path the user has confirmed so far.
        private LinkedList<Point> solidPath = new LinkedList<Point>();
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

        public Scissors()
        {
            
        }

        public void Load(Texture2D texture)
        {
            Clear();

            originalTexture = texture;
            originalImage = BasicImage.FromTexture(texture);

            pathFinder = new LiveWireDP(originalImage);
        }

        public void Clear()
        {
            seedPoints.Clear();
            solidPath.Clear();
            unconfirmedPath.Clear();
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
                foreach (var node in path) solidPath.AddLast(node);
            }

            seedPoints.AddLast(seed);
            pathFinder.SetSeed(x, y);
        }

        public void Close()
        {
            var first = seedPoints.First.Value;
            AddSeed(first.X, first.Y);
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
                mousePos.Y > originalImage.Height) return;
            
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
            foreach(var p in solidPath)
                spriteBatch.Draw(point, new Vector2(p.X, p.Y), Color.Red);

            //Draw the unconfirmed path
            foreach (var p in unconfirmedPath)
                spriteBatch.Draw(point, new Vector2(p.X, p.Y), Color.Orange);

            foreach (var p in seedPoints)
                spriteBatch.Draw(point, new Vector2(p.X, p.Y), null, Color.Green, MathHelper.PiOver4, new Vector2(0.5f), new Vector2(8), SpriteEffects.None, 0);
        }
    }
}
