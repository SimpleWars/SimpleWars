namespace SimpleWars.AssetsManagement.Interfaces
{
    using Microsoft.Xna.Framework.Graphics;

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