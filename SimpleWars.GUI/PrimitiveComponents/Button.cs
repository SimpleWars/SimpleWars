namespace SimpleWars.GUI.PrimitiveComponents
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.GUI.Interfaces;

    public class Button : IButton
    {
        public Button(Vector2 position, Texture2D background, Vector2 dimensions, Vector2 textOffset, Action clickLogic)
        {
            this.Position = position;
            this.Background = background;
            this.Dimensions = dimensions;
            this.TextOffset = textOffset;
            this.ClickLogic = clickLogic;
        }

        public Vector2 Position { get; set; }

        public Texture2D Background { get; set; }

        public TextNode TextNode { get; set; }

        public Vector2 Dimensions { get; set; }

        public Vector2 TextOffset { get; set; }

        public Action ClickLogic { get; set; }

        public bool IsClicked { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Background, this.Position, null, Color.White, 0f, Vector2.Zero, this.Dimensions, SpriteEffects.None, 0f);

            this.TextNode?.Draw(spriteBatch);
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