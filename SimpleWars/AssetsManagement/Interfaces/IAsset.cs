namespace SimpleWars.AssetsManagement.Interfaces
{
    using Microsoft.Xna.Framework.Content;

    /// <summary>
    /// The Asset interface.
    /// </summary>
    public interface IAsset
    {
        /// <summary>
        /// Gets the content.
        /// </summary>
        ContentManager Content { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The load content.
        /// </summary>
        /// <param name="dir">
        /// The dir.
        /// </param>
        /// <param name="asset">
        /// The asset.
        /// </param>
        void LoadContentManager();

        /// <summary>
        /// The load content.
        /// </summary>
        /// <param name="dir">
        /// The dir.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        void LoadAsset(string dir, string name);

        /// <summary>
        /// The unload content.
        /// </summary>
        void UnloadContent();
    }
}