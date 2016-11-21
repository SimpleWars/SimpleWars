namespace SimpleWars.GUI.PrimitiveComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using SimpleWars.GUI.Interfaces;
    using SimpleWars.Input;

    public class TextBox : ITextBox
    {
        private Rectangle rectangle;

        private Color stateClickedColor;

        private TimeSpan lastTyped = TimeSpan.Zero;

        public TextBox(Vector2 position, Vector2 dimensions, Color borderColor, Color innerColor, int borderWidth = 2)
        {
            this.Position = position;
            this.Dimensions = dimensions;
            this.BorderColor = borderColor;
            this.InnerColor = innerColor;
            this.BorderWidth = borderWidth;

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
            if (!(this.lastTyped.TotalMilliseconds >= 200))
            {
                return;
            }

            IEnumerable<Keys> pressedKeys = Input.GetKeysPressed();

            foreach (var keyPressed in pressedKeys)
            {
                switch (keyPressed)
                {
                    case Keys.Back:
                        if (!string.IsNullOrEmpty(this.TextNode.TextContent))
                        {
                            this.TextNode.TextContent = this.TextNode.TextContent.Substring(
                                0,
                                this.TextNode.TextContent.Length - 1);
                        }

                        break;

                    case Keys.Left:
                        this.TextNode.MoveCursor(-1);
                        break;

                    case Keys.Right:
                        this.TextNode.MoveCursor(1);
                        break;

                    default:
                        if (IsKeyAChar(keyPressed))
                        {
                            if (Input.KeyDown(Keys.LeftShift))
                            {
                                this.TextNode.TextContent += keyPressed.ToString();
                            }
                            else
                            {
                                this.TextNode.TextContent += keyPressed.ToString().ToLower();
                            }
                        }
                        else if (IsKeyADigit(keyPressed))
                        {
                            this.TextNode.TextContent += keyPressed.ToString().Last();
                        }

                        break;
                }

                this.lastTyped = gameTime.ElapsedGameTime;
            }
        }

        public Vector2 Position { get; set; }

        public Vector2 Dimensions { get; set; }

        public PartialTextNode TextNode { get; set; }

        public TextNode DefaultTextNode { get; set; }

        public Color BorderColor { get; set; }

        public Color InnerColor { get; set; }

        public int BorderWidth { get; set; }

        public bool IsClicked { get; set; }

        public Action ClickLogic { get; set; }

        private void DrawInner(SpriteBatch spriteBatch, Color innerColor)
        {
            spriteBatch.Draw(PointTextures.WhitePoint, this.Position, null, innerColor, 0f, Vector2.Zero, this.Dimensions, SpriteEffects.None, 0f);

            if (this.TextNode != null && this.TextNode.TextContent != "")
            {
                this.TextNode.Draw(spriteBatch);
            }
            else
            {
                this.DefaultTextNode?.Draw(spriteBatch);
            }
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
            return (key >= Keys.D0 && key <= Keys.D9) || (key >= Keys.NumPad0 && key <= Keys.NumPad9);
        }
    }
}