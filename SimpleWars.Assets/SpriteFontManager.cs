namespace SimpleWars.Assets
{
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Assets.Interfaces;

    public class SpriteFontManager : AssetsManager
    {
        private static SpriteFontManager instance;

        private SpriteFontManager()
            : base(typeof(Font))
        {
        }

        public static SpriteFontManager Instance => instance ?? (instance = new SpriteFontManager());

        public SpriteFont GetFont(string dir, string name)
        {
            this.LoadAsset(dir, name);

            return ((Font)this.AssetsDirDict[dir][name]).SpriteFont;
        }

        private class Font : Asset, ISpritefont
        {
            public Font(string name)
                : base(name)
            {
            }

            public override void LoadAsset(string dir, string name)
            {
                if (!SpriteFontManager.Instance.ContainsAsset(dir, name))
                {
                    this.SpriteFont = this.Content.Load<SpriteFont>(dir + "/" + name);
                    SpriteFontManager.Instance.InsertAsset(dir, name, this);
                }
                else
                {
                    this.SpriteFont = SpriteFontManager.Instance.GetFont(dir, name);
                }
            }

            public SpriteFont SpriteFont { get; private set; }
        }
    }
}