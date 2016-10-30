using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimpleWars
{
    using System.Diagnostics;

    using SimpleWars.Display;
    using SimpleWars.InputManager;

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Master : Game
    {
        /// <summary>
        /// The graphics.
        /// </summary>
        private readonly GraphicsDeviceManager graphics;

        /// <summary>
        /// The sprite batch.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="Master"/> class.
        /// </summary>
        public Master()
        {
            this.graphics = new GraphicsDeviceManager(this);
            var a = DisplayManager.Instance.Dimensions.X;
            this.Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine(DisplayManager.Instance.Dimensions.X);
            this.graphics.PreferredBackBufferWidth = (int)DisplayManager.Instance.Dimensions.X;
            this.graphics.PreferredBackBufferHeight = (int)DisplayManager.Instance.Dimensions.Y;
            this.graphics.ApplyChanges();
            this.IsMouseVisible = true;

            base.Initialize();     
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            DisplayManager.Instance.LoadContent(this.Content);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            DisplayManager.Instance.UnloadContent();

            this.Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            Input.Instance.Update();

            DisplayManager.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatch.Begin();
            DisplayManager.Instance.Draw(this.spriteBatch);
            this.spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
