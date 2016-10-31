namespace SimpleWars.Res
{
    using System;

    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Res.Interfaces;

    /// <summary>
    /// The asset 3 d.
    /// </summary>
    public class Asset3D : Asset, IAsset3D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Asset3D"/> class.
        /// </summary>
        /// <param name="dir">
        /// The dir.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public Asset3D(string dir, string name)
            : base(name)
        {
            this.LoadAsset(dir, name);
        }

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
            if (this.Model != null)
            {
                throw new InvalidOperationException("You are trying to load model over existing one!");
            }

            this.Model = this.Content.Load<Model>(dir + "/" + name);
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        public Model Model { get; private set; }
    }
}