namespace SimpleWars.DisplayManagement.Interfaces
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    /// <summary>
    /// The DisplayManager interface.
    /// </summary>
    public interface IDisplayManager
    {
        /// <summary>
        /// Gets the dimensions.
        /// </summary>
        Vector2 Dimensions { get; }

        /// <summary>
        /// Gets the current display.
        /// </summary>
        IDisplay CurrentDisplay { get; }

        /// <summary>
        /// The load content.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        void LoadContent(ContentManager content);

        /// <summary>
        /// The unload content.
        /// </summary>
        void UnloadContent();

        /// <summary>
        /// The change display.
        /// </summary>
        /// <param name="display">
        /// The display.
        /// </param>
        void ChangeDisplay(IDisplay display);

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        void Update(GameTime gameTime);

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="spriteBatch">
        /// The sprite batch.
        /// </param>
        void Draw(SpriteBatch spriteBatch);
    }
}