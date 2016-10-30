namespace SimpleWars.Res.Interfaces
{
    using Microsoft.Xna.Framework.Content;

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
        /// The unload content.
        /// </summary>
        void UnloadContent();
    }
}