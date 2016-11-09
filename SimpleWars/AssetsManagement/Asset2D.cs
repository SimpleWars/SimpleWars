namespace SimpleWars.AssetsManagement
{
    using System;

    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.AssetsManagement.Interfaces;

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
            this.LoadAsset(dir, name);
        }

        /// <summary>
        /// Gets the texture.
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// The load asset.
        /// </summary>
        /// <param name="dir">
        /// The dir.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        public override void LoadAsset(string dir, string name)
        {
            if (this.Texture != null)
            {
                throw new InvalidOperationException("You are trying to load texture over existing one!");
            }

            this.Texture = this.Content.Load<Texture2D>(dir + "/" + name);
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