namespace SimpleWars.GUI.Layouts.PrimitiveLayouts
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Assets;
    using SimpleWars.GUI.Interfaces;
    using SimpleWars.GUI.PrimitiveComponents;
    using SimpleWars.Users;
    using SimpleWars.Utils;

    public class RegisterLayout : Layout
    {
        private IButton registerButton;

        private ITextBox usernameTb;

        private ITextBox passwordTb;

        private ITextBox confirmPasswordTb;

        private bool passwordsDoNotMatch;

        private bool passwordEmpty;

        private bool usernameEmpty;

        public RegisterLayout(Texture2D background)
            : base(background)
        {
            this.Dimensions = new Vector2(240, 140);
            this.Position = new Vector2(500, 300);

            this.InitializeComponents();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (this.passwordsDoNotMatch)
            {
                spriteBatch.DrawString(
                    SpriteFontManager.Instance.GetFont("Arial_18"),
                    "Passwords do not match",
                    this.Position + new Vector2(20, -20),
                    Color.Red);
            }
            else if (this.passwordEmpty)
            {
                spriteBatch.DrawString(
                    SpriteFontManager.Instance.GetFont("Arial_18"),
                    "Password cannot be empty",
                    this.Position + new Vector2(20, -20),
                    Color.Red);
            }
            else if (this.usernameEmpty)
            {
                spriteBatch.DrawString(
                    SpriteFontManager.Instance.GetFont("Arial_18"),
                    "Username cannot be empty",
                    this.Position + new Vector2(20, -20),
                    Color.Red);
            }
        }

        private void InitializeComponents()
        {
            this.usernameTb = new TextBox(
               this.Position + new Vector2(20, 20),
               new Vector2(200, 30),
               Color.Black,
               Color.White);

            this.passwordTb = new TextBox(
                this.Position + new Vector2(20, 60),
                new Vector2(200, 30),
                Color.Black,
                Color.White);

            this.confirmPasswordTb = new TextBox(
                this.Position + new Vector2(20, 100),
                new Vector2(200, 30),
                Color.Black,
                Color.White);


            var usernameTbDefaultTextNode = new TextNode(
                this.usernameTb,
                new Vector2(30, 0),
                Vector2.One,
                "Username",
                SpriteFontManager.Instance.GetFont("Arial_Italic_22"),
                Color.Gray);

            var passwordDefaultTextNode = new TextNode(
                this.passwordTb,
                new Vector2(30, 0),
                Vector2.One,
                "Password",
                SpriteFontManager.Instance.GetFont("Arial_Italic_22"),
                Color.Gray);

            var passwordConfirmDefaultTextNode = new TextNode(
                this.confirmPasswordTb,
                new Vector2(5, 5),
                new Vector2(0.8f, 0.8f),
                "Confirm Password",
                SpriteFontManager.Instance.GetFont("Arial_Italic_22"),
                Color.Gray);

            var usernameTbPartialTextNode = new PartialTextNode(
                this.usernameTb,
                new Vector2(8, 0),
                Vector2.One,
                SpriteFontManager.Instance.GetFont("Arial_22"),
                Color.Black,
                12,
                12);

            var passwordTbPartialTextNode = new PasswordTextNode(
                this.passwordTb,
                new Vector2(15, 3),
                Vector2.One,
                SpriteFontManager.Instance.GetFont("Arial_26"),
                Color.Black,
                12,
                '*',
                12);

            var passwordConfirmTbPartialTextNode = new PasswordTextNode(
               this.confirmPasswordTb,
               new Vector2(15, 3),
               Vector2.One,
               SpriteFontManager.Instance.GetFont("Arial_26"),
               Color.Black,
               12,
               '*',
               12);


            this.usernameTb.DefaultTextNode = usernameTbDefaultTextNode;
            this.passwordTb.DefaultTextNode = passwordDefaultTextNode;
            this.confirmPasswordTb.DefaultTextNode = passwordConfirmDefaultTextNode;

            this.usernameTb.TextNode = usernameTbPartialTextNode;
            this.passwordTb.TextNode = passwordTbPartialTextNode;
            this.confirmPasswordTb.TextNode = passwordConfirmTbPartialTextNode;


            this.TextBoxes.Add(this.usernameTb);
            this.TextBoxes.Add(this.passwordTb);
            this.TextBoxes.Add(this.confirmPasswordTb);

            this.registerButton = new Button(
                this.Position + new Vector2(20, 140),
                PointTextures.TransparentBlackPoint,
                new Vector2(200, 30),
                Color.Black,
                2,
                () =>
                {
                    if (string.IsNullOrWhiteSpace(this.usernameTb.TextNode.TextContent))
                    {
                        this.usernameEmpty = true;
                        this.passwordEmpty = false;
                        this.passwordsDoNotMatch = false;
                    }
                    else if (string.IsNullOrWhiteSpace(this.passwordTb.TextNode.TextContent))
                    {
                        this.passwordEmpty = true;
                        this.passwordsDoNotMatch = false;
                        this.usernameEmpty = false;
                    }
                    else if (this.passwordTb.TextNode.TextContent == this.confirmPasswordTb.TextNode.TextContent)
                    {
                        UsersManager.RegisterUser(
                            this.usernameTb.TextNode.TextContent,
                            this.passwordTb.TextNode.TextContent);
                    }
                    else
                    {
                        this.passwordsDoNotMatch = true;
                        this.passwordEmpty = false;
                        this.usernameEmpty = false;
                    }
                });

            var registerButtonTextNode = new TextNode(this.registerButton, new Vector2(50, 0), Vector2.One, "Register", SpriteFontManager.Instance.GetFont("Arial_22"), Color.White);

            this.registerButton.TextNode = registerButtonTextNode;

            this.Buttons.Add(this.registerButton);
        }
    }
}