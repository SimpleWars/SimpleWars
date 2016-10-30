namespace SimpleWars.Res.Interfaces
{
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Res.Interfaces;

    public interface IAsset2D : IAsset
    {
        /// <summary>
        /// Gets the texture.
        /// </summary>
        Texture2D Texture { get; }

        /// <summary>
        /// The load content.
        /// </summary>
        /// <param name="dir">
        /// The dir.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        void LoadContent(string dir, string name);
    }
}