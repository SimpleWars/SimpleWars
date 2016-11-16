using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimpleWars
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using SimpleWars.AssetsManagement;
    using SimpleWars.AssetsManagement.Interfaces;
    using SimpleWars.DBContexts;
    using SimpleWars.DisplayManagement;
    using SimpleWars.DisplayManagement.DisplaysHolder;
    using SimpleWars.GameData.Entities;
    using SimpleWars.InputManagement;
    using SimpleWars.User;

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
        /// The context.
        /// </summary>
        private readonly GameContext context;

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
            this.context = new GameContext();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            DisplayManager.Instance.GraphicsManager = this.graphics;

            DisplayManager.Instance.ChangeDimensions(
                (int)DisplayManager.Instance.Dimensions.X, 
                (int)DisplayManager.Instance.Dimensions.Y);
            
            this.IsMouseVisible = true;

            base.Initialize();

            this.Window.Position = new Point(30, 30);
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

            DisplayManager.Instance.LoadContent(this.Content, this.context);

            //PlayerManager.CurrentPlayer = this.context.Players.Include(p => p.Resources).Include(p => p.ResourceProviders).Include(p => p.Units).SingleOrDefault(p => p.Id == 1);
            PlayerManager.CurrentPlayer = this.context.Players.Find(1);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            this.context.SaveChanges();

            TexturesManager.Instance.DisposeAll();
            ModelsManager.Instance.DisposeAll();

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

            Input.Update();

            if (Input.KeyPressed(Keys.R))
            {
                DisplayManager.Instance.ChangeDisplay(new Test3Display(), this.context);
            }

            if (Input.KeyPressed(Keys.T))
            {
                DisplayManager.Instance.ChangeDisplay(new MenuDisplay(), this.context);
            }

            DisplayManager.Instance.Update(gameTime, this.context);

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
            this.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            this.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;

            DisplayManager.Instance.Draw(this.spriteBatch);
            this.spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
