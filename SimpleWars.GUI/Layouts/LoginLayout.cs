namespace SimpleWars.GUI.Layouts
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Assets;
    using SimpleWars.Data.Contexts;
    using SimpleWars.GUI.Interfaces;
    using SimpleWars.Input;
    using SimpleWars.Users;
    using SimpleWars.Users.Enums;

    public class LoginLayout : ILayout
    {
        public LoginLayout(GraphicsDevice device, GameContext context)
        {
            this.LoginState = LoginState.None;

            this.Buttons = new HashSet<IButton>();
            this.TextBoxes = new HashSet<ITextBox>();

            this.Background = new Texture2D(device, 1, 1);
            this.Background.SetData<Color>(new Color[] { Color.CornflowerBlue });

            // Just placeholder values for now. Will be properly calculated.
            this.Dimensions = new Vector2(500, 300);
            this.Position = new Vector2(500, 300);

            this.InitializeComponents(context);
        }

        public LoginState LoginState { get; set; }

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

            if (this.LoginState == LoginState.Invalid)
            {
                spriteBatch.DrawString(SpriteFontManager.Instance.GetFont("Spritefonts", "Basic"), "Invalid credentials", this.Position - new Vector2(0, 50), Color.Red);
            }
        }


        private void InitializeComponents(GameContext context)
        {
            var usernameTb = new TextBox(
                this.Position + new Vector2(20, 20),
                new Vector2(200, 30),
                Color.Black,
                Color.White,
                new Vector2(8, 5), 
                14);

            var passwordTb = new TextBox(
                this.Position + new Vector2(20, 60),
                new Vector2(200, 30),
                Color.Black,
                Color.White,
                new Vector2(8, 5),
                14);

            this.TextBoxes.Add(usernameTb);
            this.TextBoxes.Add(passwordTb);

            var loginButton = new Button(
                this.Position + new Vector2(20, 100),
                this.Background,
                "Log In",
                new Vector2(200, 30),
                new Vector2(70, 0),
                () =>
                    {
                        string result = UsersManager.LoginUser(usernameTb.TextContent, passwordTb.TextContent, context);

                        if (result == "Successful login")
                        {
                            this.LoginState = LoginState.Successful;
                        }
                        else if (result == "Invalid credentials")
                        {
                            this.LoginState = LoginState.Invalid;
                        }
                    });

            this.Buttons.Add(loginButton);
        }
    }
}