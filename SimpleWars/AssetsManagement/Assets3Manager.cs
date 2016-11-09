namespace SimpleWars.AssetsManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.AssetsManagement.Interfaces;

    public class Assets3Manager
    {
        private static Assets3Manager instance;

        /// <summary>
        /// The 3D Assets.
        /// </summary>
        private readonly IDictionary<string, IDictionary<string, IAsset3D>> assetsDirDict;

        private readonly ISet<IAsset3D> assetsSet;

        private Assets3Manager()
        {
            this.assetsDirDict = new Dictionary<string, IDictionary<string, IAsset3D>>();
            this.assetsSet = new HashSet<IAsset3D>();
        }

        public static Assets3Manager Instance => instance ?? (instance = new Assets3Manager());

        private void LoadAsset(string dir, string name)
        {
            if (this.assetsDirDict.ContainsKey(dir) && this.assetsDirDict[dir].ContainsKey(name))
            {
                return;
            }

            Activator.CreateInstance(typeof(Asset3D), dir, name);
        }

        public IEnumerable<Model> GetAllModels()
        {
            return this.assetsSet.Select(a => a.Model);
        }

        private IAsset3D GetAsset(string dir, string name)
        {
            this.LoadAsset(dir, name);

            return this.assetsDirDict[dir][name];
        }

        public Model GetModel(string dir, string name)
        {
            this.LoadAsset(dir, name);

            return this.assetsDirDict[dir][name].Model;
        }

        private bool ContainsAsset(string dir, string name)
        {
            return this.assetsDirDict.ContainsKey(dir) && this.assetsDirDict[dir].ContainsKey(name);
        }

        private void InsertAsset(string dir, string name, IAsset3D asset)
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
                this.assetsDirDict.Add(dir, new Dictionary<string, IAsset3D>());
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

            IAsset3D asset = this.GetAsset(dir, name);

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

        private class Asset3D : Asset, IAsset3D
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
            /// Gets the model.
            /// </summary>
            public Model Model { get; private set; }

            /// <summary>
            /// Loads asset if it has not been loaded yet
            /// or sets the model to existing one if it has
            /// </summary>
            /// <param name="dir">
            /// The dir.
            /// </param>
            /// <param name="name">
            /// The name.
            /// </param>
            public override void LoadAsset(string dir, string name)
            {
                if (!Assets3Manager.Instance.ContainsAsset(dir, name))
                {
                    this.Model = this.Content.Load<Model>(dir + "/" + name);
                    Assets3Manager.Instance.InsertAsset(dir, name, this);
                }
                else
                {
                    this.Model = Assets3Manager.Instance.GetAsset(dir, name).Model;
                }
            }
        }
    }
}