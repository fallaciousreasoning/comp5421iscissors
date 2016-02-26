﻿using System;
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
        public Color ConfirmedPathColor = Color.Red;
        public Color UnconfirmedPathColor = Color.Orange;
        public Color SeedPointColor = Color.Green;

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

        //Indicates whether the path to the mouse point should be drawn
        private bool active = false;

        public Scissors()
        {
            
        }

        public BasicImage Mask()
        {
            var colors = new Color[originalImage.Width, originalImage.Height];
            foreach (var point in solidPath)
                colors[point.X, point.Y] = ConfirmedPathColor;

            //TODO fill the shape made by the contour

            //Pretty much, any pixels that are colored by the filled contour should be made a part of the masked image
            for (var i = 0; i < originalImage.Width; ++i)
                for (var j = 0; j < originalImage.Height; ++j)
                {
                    if (colors[i, j] != Color.Transparent)
                        colors[i, j] = originalImage.Colors[i, j];
                }

            return new BasicImage(colors);
        }

        public BasicImage ImageWithContour()
        {
            var image = originalImage.Duplicate();
            foreach (var point in solidPath)
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

        public void Clear()
        {
            seedPoints.Clear();
            solidPath.Clear();
            unconfirmedPath.Clear();
            active = false;
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

            active = true;
        }

        public void Close()
        {
            var first = seedPoints.First.Value;
            AddSeed(first.X, first.Y);

            active = false;
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
                mousePos.Y > originalImage.Height || !active) return;
            
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
                spriteBatch.Draw(point, new Vector2(p.X, p.Y), ConfirmedPathColor);

            //Draw the unconfirmed path
            foreach (var p in unconfirmedPath)
                spriteBatch.Draw(point, new Vector2(p.X, p.Y), UnconfirmedPathColor);

            foreach (var p in seedPoints)
                spriteBatch.Draw(point, new Vector2(p.X, p.Y), null, SeedPointColor, MathHelper.PiOver4, new Vector2(0.5f), new Vector2(8), SpriteEffects.None, 0);
        }
    }
}