namespace SimpleWars.GUI.Layouts.PrimitiveLayouts
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Assets;
    using SimpleWars.Data.Contexts;
    using SimpleWars.GUI.Interfaces;
    using SimpleWars.GUI.PrimitiveComponents;
    using SimpleWars.Users;
    using SimpleWars.Users.Enums;

    public class RegisterLayout : Layout
    {
        private IButton registerButton;

        private ITextBox usernameTb;

        private ITextBox passwordTb;

        private ITextBox confirmPasswordTb;

        private bool passwordsDoNotMatch;

        public RegisterLayout(Texture2D background, GameContext context)
            : base(background)
        {
            this.RegisterState = RegisterState.None;

            this.Dimensions = new Vector2(240, 140);
            this.Position = new Vector2(500, 300);

            this.InitializeComponents(context);
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
            else if (this.RegisterState == RegisterState.UsernameTaken)
            {
                spriteBatch.DrawString(
                    SpriteFontManager.Instance.GetFont("Arial_18"),
                    "Username already taken",
                    this.Position + new Vector2(20, -20),
                    Color.Red);
            }
            else if (this.RegisterState == RegisterState.Error)
            {
                spriteBatch.DrawString(
                    SpriteFontManager.Instance.GetFont("Arial_18"),
                    "Internal error",
                    this.Position + new Vector2(20, -20),
                    Color.Red);
            }
        }

        public RegisterState RegisterState { get; private set; }

        private void InitializeComponents(GameContext context)
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
                this.Background,
                new Vector2(200, 30),
                Color.Transparent,
                2,
                () =>
                {
                    if (this.passwordTb.TextNode.TextContent == this.confirmPasswordTb.TextNode.TextContent)
                    {
                        this.RegisterState = UsersManager.RegisterUser(
                            this.usernameTb.TextNode.TextContent,
                            this.passwordTb.TextNode.TextContent,
                            context);
                    }
                    else
                    {
                        this.passwordsDoNotMatch = true;
                    }
                });

            var registerButtonTextNode = new TextNode(this.registerButton, new Vector2(60, 0), Vector2.One, "Register", SpriteFontManager.Instance.GetFont("Arial_22"), Color.Black);

            this.registerButton.TextNode = registerButtonTextNode;

            this.Buttons.Add(this.registerButton);
        }
    }
}