namespace SimpleWars.GUI.Layouts.PrimitiveLayouts
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Assets;
    using SimpleWars.GUI.Interfaces;
    using SimpleWars.GUI.PrimitiveComponents;
    using SimpleWars.Users;
    using SimpleWars.Users.Enums;
    using SimpleWars.Utils;

    public class LoginLayout : Layout
    {
        private ITextBox usernameTb;
        private ITextBox passwordTb;
        private IButton loginButton;

        public LoginLayout(Texture2D background)
            : base(background)
        {
            this.LoginState = LoginState.None;

            this.Dimensions = new Vector2(240, 140);
            this.Position = new Vector2(500, 300);

            this.InitializeComponents();
        }

        public LoginState LoginState { get; private set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (this.LoginState == LoginState.Invalid)
            {
                spriteBatch.DrawString(SpriteFontManager.Instance.GetFont("Arial_18"), "Invalid credentials", this.Position + new Vector2(20, -20), Color.Red);
            }
            else if (this.LoginState == LoginState.Error)
            {
                spriteBatch.DrawString(SpriteFontManager.Instance.GetFont("Arial_18"), "Internal error", this.Position + new Vector2(20, -20), Color.Red);
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


            this.usernameTb.DefaultTextNode = usernameTbDefaultTextNode;
            this.passwordTb.DefaultTextNode = passwordDefaultTextNode;

            this.usernameTb.TextNode = usernameTbPartialTextNode;
            this.passwordTb.TextNode = passwordTbPartialTextNode;
            

            this.TextBoxes.Add(this.usernameTb);
            this.TextBoxes.Add(this.passwordTb);

            this.loginButton = new Button(
                this.Position + new Vector2(20, 100),
                PointTextures.TransparentBlackPoint,
                new Vector2(200, 30),
                Color.Black,
                2,
                () =>
                    {
                        this.LoginState = UsersManager.LoginUser(this.usernameTb.TextNode.TextContent, this.passwordTb.TextNode.TextContent);
                    });

            var loginButtonTextNode = new TextNode(this.loginButton, new Vector2(70, -2), Vector2.One, "Log In", SpriteFontManager.Instance.GetFont("Arial_22"), Color.White);

            this.loginButton.TextNode = loginButtonTextNode;

            this.Buttons.Add(this.loginButton);
        }
    }
}