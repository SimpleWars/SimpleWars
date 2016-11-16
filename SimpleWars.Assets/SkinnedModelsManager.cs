namespace SimpleWars.Assets
{
    using System;

    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Assets.Interfaces;

    using SkinnedModel;

    public class SkinnedModelsManager : AssetsManager
    {
        private static SkinnedModelsManager instance;

        private SkinnedModelsManager()
            : base(typeof(Animation3D))
        {
        }

        public static SkinnedModelsManager Instance => instance ?? (instance = new SkinnedModelsManager());

        public AnimationPlayer CreateAnimation(string dir, string name)
        {
            this.LoadAsset(dir, name);

            return new AnimationPlayer(this.GetSkinningData(dir, name));
        }

        private SkinningData GetSkinningData(string dir, string name)
        {
            this.LoadAsset(dir, name);

            return ((Animation3D)this.AssetsDirDict[dir][name]).SkinningData;
        }

        private class Animation3D : Asset, IAssetAnimation3D
        {
            public Animation3D(string dir, string name)
                : base(name)
            {
                this.LoadAsset(dir, name);
            }

            public override void LoadAsset(string dir, string name)
            {
                if (!SkinnedModelsManager.Instance.ContainsAsset(dir, name))
                {
                    Model model = ModelsManager.Instance.GetModel(dir, name);

                    this.SkinningData = model.Tag as SkinningData;

                    if (this.SkinningData == null)
                    {
                        throw new InvalidOperationException($"Model {name} in dir {dir} does not contain SkinningData tag.");
                    }

                    SkinnedModelsManager.Instance.InsertAsset(dir, name, this);
                }                
            }

            public SkinningData SkinningData { get; private set; }
        }
    }
}