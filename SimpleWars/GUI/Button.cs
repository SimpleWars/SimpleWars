namespace SimpleWars.GUI
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.AssetsManagement;

    public class Button
    {
        public Button(Vector2 position, Texture2D background, string textContent, Vector2 scale, Vector2 textOffset)
        {
            this.Position = position;
            this.Background = background;
            this.TextContent = textContent;
            this.Scale = scale;
            this.TextOffset = textOffset;
        }

        public Vector2 Position { get; set; }

        public Texture2D Background { get; set; }

        public string TextContent { get; set; }

        public Vector2 Scale { get; set; }

        public Vector2 TextOffset { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Background, this.Position, null, Color.White, 0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0f);

            spriteBatch.DrawString(SpriteFontManager.Instance.GetFont("Spritefonts", "Basic"), this.TextContent, this.Position + this.TextOffset, Color.Black);
        }
    }
}