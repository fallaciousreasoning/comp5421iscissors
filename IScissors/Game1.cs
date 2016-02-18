using System.Collections.Generic;
using System.IO;
using IScissors.Filters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IScissors
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private static GraphicsDeviceManager graphics;
        public static GraphicsDevice Device { get { return graphics.GraphicsDevice; } }

        SpriteBatch spriteBatch;

        private ImageScreen imageScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var calvinTexture = Texture2D.FromStream(Device, File.OpenRead("Content//calvin and hobbs.jpg"));
            var ferryTexture = Texture2D.FromStream(Device, File.OpenRead("Content//ferry.bmp"));
            var lenaTexture = Texture2D.FromStream(Device, File.OpenRead("Content//lena.jpg"));

            //var originalImage = new ImageScreen(lenaTexture, new List<IFilter>() {ColorFilter.GrayScale});
            imageScreen = new ImageScreen(ferryTexture, new List<IFilter>()
            {
                new GuassianBlur(2, 1.4f),
                new Sobel2(),
                //ColorFilter.GrayScale,
                ////BasicFilter.Blur,
                //BasicFilter.SobelHorizontal,
                //BasicFilter.SobelVertical,
                ////ColorFilter.GrayScale
                //BasicFilter.LaplacianOfTheGuassian,
                //new CannyFilter(5, 1.4f),
            });
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            imageScreen.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            imageScreen.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
