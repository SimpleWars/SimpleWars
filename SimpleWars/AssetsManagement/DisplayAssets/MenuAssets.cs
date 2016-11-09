namespace SimpleWars.AssetsManagement.DisplayAssets
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.AssetsManagement.Interfaces;
    using SimpleWars.DisplayManagement;

    /// <summary>
    /// The menu assets.
    /// </summary>
    public class MenuAssets : IAssetLoader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuAssets"/> class.
        /// </summary>
        public MenuAssets()
        {
            this.LoadAssets();

            this.MainFrame = new Rectangle(
                0,
                0,
                (int)DisplayManager.Instance.Dimensions.X,
                (int)DisplayManager.Instance.Dimensions.Y);
        }

        /// <summary>
        /// Gets the background.
        /// </summary>
        public Texture2D Background { get; private set; }

        /// <summary>
        /// Gets the main frame.
        /// </summary>
        public Rectangle MainFrame { get; private set; }

        /// <summary>
        /// The load assets.
        /// </summary>
        public void LoadAssets()
        {
            Assets2Manager.Instance.Load2DAsset("Menu", "menu-background");

            this.Background = Assets2Manager.Instance.Get2DAsset("menu-background").Texture;
        }

        /// <summary>
        /// The unload assets.
        /// </summary>
        public void UnloadAssets()
        {
            Assets2Manager.Instance.Dispose2DAsset("Menu", "menu-background");
        }
    }
}