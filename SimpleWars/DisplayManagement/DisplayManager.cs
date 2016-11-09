namespace SimpleWars.DisplayManagement
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.DisplayManagement.DisplaysHolder;
    using SimpleWars.DisplayManagement.Interfaces;

    /// <summary>
    /// The display manager.
    /// </summary>
    public class DisplayManager : IDisplayManager
    {
        /// <summary>
        /// The instance.
        /// </summary>
        private static DisplayManager instance;

        /// <summary>
        /// The dimensions.
        /// </summary>
        private Vector2 dimensions;

        private GraphicsDeviceManager graphicsManager;

        private DisplayManager()
        {
            this.Dimensions = new Vector2(1280, 720);

            this.CurrentDisplay = new MenuDisplay();
        }

        /// <summary>
        /// The instance.
        /// </summary>
        public static readonly DisplayManager Instance = instance ?? (instance = new DisplayManager());

        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// Gets or sets the graphics manager.
        /// </summary>
        public GraphicsDeviceManager GraphicsManager
        {
            get
            {
                return this.graphicsManager;
            }

            set
            {
                this.graphicsManager = value;
                this.GraphicsDevice = this.GraphicsManager.GraphicsDevice;
            }
        }

        /// <summary>
        /// Gets or sets the dimensions.
        /// </summary>
        public Vector2 Dimensions
        {
            get
            {
                return this.dimensions;
            }

            set
            {
                this.dimensions = value;

                this.GraphicsManager.PreferredBackBufferWidth = (int)this.Dimensions.X;
                this.GraphicsManager.PreferredBackBufferHeight = (int)this.Dimensions.Y;
                this.GraphicsManager.ApplyChanges();
            }
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        public ContentManager Content { get; private set; }

        /// <summary>
        /// Gets the current display.
        /// </summary>
        public IDisplay CurrentDisplay { get; private set; }

        /// <summary>
        /// The load content.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        public void LoadContent(ContentManager content)
        {
            this.Content = new ContentManager(content.ServiceProvider, content.RootDirectory);
            this.CurrentDisplay.LoadContent();
        }

        /// <summary>
        /// The unload content.
        /// </summary>
        public void UnloadContent()
        {
            this.Content.Unload();
            this.CurrentDisplay.UnloadContent();
        }

        public void ChangeDisplay(IDisplay display)
        {
            this.CurrentDisplay.UnloadContent();
            this.CurrentDisplay = display;
            this.CurrentDisplay.LoadContent();
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public void Update(GameTime gameTime)
        {
            this.CurrentDisplay.Update(gameTime);
        }

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="spriteBatch">
        /// The sprite batch.
        /// </param>
        public void Draw(SpriteBatch spriteBatch)
        {
            this.CurrentDisplay.Draw(spriteBatch);
        }
    }
}