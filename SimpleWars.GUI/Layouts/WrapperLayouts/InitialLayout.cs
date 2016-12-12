namespace SimpleWars.GUI.Layouts.WrapperLayouts
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Assets;
    using Interfaces;
    using PrimitiveLayouts;
    using PrimitiveComponents;
    using Utils;

    public class InitialLayout : Layout
    {
        private ILayout activeLayout;

        private readonly LoginLayout loginLayout;

        private readonly RegisterLayout registerLayout;

        public InitialLayout(Texture2D background)
            : base(background)
        {
            this.loginLayout = new LoginLayout(PointTextures.TransparentPoint);
            this.activeLayout = this.loginLayout;

            this.registerLayout = new RegisterLayout(PointTextures.TransparentPoint);

            this.InitializeComponents();
        }

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