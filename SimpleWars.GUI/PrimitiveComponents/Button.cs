namespace SimpleWars.GUI
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Assets;
    using SimpleWars.GUI.Interfaces;

    public class Button : IButton
    {
        public Button(Vector2 position, Texture2D background, string textContent, Vector2 dimensions, Vector2 textOffset, Action clickLogic, ITextBox attachedTextBox = null)
        {
            this.Position = position;
            this.Background = background;
            this.TextContent = textContent;
            this.Dimensions = dimensions;
            this.TextOffset = textOffset;
            this.ClickLogic = clickLogic;
            this.AttachedTextBox = attachedTextBox;
        }

        public Vector2 Position { get; set; }

        public Texture2D Background { get; set; }

        public string TextContent { get; set; }

        public Vector2 Dimensions { get; set; }

        public Vector2 TextOffset { get; set; }

        public Action ClickLogic { get; set; }

        public ITextBox AttachedTextBox { get; set; }

        public bool IsClicked { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Background, this.Position, null, Color.White, 0f, Vector2.Zero, this.Dimensions, SpriteEffects.None, 0f);

            spriteBatch.DrawString(SpriteFontManager.Instance.GetFont("Spritefonts", "Basic"), this.TextContent, this.Position + this.TextOffset, Color.Black);
        }

        public void DetectClick(float mouseX, float mouseY)
        {
            if (mouseX >= this.Position.X && mouseX <= this.Position.X + this.Background.Width * this.Dimensions.X
                && mouseY >= this.Position.Y && mouseY <= this.Position.Y + this.Background.Height * this.Dimensions.Y && this.IsClicked == false)
            {
                this.IsClicked = true;
                this.ClickLogic.Invoke();
            }
            else
            {
                this.IsClicked = false;
            }
        }
    }
}