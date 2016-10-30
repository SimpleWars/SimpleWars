namespace SimpleWars.Display
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The menu display.
    /// </summary>
    public class MenuDisplay : GameDisplay
    {
        /// <summary>
        /// The background.
        /// </summary>
        private Texture2D background;

        /// <summary>
        /// The main frame.
        /// </summary>
        private Rectangle mainFrame;

        /// <summary>
        /// The load content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            this.background = this.Content.Load<Texture2D>("Menu/menu-background");

            this.mainFrame = new Rectangle(
                0,
                0,
                (int)DisplayManager.Instance.Dimensions.X,
                (int)DisplayManager.Instance.Dimensions.Y);
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
            spriteBatch.Draw(this.background, null, this.mainFrame);
        }
    }
}