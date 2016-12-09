
namespace SimpleWars
{
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using SimpleWars.Assets;
    using SimpleWars.Comms;
    using SimpleWars.DisplayManagement;
    using SimpleWars.DisplayManagement.Displays;
    using SimpleWars.GUI.Interfaces;
    using SimpleWars.ModelDTOs;
    using SimpleWars.ModelDTOs.Enums;
    using SimpleWars.Utils;

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameInitializer : Game
    {
        /// <summary>
        /// The graphics.
        /// </summary>
        private readonly GraphicsDeviceManager graphics;

        private readonly Parser parser;

        /// <summary>
        /// The sprite batch.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameInitializer"/> class.
        /// </summary>
        public GameInitializer()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            Client.Socket.Connect(new IPEndPoint(Client.GetLocalIPAddress(), 3000));
            Client.Socket.Reader.ReadMessagesContinuously();
            this.parser = new Parser(Client.Socket);
            Task.Run(() => this.parser.StartParsing());

            ContentServiceProvider.ContentService = this.Content.ServiceProvider;
            ContentServiceProvider.RootDirectory = this.Content.RootDirectory;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.InitializePointTextures();

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

            DisplayManager.Instance.LoadContent(this.Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            Client.Socket.Writer.Send(new Message<byte>(Service.Logout, 1));

            TexturesManager.Instance.DisposeAll();
            ModelsManager.Instance.DisposeAll();

            DisplayManager.Instance.UnloadContent();
            Client.Socket.Dispose();
        
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

            Input.Input.Update(gameTime);

            if (Input.Input.KeyPressed(Keys.M))
            {
                DisplayManager.Instance.ChangeDisplay(new InitialDisplay());
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

            this.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            this.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;

            DisplayManager.Instance.Draw(this.spriteBatch);
            this.spriteBatch.End();
            
            base.Draw(gameTime);
        }

        private void InitializePointTextures()
        {
            PointTextures.WhitePoint = new Texture2D(this.graphics.GraphicsDevice, 1, 1);
            PointTextures.WhitePoint.SetData<Color>(new Color[] { Color.White });
            PointTextures.BlackPoint = new Texture2D(this.graphics.GraphicsDevice, 1, 1);
            PointTextures.BlackPoint.SetData<Color>(new Color[] { Color.Black });
            PointTextures.TransparentPoint = new Texture2D(this.graphics.GraphicsDevice, 1, 1);
            PointTextures.TransparentPoint.SetData<Color>(new Color[] { Color.Transparent });
            PointTextures.GrayPoint = new Texture2D(this.graphics.GraphicsDevice, 1, 1);
            PointTextures.GrayPoint.SetData<Color>(new Color[] { Color.Gray });
            PointTextures.TransparentBlackPoint = new Texture2D(this.graphics.GraphicsDevice, 1, 1);
            PointTextures.TransparentBlackPoint.SetData(new Color[] { new Color(Color.Black, 0.6f) });
            PointTextures.TransparentGrayPoint = new Texture2D(this.graphics.GraphicsDevice, 1, 1);
            PointTextures.TransparentGrayPoint.SetData(new Color[] { new Color(Color.Gray, 0.6f) });
            PointTextures.TransparentWhitePoint = new Texture2D(this.graphics.GraphicsDevice, 1, 1);
            PointTextures.TransparentWhitePoint.SetData(new Color[] { new Color(Color.White, 0.6f) });
            PointTextures.TransparentLightYellowPoint = new Texture2D(this.graphics.GraphicsDevice, 1, 1);
            PointTextures.TransparentLightYellowPoint.SetData(new Color[] { new Color(Color.LightYellow, 0.6f) });
        }
    }
}
