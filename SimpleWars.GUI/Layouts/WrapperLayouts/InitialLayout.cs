namespace SimpleWars.GUI.Layouts.WrapperLayouts
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Assets;
    using SimpleWars.Data.Contexts;
    using SimpleWars.GUI.Interfaces;
    using SimpleWars.GUI.Layouts.PrimitiveLayouts;
    using SimpleWars.GUI.PrimitiveComponents;
    using SimpleWars.Users.Enums;
    using SimpleWars.Utils;

    public class InitialLayout : Layout
    {
        private ILayout activeLayout;

        private readonly LoginLayout loginLayout;

        private readonly RegisterLayout registerLayout;

        public InitialLayout(Texture2D background, GameContext context)
            : base(background)
        {
            this.loginLayout = new LoginLayout(PointTextures.TransparentPoint, context);
            this.activeLayout = this.loginLayout;

            this.registerLayout = new RegisterLayout(PointTextures.TransparentPoint, context);

            this.InitializeComponents();
        }

        public LoginState LoginState => this.loginLayout.LoginState;

        public RegisterState RegisterState => this.registerLayout.RegisterState;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.activeLayout.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            this.activeLayout.Draw(spriteBatch);
        }

        private void InitializeComponents()
        {
            var loginLayoutButton = new Button(
                this.Position + new Vector2(410, 250),
                PointTextures.TransparentBlackPoint,
                new Vector2(200, 30),
                Color.Black,
                2,
                () => { this.activeLayout = this.loginLayout; });

            var loginLayoutButtonTextNode = new TextNode(loginLayoutButton, new Vector2(70, 0), Vector2.One, "Login", SpriteFontManager.Instance.GetFont("Arial_22"), Color.White);

            loginLayoutButton.TextNode = loginLayoutButtonTextNode;

            var registerLayoutButton = new Button(
                this.Position + new Vector2(630, 250),
                PointTextures.TransparentBlackPoint,
                new Vector2(200, 30),
                Color.Black,
                2,
                () => { this.activeLayout = this.registerLayout; });

            var registerLayoutButtonTextNode = new TextNode(registerLayoutButton, new Vector2(50, 0), Vector2.One, "Register", SpriteFontManager.Instance.GetFont("Arial_22"), Color.White);

            registerLayoutButton.TextNode = registerLayoutButtonTextNode;

            this.Buttons.Add(loginLayoutButton);
            this.Buttons.Add(registerLayoutButton);
        }
    }
}