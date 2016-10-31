namespace SimpleWars.Displays.DisplaysHolder
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Res.DisplayAssets;

    /// <summary>
    /// The menu display.
    /// </summary>
    public class MenuDisplay : Display
    {
        private MenuAssets assets;

        /// <summary>
        /// The load content.
        /// </summary>
        public override void LoadContent()
        {
            this.assets = new MenuAssets();
        }

        public override void UnloadContent()
        {
            this.assets.UnloadAssets();
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public override void Update(GameTime gameTime)
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
            spriteBatch.Draw(this.assets.Background, null, this.assets.MainFrame);
        }
    }
}