namespace SimpleWars.AssetsManagement
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.AssetsManagement.Interfaces;

    public class TexturesManager : AssetsManager
    {
        private static TexturesManager instance;

        private TexturesManager()
            : base(typeof(Asset2D))
        {  
        }

        public static TexturesManager Instance => instance ?? (instance = new TexturesManager());

        public IEnumerable<Texture2D> GetAllTextures()
        {
            if (this.AssetsSet == null)
            {
                yield break;
            }

            foreach (var asset in this.AssetsSet)
            {
                yield return ((Asset2D)asset).Texture;
            }
        }

        public Texture2D GetTexture(string dir, string name)
        {
            this.LoadAsset(dir, name);

            return ((Asset2D)this.AssetsDirDict[dir][name]).Texture;
        }

        private class Asset2D : Asset, IAsset2D
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
            /// Loads asset if it has not been loaded yet
            /// or sets the texture to existing one if it has
            /// I recommend using the assets managers 
            /// and not creating new instances of assets in the code
            /// </summary>
            /// <param name="dir">
            /// The dir.
            /// </param>
            /// <param name="name">
            /// The name.
            /// </param>
            public override void LoadAsset(string dir, string name)
            {
                if (!TexturesManager.Instance.ContainsAsset(dir, name))
                {
                    this.Texture = this.Content.Load<Texture2D>(dir + "/" + name);
                    TexturesManager.Instance.InsertAsset(dir, name, this);
                }
                else
                {
                    this.Texture = TexturesManager.Instance.GetTexture(dir, name);
                }
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
}