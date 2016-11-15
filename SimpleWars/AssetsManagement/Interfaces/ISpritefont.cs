namespace SimpleWars.AssetsManagement.Interfaces
{
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The Spritefont interface.
    /// </summary>
    public interface ISpritefont : IAsset
    {
        /// <summary>
        /// Gets the sprite font.
        /// </summary>
        SpriteFont SpriteFont { get; }
    }
}