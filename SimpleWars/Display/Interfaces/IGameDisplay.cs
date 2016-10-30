namespace SimpleWars.Display.Interfaces
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The GameDisplay interface.
    /// </summary>
    public interface IGameDisplay
    {
        /// <summary>
        /// The load content.
        /// </summary>
        void LoadContent();

        /// <summary>
        /// The unload content.
        /// </summary>
        void UnloadContent();

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game Time.
        /// </param>
        void Update(GameTime gameTime);

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="spriteBatch">
        /// The sprite Batch.
        /// </param>
        void Draw(SpriteBatch spriteBatch);
    }
}