namespace SimpleWars.Res.Interfaces
{
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Res.Interfaces;

    /// <summary>
    /// The Asset2D interface.
    /// </summary>
    public interface IAsset2D : IAsset
    {
        /// <summary>
        /// Gets the texture.
        /// </summary>
        Texture2D Texture { get; }
    }
}