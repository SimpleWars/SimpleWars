namespace SimpleWars.GUI
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.GUI.Interfaces;
    public class TextNode : ITextNode
    {
        public TextNode(
            Vector2 position,
            Vector2 dimensions,
            string textContent,
            SpriteFont spriteFont,
            Color textColor,
            string displayText = null)
        {
            this.Position = position;
            this.Dimensions = dimensions;
            this.TextContent = textContent;
            this.SpriteFont = spriteFont;
            this.TextColor = textColor;
            this.DisplayText = displayText ?? this.TextContent;
        }

        public Vector2 Position { get; set; }

        public Vector2 Dimensions { get; set; }

        public string TextContent { get; set; }

        public string DisplayText { get; set; }

        public SpriteFont SpriteFont { get; set; }

        public Color TextColor { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.SpriteFont, this.DisplayText, this.Position, this.TextColor, 0f, Vector2.Zero, this.Dimensions, SpriteEffects.None, 0f);
        }
    }
}