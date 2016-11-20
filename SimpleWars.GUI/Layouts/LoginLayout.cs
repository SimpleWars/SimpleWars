namespace SimpleWars.GUI.Layouts
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Data.Contexts;
    using SimpleWars.GUI.Interfaces;
    using SimpleWars.Input;
    public class LoginLayout : ILayout
    {
        public LoginLayout(GraphicsDevice device)
        {
            this.Buttons = new HashSet<IButton>();
            this.TextBoxes = new HashSet<ITextBox>();

            this.Background = new Texture2D(device, 1, 1);
            this.Background.SetData<Color>(new Color[] { Color.CornflowerBlue });

            // Just placeholder values for now. Will be properly calculated.
            this.Dimensions = new Vector2(300, 300);
            this.Position = new Vector2(300, 300);

            this.InitializeComponents();
        }

        public Vector2 Position { get; set; }

        public Vector2 Dimensions { get; set; }

        public ICollection<IButton> Buttons { get; }

        public ICollection<ITextBox> TextBoxes { get; }

        public Texture2D Background { get; set; }

        public void Update(GameTime gameTime, GameContext context)
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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Background, this.Position, Color.White);

            foreach (var button in this.Buttons)
            {
                button.Draw(spriteBatch);
            }

            foreach (var textBox in this.TextBoxes)
            {
                textBox.Draw(spriteBatch);
            }
        }


        private void InitializeComponents()
        {
            var usernameTb = new TextBox(
                this.Position + new Vector2(20, 20),
                new Vector2(100, 30),
                Color.Black,
                Color.White);
            var passwordTb = new TextBox(
                this.Position + new Vector2(20, 60),
                new Vector2(100, 30),
                Color.Black,
                Color.White);

            this.TextBoxes.Add(usernameTb);
            this.TextBoxes.Add(passwordTb);

            var loginButton = new Button(this.Position + new Vector2(20, 100), this.Background, "Login", new Vector2(100, 30), new Vector2(5, 5),
                this.LoginPlayer);

            this.Buttons.Add(loginButton);
        }

        private void LoginPlayer()
        {

        }
    }
}