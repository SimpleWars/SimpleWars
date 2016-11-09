namespace SimpleWars.AssetsManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.AssetsManagement.Interfaces;

    public abstract class AssetsManager
    {
        private readonly Type assetType;

        /// <summary>
        /// The assets.
        /// </summary>
        protected readonly IDictionary<string, IDictionary<string, IAsset>> AssetsDirDict;

        protected readonly ISet<IAsset> AssetsSet;
        protected AssetsManager(Type assetType)
        {
            this.AssetsDirDict = new Dictionary<string, IDictionary<string, IAsset>>();
            this.AssetsSet = new HashSet<IAsset>();
            this.assetType = assetType;
        }

        protected virtual void LoadAsset(string dir, string name)
        {
            if (this.AssetsDirDict.ContainsKey(dir) && this.AssetsDirDict[dir].ContainsKey(name))
            {
                return;
            }

            Activator.CreateInstance(this.assetType, dir, name);
        }

        protected virtual IAsset GetAsset(string dir, string name)
        {
            this.LoadAsset(dir, name);

            return this.AssetsDirDict[dir][name];
        }

        protected virtual bool ContainsAsset(string dir, string name)
        {
            return this.AssetsDirDict.ContainsKey(dir) && this.AssetsDirDict[dir].ContainsKey(name);
        }

        protected virtual void InsertAsset(string dir, string name, IAsset asset)
        {
            if (this.AssetsDirDict.ContainsKey(dir) && this.AssetsDirDict[dir].ContainsKey(name))
            {
                return;
            }

            if (asset == null)
            {
                throw new InvalidOperationException("Asset cannot be null!");
            }

            this.AssetsSet.Add(asset);

            if (!this.AssetsDirDict.ContainsKey(dir))
            {
                this.AssetsDirDict.Add(dir, new Dictionary<string, IAsset>());
            }

            this.AssetsDirDict[dir].Add(name, asset);
        }

        public virtual void DisposeAsset(string dir, string name)
        {
            if (!this.AssetsDirDict.ContainsKey(dir))
            {
                throw new InvalidOperationException($"Directory {dir} has not been loaded or is invalid!");
            }
            if (!this.AssetsDirDict[dir].ContainsKey(name))
            {
                throw new InvalidOperationException($"File {name} in directory {dir} has not been loaded or is invalid!");
            }

            IAsset asset = this.AssetsDirDict[dir][name];

            this.AssetsSet.Remove(asset);
            this.AssetsDirDict[dir].Remove(asset.Name);

            asset.UnloadContent();
        }

        public virtual void DisposeAll()
        {
            foreach (var asset in this.AssetsSet)
            {
                asset.UnloadContent();
            }

            this.AssetsSet.Clear();
            this.AssetsDirDict.Clear();
        }
    }
}