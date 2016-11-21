namespace SimpleWars.GUI.PrimitiveComponents
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.GUI.Interfaces;

    public class PasswordTextNode : PartialTextNode, IPasswordTextNode  
    {
        public PasswordTextNode(IGui parent, Vector2 offsetFromParent, Vector2 dimensions, SpriteFont spriteFont, Color textColor, int charsDisplayed, char replacementSymbol = '*', int limit = int.MaxValue)
            : base(parent, offsetFromParent, dimensions, spriteFont, textColor, charsDisplayed, limit)
        {
            this.SymbolReplacement = replacementSymbol;
        }

        public char SymbolReplacement { get; set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            string replacement = new string(this.SymbolReplacement, this.DisplayText.Length);
            spriteBatch.DrawString(this.SpriteFont, replacement, this.Position, this.TextColor, 0f, Vector2.Zero, this.Dimensions, SpriteEffects.None, 0f);
        }
    }
}