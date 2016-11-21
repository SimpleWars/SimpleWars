namespace SimpleWars.GUI.PrimitiveComponents
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.GUI.Interfaces;

    public class TextNode : ITextNode
    {
        public TextNode(
            IGui parent,
            Vector2 offsetFromParent,
            Vector2 dimensions,
            string textContent,
            SpriteFont spriteFont,
            Color textColor)
        {
            this.Parent = parent;
            this.Position = this.Parent != null ? this.Parent.Position + offsetFromParent : offsetFromParent;
            this.Dimensions = dimensions;
            this.TextContent = textContent;
            this.SpriteFont = spriteFont;
            this.TextColor = textColor;
        }

        public Vector2 Position { get; set; }

        public Vector2 Dimensions { get; set; }

        public string TextContent { get; set; }

        public SpriteFont SpriteFont { get; set; }

        public Color TextColor { get; set; }

        public IGui Parent { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.SpriteFont, this.TextContent, this.Position, this.TextColor, 0f, Vector2.Zero, this.Dimensions, SpriteEffects.None, 0f);
        }
    }
}