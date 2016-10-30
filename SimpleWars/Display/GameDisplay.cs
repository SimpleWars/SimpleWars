namespace SimpleWars.Display
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Display.Interfaces;

    /// <summary>
    /// The game display.
    /// </summary>
    public abstract class GameDisplay : IGameDisplay
    {
        /// <summary>
        /// Gets the content.
        /// </summary>
        protected ContentManager Content { get; private set; }

        /// <summary>
        /// The load content.
        /// </summary>
        public virtual void LoadContent()
        {
            this.Content =
                new ContentManager(
                DisplayManager.Instance.Content.ServiceProvider,
                DisplayManager.Instance.Content.RootDirectory);
        }

        /// <summary>
        /// The unload content.
        /// </summary>
        public virtual void UnloadContent()
        {
            this.Content.Unload();
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="spriteBatch">
        /// The sprite batch.
        /// </param>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}