namespace SimpleWars.DisplayManagement.Displays
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Assets;
    using SimpleWars.DBContexts;

    /// <summary>
    /// The menu display.
    /// </summary>
    public class MenuDisplay : Display
    {
        /// <summary>
        /// The main frame.
        /// </summary>
        private Rectangle mainFrame;

        private Texture2D background;

        /// <summary>
        /// The load content.
        /// </summary>
        public override void LoadContent(GameContext context)
        {
            this.mainFrame = new Rectangle(
                0, 
                0, 
                (int)DisplayManager.Instance.Dimensions.X,
                (int)DisplayManager.Instance.Dimensions.Y);

            this.background = TexturesManager.Instance.GetTexture("Menu", "background");
        }

        /// <summary>
        /// The unload content.
        /// </summary>
        public override void UnloadContent()
        {
            TexturesManager.Instance.DisposeAll();
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public override void Update(GameTime gameTime, GameContext context)
        {
        }

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="spriteBatch">
        /// The sprite batch.
        /// </param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.background, null, this.mainFrame);
        }
    }
}