namespace SimpleWars.Res
{
    using System;

    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Res.Interfaces;

    public class Asset2D : Asset, IAsset2D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Asset2D"/> class.
        /// </summary>
        /// <param name="dir">
        /// The dir.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public Asset2D(string dir, string name)
            : base(name)
        {
            this.LoadContent(dir, name);
        }

        /// <summary>
        /// Gets the texture.
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// The load content.
        /// </summary>
        /// <param name="dir">
        /// The dir.
        /// </param>
        /// <param name="asset">
        /// The asset.
        /// </param>
        public void LoadContent(string dir, string asset)
        {
            if (this.Texture != null)
            {
                throw new InvalidOperationException("You are trying to load texture over existing one!");
            }

            this.Texture = this.Content.Load<Texture2D>(dir + "/" + asset);
        }

        /// <summary>
        /// The unload content.
        /// </summary>
        public override void UnloadContent()
        {
            this.Texture.Dispose();

            base.UnloadContent();
        }
    }
}