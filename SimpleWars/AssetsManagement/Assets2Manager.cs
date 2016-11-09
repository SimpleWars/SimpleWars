namespace SimpleWars.AssetsManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.AssetsManagement.Interfaces;

    public class Assets2Manager
    {
        private static Assets2Manager instance;

        /// <summary>
        /// The 2D Assets.
        /// </summary>
        private readonly IDictionary<string, IDictionary<string, IAsset2D>> assetsDirDict;

        private readonly ISet<IAsset2D> assetsSet;

        private Assets2Manager()
        {
            this.assetsDirDict = new Dictionary<string, IDictionary<string, IAsset2D>>();
            this.assetsSet = new HashSet<IAsset2D>();
        }

        public static Assets2Manager Instance => instance ?? (instance = new Assets2Manager());

        private void LoadAsset(string dir, string name)
        {
            if (this.assetsDirDict.ContainsKey(dir) && this.assetsDirDict[dir].ContainsKey(name))
            {
                return;
            }

            Activator.CreateInstance(typeof(Asset2D), dir, name);
        }

        public IEnumerable<Texture2D> GetAllTextures()
        {
            return this.assetsSet.Select(a => a.Texture);
        }

        public Texture2D GetTexture(string dir, string name)
        {
            this.LoadAsset(dir, name);

            return this.assetsDirDict[dir][name].Texture;
        }

        private IAsset2D GetAsset(string dir, string name)
        {
            this.LoadAsset(dir, name);

            return this.assetsDirDict[dir][name];
        }

        private bool ContainsAsset(string dir, string name)
        {
            return this.assetsDirDict.ContainsKey(dir) && this.assetsDirDict[dir].ContainsKey(name);
        }

        private void InsertAsset(string dir, string name, IAsset2D asset)
        {
            if (this.assetsDirDict.ContainsKey(dir) && this.assetsDirDict[dir].ContainsKey(name))
            {
                return;
            }

            if (asset == null)
            {
                throw new InvalidOperationException("Asset cannot be null!");
            }

            this.assetsSet.Add(asset);

            if (!this.assetsDirDict.ContainsKey(dir))
            {
                this.assetsDirDict.Add(dir, new Dictionary<string, IAsset2D>());
            }

            this.assetsDirDict[dir].Add(name, asset);
        }

        public void DisposeAsset(string dir, string name)
        {
            if (!this.assetsDirDict.ContainsKey(dir))
            {
                throw new InvalidOperationException($"Directory {dir} has not been loaded or is invalid!");
            }
            if (!this.assetsDirDict[dir].ContainsKey(name))
            {
                throw new InvalidOperationException($"File {name} in directory {dir} has not been loaded or is invalid!");
            }

            IAsset2D asset = this.GetAsset(dir, name);

            this.assetsSet.Remove(asset);
            this.assetsDirDict[dir].Remove(asset.Name);

            asset.UnloadContent();
        }

        public void DisposeAll()
        {
            foreach (var asset in this.assetsSet)
            {
                asset.UnloadContent();
            }

            this.assetsSet.Clear();
            this.assetsDirDict.Clear();

            GC.Collect();
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
                if (!Assets2Manager.Instance.ContainsAsset(dir, name))
                {
                    this.Texture = this.Content.Load<Texture2D>(dir + "/" + name);
                    Assets2Manager.Instance.InsertAsset(dir, name, this);
                }
                else
                {
                    this.Texture = Assets2Manager.Instance.GetAsset(dir, name).Texture;
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