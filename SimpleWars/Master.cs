using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimpleWars
{
    using System.Diagnostics;
    using System.Linq;

    using SimpleWars.Displays;
    using SimpleWars.Displays.DisplaysHolder;
    using SimpleWars.InputManager;
    using SimpleWars.Res;

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
            this.graphics.PreferredBackBufferWidth = (int)DisplayManager.Instance.Dimensions.X;
            this.graphics.PreferredBackBufferHeight = (int)DisplayManager.Instance.Dimensions.Y;
            this.graphics.ApplyChanges();

            this.IsMouseVisible = true;

            base.Initialize();

            DisplayManager.Instance.GraphicsDevice = this.GraphicsDevice;
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
            Assets2Manager.Instance.DisposeAll();
            Assets3Manager.Instance.DisposeAll();

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

            if (Input.Instance.KeyPressed(Keys.A))
            {
                DisplayManager.Instance.ChangeDisplay(new Test3Display());
            }

            if (Input.Instance.KeyPressed(Keys.S))
            {
                DisplayManager.Instance.ChangeDisplay(new MenuDisplay());
            }

            DisplayManager.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            this.spriteBatch.Begin();

            this.GraphicsDevice.BlendState = BlendState.Opaque;
            this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            DisplayManager.Instance.Draw(this.spriteBatch);
            this.spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
