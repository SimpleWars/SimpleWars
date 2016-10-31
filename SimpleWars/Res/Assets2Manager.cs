namespace SimpleWars.Res
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SimpleWars.Res.Interfaces;

    public class Assets2Manager
    {
        private static Assets2Manager instance;

        /// <summary>
        /// The 2D Assets.
        /// </summary>
        private readonly IDictionary<string, IAsset2D> assets2Dict;

        private readonly IDictionary<string, IList<IAsset2D>> assetsInDirs2Dict; 

        private readonly ISet<IAsset2D> assets2D;

        private Assets2Manager()
        {
            this.assetsInDirs2Dict = new Dictionary<string, IList<IAsset2D>>();
            this.assets2Dict = new Dictionary<string, IAsset2D>();
            this.assets2D = new HashSet<IAsset2D>();
            // Sprite sheets and other assets will be declared below when needed
        }

        public static Assets2Manager Instance => instance ?? (instance = new Assets2Manager());

        public void Load2DAsset(string dir, string name)
        {
            IAsset2D asset = new Asset2D(dir, name);

            if (this.assets2D.Contains(asset) || this.assets2Dict.ContainsKey(name))
            {
                throw new InvalidOperationException("You are trying to load the same asset 2 times!");
            }

            this.assets2D.Add(asset);
            this.assets2Dict.Add(name, asset);

            if (!this.assetsInDirs2Dict.ContainsKey(dir))
            {
                this.assetsInDirs2Dict.Add(dir, new List<IAsset2D>());
            }

            this.assetsInDirs2Dict[dir].Add(asset);
        }

        public IEnumerable<IAsset2D> GetAll2DAssets()
        {
            return this.assets2D;
        }

        public IEnumerable<IAsset2D> GetAll2DAssetsInDir(string dir)
        {
            return this.assetsInDirs2Dict.ContainsKey(dir) ? this.assetsInDirs2Dict[dir] : null;
        }

        public IAsset2D Get2DAsset(string name)
        {
            return this.assets2Dict.ContainsKey(name) ? this.assets2Dict[name] : null;
        }

        public void Dispose2DAsset(string dir, string name)
        {
            IAsset2D asset = this.Get2DAsset(name);

            if (!this.assets2D.Contains(asset) || !this.assets2Dict.ContainsKey(name))
            {
                throw new InvalidOperationException("You are trying to dispose of asset that does not exist!");
            }
            if (!this.assetsInDirs2Dict.ContainsKey(dir))
            {
                throw new InvalidOperationException("The specified directory is invalid!");
            }

            this.assets2D.Remove(asset);
            this.assets2Dict.Remove(name);
            this.assetsInDirs2Dict[dir].Remove(asset);

            asset.UnloadContent();
        }

        public void DisposeAll()
        {
            foreach (var asset in this.assets2D)
            {
                asset.UnloadContent();
            }

            foreach (var name in this.assets2Dict.Keys)
            {
                this.assets2Dict[name].UnloadContent();
            }

            foreach (var asset in this.assetsInDirs2Dict.Keys.SelectMany(dir => this.assetsInDirs2Dict[dir]))
            {
                asset.UnloadContent();
            }
        }
    }
}