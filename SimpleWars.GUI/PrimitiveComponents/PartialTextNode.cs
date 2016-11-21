namespace SimpleWars.GUI.PrimitiveComponents
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.GUI.Interfaces;

    public class PartialTextNode : IPartialTextNode
    {
        private int charsDisplayed;

        private int cursorIndex;

        private string textContent;

        public PartialTextNode(
            IGui parent,
            Vector2 offsetFromParent,
            Vector2 dimensions,
            SpriteFont spriteFont,
            Color textColor,
            int charsDisplayed,
            int limit = int.MaxValue)
        {
            this.Parent = parent;
            this.Position = this.Parent != null ? this.Parent.Position + offsetFromParent : offsetFromParent;
            this.Dimensions = dimensions;
            this.textContent = string.Empty;
            this.TextContent = this.textContent;
            this.SpriteFont = spriteFont;
            this.TextColor = textColor;
            this.CharsDisplayed = charsDisplayed;
            this.Limit = limit;
        }

        public Vector2 Position { get; set; }

        public Vector2 Dimensions { get; set; }

        public string TextContent
        {
            get
            {
                return this.textContent;
            }

            set
            {
                if (value == null)
                {
                    this.textContent = string.Empty;
                }
                else if (value.Length > this.Limit)
                {
                }
                else if (value.Length > this.textContent.Length && value.Length > this.CharsDisplayed)
                {
                    this.textContent = value;
                    this.CursorIndex = value.Length - this.CharsDisplayed;
                    this.MoveCursor(0);
                }
                else
                {
                    int moveCursorAmount = value.Length - this.textContent.Length;
                    this.textContent = value;
                    this.MoveCursor(moveCursorAmount);
                }
            }
        }

        public SpriteFont SpriteFont { get; set; }

        public Color TextColor { get; set; }

        public IGui Parent { get; set; }

        public int CharsDisplayed
        {
            get
            {
                return this.charsDisplayed;
            }

            set
            {
                if (value < 0)
                {
                    this.charsDisplayed = 0;
                    return;
                }

                this.charsDisplayed = value;
            }
        }

        public int CursorIndex
        {
            get
            {
                return this.cursorIndex;
            }

            private set
            {
                if (value < 0)
                {
                    this.cursorIndex = 0;
                }
                else if (value <= this.TextContent.Length - this.CharsDisplayed)
                {
                    this.cursorIndex = value;
                }
            }
        }

        public int Limit { get; }

        public string DisplayText { get; private set; }

        public void MoveCursor(int amount)
        {
            this.CursorIndex += amount;

            this.DisplayText = this.TextContent.Substring(
               this.CursorIndex,
               Math.Min(this.CharsDisplayed, this.TextContent.Length));
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.SpriteFont, this.DisplayText, this.Position, this.TextColor, 0f, Vector2.Zero, this.Dimensions, SpriteEffects.None, 0f);
        }
    }
}