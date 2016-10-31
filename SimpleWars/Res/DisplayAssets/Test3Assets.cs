namespace SimpleWars.Res.DisplayAssets
{
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Res.Interfaces;
    public class Test3Assets : IAssetLoader
    {
        public Test3Assets()
        {
            this.LoadAssets();
        }

        public Model Model { get; private set; }

        public void LoadAssets()
        {
            Assets3Manager.Instance.Load3DAsset("Models3D", "tree");

            this.Model = Assets3Manager.Instance.Get3DAsset("tree").Model;
        }

        public void UnloadAssets()
        {
            Assets3Manager.Instance.Dispose3DAsset("Models3D", "tree");
        }
    }
}