namespace SimpleWars.GUI.Layouts
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Data.Contexts;
    using SimpleWars.GUI.Interfaces;
    using SimpleWars.Input;

    public abstract class Layout : ILayout
    {
        protected Layout(Texture2D background, float transparency = 1)
        {
            this.Background = background;
            this.Buttons = new HashSet<IButton>();
            this.TextBoxes = new HashSet<ITextBox>();
            this.TextNodes = new HashSet<ITextNode>();
        }

        public Vector2 Position { get; set; }

        public Vector2 Dimensions { get; set; }

        public ICollection<IButton> Buttons { get; }

        public ICollection<ITextBox> TextBoxes { get; }

        public ICollection<ITextNode> TextNodes { get; }

        public Texture2D Background { get; set; }

        public virtual void Update(GameTime gameTime)
        {
            if (Input.LeftMouseClick())
            {
                float mouseX = Input.MousePos.X;
                float mouseY = Input.MousePos.Y;

                foreach (var button in this.Buttons)
                {
                    button.DetectClick(mouseX, mouseY);
                }

                foreach (var textBox in this.TextBoxes)
                {
                    textBox.DetectClick(mouseX, mouseY);
                }
            }
            else
            {
                foreach (var textBox in this.TextBoxes)
                {
                    textBox.ReadInput(gameTime);
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Background, this.Position, null, Color.White, 0f, Vector2.Zero, this.Dimensions, SpriteEffects.None, 0f);

            foreach (var button in this.Buttons)
            {
                button.Draw(spriteBatch);
            }

            foreach (var textBox in this.TextBoxes)
            {
                textBox.Draw(spriteBatch);
            }

            foreach (var textNode in this.TextNodes)
            {
                textNode.Draw(spriteBatch);
            }
        }
    }
}