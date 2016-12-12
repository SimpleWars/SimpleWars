namespace SimpleWars.Assets
{
    using Microsoft.Xna.Framework.Graphics;

    using Interfaces;

    public class SpriteFontManager : AssetsManager
    {
        private static SpriteFontManager instance;

        private SpriteFontManager()
            : base(typeof(SpriteFontManager.Font))
        {
        }

        public static SpriteFontManager Instance => instance ?? (instance = new SpriteFontManager());

        public SpriteFont GetFont(string name)
        {
            this.LoadAsset("Spritefonts", name);

            return ((Font)this.AssetsDirDict["Spritefonts"][name]).SpriteFont;
        }

        private class Font : Asset, ISpritefont
        {
            public Font(string dir, string name)
                : base(name)
            {
                this.LoadAsset(dir, name);
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
                    this.SpriteFont = SpriteFontManager.Instance.GetFont(name);
                }
            }

            public SpriteFont SpriteFont { get; private set; }
        }
    }
}