namespace SimpleWars.GUI.PrimitiveComponents
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Interfaces;
    using Utils;

    public class Button : IButton
    {
        private Rectangle rectangle;

        private Vector2 position;

        public Button(Vector2 position, Texture2D background, Vector2 dimensions, Color borderColor, int borderWidth, Action clickLogic)
        {
            this.Background = background;
            this.Dimensions = dimensions;

            this.Position = position;
            this.BorderColor = borderColor;
            this.ClickLogic = clickLogic;
            this.BorderWidth = borderWidth; 
        }

        public Vector2 Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;

                this.rectangle = new Rectangle(
                    (int)this.Position.X,
                    (int)this.Position.Y,
                    (int)(this.Dimensions.X * this.Background.Width),
                    (int)(this.Dimensions.Y * this.Background.Height));
            }
        }

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