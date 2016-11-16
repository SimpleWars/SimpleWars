namespace SimpleWars.GUI
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using SimpleWars.GUI.Interfaces;
    using SimpleWars.InputManagement;

    public class TextBox : ITextBox
    {
        private static Texture2D pointTexture;

        private Rectangle rectangle;

        public TextBox(Vector2 position, Vector2 dimensions, Color borderColor, Color innerColor, string textContent = "", int borderWidth = 2)
        {
            this.Position = position;
            this.Dimensions = dimensions;
            this.BorderColor = borderColor;
            this.InnerColor = innerColor;
            this.BorderWidth = borderWidth;
            this.TextContent = textContent;

            this.ClickLogic = () => { this.IsClicked = true; };

            this.rectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)this.Dimensions.X, (int)this.Dimensions.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.DrawInner(spriteBatch);
            this.DrawBorder(spriteBatch);
        }

        public void DetectClick(float mouseX, float mouseY)
        {
            if (mouseX >= this.Position.X && mouseX <= this.Position.X + this.Dimensions.X && mouseY >= this.Position.Y
                && mouseY <= this.Position.Y + this.Dimensions.Y)
            {
                this.ClickLogic.Invoke();
            }
            else
            {
                this.IsClicked = false;
            }
        }

        public void ReadInput()
        {
            if (!this.IsClicked)
            {
                return;
            }

            IEnumerable<Keys> pressedKeys = Input.GetKeysPressed();
            foreach (var keyPressed in pressedKeys)
            {
                if (keyPressed == Keys.Back)
                {
                    if (!string.IsNullOrEmpty(this.TextContent))
                    {
                        this.TextContent = this.TextContent.Substring(0, this.TextContent.Length - 1);
                    }
                }
                else
                {
                    this.TextContent += keyPressed.ToString();
                }
            }
        }

        public Vector2 Position { get; set; }

        public Vector2 Dimensions { get; set; }

        public string TextContent { get; set; }

        public Color BorderColor { get; set; }

        public Color InnerColor { get; set; }

        public int BorderWidth { get; set; }

        public bool IsClicked { get; set; }

        public Action ClickLogic { get; set; }

        private void DrawInner(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pointTexture, this.Position, null, this.InnerColor, 0f, Vector2.Zero, this.Dimensions, SpriteEffects.None, 0f);
        }

        private void DrawBorder(SpriteBatch spriteBatch)
        {
            DrawRectangle(spriteBatch, this.rectangle, this.BorderColor, this.BorderWidth);
        }

        private static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            if (pointTexture == null)
            {
                pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                pointTexture.SetData<Color>(new Color[] { Color.White });
            }

            spriteBatch.Draw(pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            spriteBatch.Draw(pointTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
        }
    }
}