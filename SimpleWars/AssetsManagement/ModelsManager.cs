namespace SimpleWars.AssetsManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.AssetsManagement.Interfaces;

    public class ModelsManager : AssetsManager
    {
        private static ModelsManager instance;

        private ModelsManager()
            : base(typeof(Asset3D))
        {
        }

        public static ModelsManager Instance => instance ?? (instance = new ModelsManager());

        public IEnumerable<Model> GetAllModels()
        {
            if (this.AssetsSet == null)
            {
                yield break;
            }

            foreach (var asset in this.AssetsSet)
            {
                yield return ((Asset3D)asset).Model;
            }
        }

        public Model GetModel(string dir, string name)
        {
            this.LoadAsset(dir, name);

            return ((Asset3D)this.AssetsDirDict[dir][name]).Model;
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
                if (!ModelsManager.Instance.ContainsAsset(dir, name))
                {
                    this.Model = this.Content.Load<Model>(dir + "/" + name);
                    ModelsManager.Instance.InsertAsset(dir, name, this);
                }
                else
                {
                    this.Model = ModelsManager.Instance.GetModel(dir, name);
                }
            }
        }
    }
}