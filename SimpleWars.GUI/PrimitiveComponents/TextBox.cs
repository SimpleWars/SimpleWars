namespace SimpleWars.GUI
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using SimpleWars.Assets;
    using SimpleWars.GUI.Interfaces;
    using SimpleWars.Input;

    public class TextBox : ITextBox
    {
        private Rectangle rectangle;

        private Color stateClickedColor;

        private TimeSpan lastTyped = TimeSpan.Zero;

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

            this.stateClickedColor = Color.LightYellow;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.DrawInner(spriteBatch, this.IsClicked ? this.stateClickedColor : this.InnerColor);
            this.DrawBorder(spriteBatch);
        }

        public void DetectClick(float mouseX, float mouseY)
        {
            if (mouseX >= this.Position.X && mouseX <= this.Position.X + this.Dimensions.X && mouseY >= this.Position.Y
                && mouseY <= this.Position.Y + this.Dimensions.Y && this.IsClicked == false)
            {
                this.ClickLogic.Invoke();
            }
            else
            {
                this.IsClicked = false;
            }
        }

        public void ReadInput(GameTime gameTime)
        {
            if (!this.IsClicked)
            {
                return;
            }
            this.lastTyped = this.lastTyped.Add(gameTime.ElapsedGameTime);
            IEnumerable<Keys> pressedKeys = Input.GetKeysPressed();
            if (this.lastTyped.TotalMilliseconds >= 200)
            {
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
                        if (IsKeyAChar(keyPressed))
                        {
                            if (Input.KeyDown(Keys.LeftShift))
                            {
                                this.TextContent += keyPressed.ToString();
                            }
                            else
                            {
                                this.TextContent += keyPressed.ToString().ToLower();
                            }
                        }
                        else if (IsKeyADigit(keyPressed))
                        {
                            this.TextContent += keyPressed.ToString()[1];
                        }

                        this.lastTyped = gameTime.ElapsedGameTime;
                    }
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

        private void DrawInner(SpriteBatch spriteBatch, Color innerColor)
        {
            spriteBatch.Draw(PointTextures.WhitePoint, this.Position, null, innerColor, 0f, Vector2.Zero, this.Dimensions, SpriteEffects.None, 0f);

            spriteBatch.DrawString(SpriteFontManager.Instance.GetFont("Spritefonts", "Basic"), this.TextContent, this.Position + new Vector2(7, 7), Color.Black);
        }

        private void DrawBorder(SpriteBatch spriteBatch)
        {
            DrawRectangle(spriteBatch, this.rectangle, this.BorderColor, this.BorderWidth);
        }

        private static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            spriteBatch.Draw(PointTextures.WhitePoint, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(PointTextures.WhitePoint, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            spriteBatch.Draw(PointTextures.WhitePoint, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(PointTextures.WhitePoint, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
        }

        private static bool IsKeyAChar(Keys key)
        {
            return key >= Keys.A && key <= Keys.Z;
        }

        private static bool IsKeyADigit(Keys key)
        {
            return key >= Keys.D0 && key <= Keys.D9;
        }
    }
}