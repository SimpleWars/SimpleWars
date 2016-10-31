namespace SimpleWars.Res
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SimpleWars.Res.Interfaces;

    public class Assets3Manager
    {
        private static Assets3Manager instance;

        /// <summary>
        /// The 2D Assets.
        /// </summary>
        private readonly IDictionary<string, IAsset3D> assets3Dict;

        private readonly IDictionary<string, IList<IAsset3D>> assetsInDirs3Dict;

        private readonly ISet<IAsset3D> assets3D;

        private Assets3Manager()
        {
            this.assetsInDirs3Dict = new Dictionary<string, IList<IAsset3D>>();
            this.assets3Dict = new Dictionary<string, IAsset3D>();
            this.assets3D = new HashSet<IAsset3D>();
            // Sprite sheets and other assets will be declared below when needed
        }

        public static Assets3Manager Instance => instance ?? (instance = new Assets3Manager());

        public void Load3DAsset(string dir, string name)
        {
            IAsset3D asset = new Asset3D(dir, name);

            if (this.assets3D.Contains(asset) || this.assets3Dict.ContainsKey(name))
            {
                throw new InvalidOperationException("You are trying to load the same asset 2 times!");
            }

            this.assets3D.Add(asset);
            this.assets3Dict.Add(name, asset);

            if (!this.assetsInDirs3Dict.ContainsKey(dir))
            {
                this.assetsInDirs3Dict.Add(dir, new List<IAsset3D>());
            }

            this.assetsInDirs3Dict[dir].Add(asset);
        }

        public IEnumerable<IAsset3D> GetAll3DAssets()
        {
            return this.assets3D;
        }

        public IEnumerable<IAsset3D> GetAll3DAssetsInDir(string dir)
        {
            return this.assetsInDirs3Dict.ContainsKey(dir) ? this.assetsInDirs3Dict[dir] : null;
        }

        public IAsset3D Get3DAsset(string name)
        {
            return this.assets3Dict.ContainsKey(name) ? this.assets3Dict[name] : null;
        }

        public void Dispose3DAsset(string dir, string name)
        {
            IAsset3D asset = this.Get3DAsset(name);

            if (!this.assets3D.Contains(asset) || !this.assets3Dict.ContainsKey(name))
            {
                throw new InvalidOperationException("You are trying to dispose of asset that does not exist!");
            }
            if (!this.assetsInDirs3Dict.ContainsKey(dir))
            {
                throw new InvalidOperationException("The specified directory is invalid!");
            }

            this.assets3D.Remove(asset);
            this.assets3Dict.Remove(name);
            this.assetsInDirs3Dict[dir].Remove(asset);

            asset.UnloadContent();
        }

        public void DisposeAll()
        {
            foreach (var asset in this.assets3D)
            {
                asset.UnloadContent();
            }

            foreach (var name in this.assets3Dict.Keys)
            {
                this.assets3Dict[name].UnloadContent();
            }

            foreach (var asset in this.assetsInDirs3Dict.Keys.SelectMany(dir => this.assetsInDirs3Dict[dir]))
            {
                asset.UnloadContent();
            }
        }
    }
}