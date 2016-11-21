namespace SimpleWars.GUI.PrimitiveComponents
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.GUI.Interfaces;
    using SimpleWars.Utils;

    public class Button : IButton
    {
        private Rectangle rectangle;
        public Button(Vector2 position, Texture2D background, Vector2 dimensions, Color borderColor, int borderWidth, Action clickLogic)
        {
            this.Position = position;
            this.Background = background;
            this.Dimensions = dimensions;
            this.BorderColor = borderColor;
            this.ClickLogic = clickLogic;
            this.BorderWidth = borderWidth;

            if (this.Background.Width == 1 && this.Background.Height == 1)
            {
                this.rectangle = new Rectangle(
                    (int)this.Position.X,
                    (int)this.Position.Y,
                    (int)this.Dimensions.X,
                    (int)this.Dimensions.Y);
            }
            else
            {
                this.rectangle = new Rectangle(
                    (int)this.Position.X,
                    (int)this.Position.Y,
                    this.Background.Width,
                    this.Background.Height);
            }
        }

        public Vector2 Position { get; set; }

        public Texture2D Background { get; set; }

        public TextNode TextNode { get; set; }

        public Color BorderColor { get; set; }

        public int BorderWidth { get; set; }

        public Vector2 Dimensions { get; set; }

        public Action ClickLogic { get; set; }

        public bool IsClicked { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Background, this.Position, null, Color.White, 0f, Vector2.Zero, this.Dimensions, SpriteEffects.None, 0f);

            PrimitiveShapes.DrawRectangle(spriteBatch, this.rectangle, this.BorderColor, this.BorderWidth);

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