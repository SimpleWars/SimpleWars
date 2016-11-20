namespace SimpleWars.DisplayManagement.Displays
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Assets;
    using SimpleWars.Data.Contexts;
    using SimpleWars.GUI.Layouts;
    using SimpleWars.Users.Enums;

    /// <summary>
    /// The menu display.
    /// </summary>
    public class MenuDisplay : Display
    {
        /// <summary>
        /// The main frame.
        /// </summary>
        private Rectangle mainFrame;

        private Texture2D background;

        private LoginLayout loginGui;

        /// <summary>
        /// The load content.
        /// </summary>
        public override void LoadContent(GameContext context)
        {
            this.mainFrame = new Rectangle(
                0, 
                0, 
                (int)DisplayManager.Instance.Dimensions.X,
                (int)DisplayManager.Instance.Dimensions.Y);

            this.background = TexturesManager.Instance.GetTexture("Menu", "background");

            this.loginGui = new LoginLayout(DisplayManager.Instance.GraphicsDevice, context);
        }

        /// <summary>
        /// The unload content.
        /// </summary>
        public override void UnloadContent()
        {
            TexturesManager.Instance.DisposeAll();
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public override void Update(GameTime gameTime, GameContext context)
        {
            this.loginGui.Update(gameTime, context);
            if (this.loginGui.LoginState == LoginState.Successful)
            {
                DisplayManager.Instance.ChangeDisplay(new Test3Display(), context);
            }
        }

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="spriteBatch">
        /// The sprite batch.
        /// </param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.background, null, this.mainFrame);

            this.loginGui.Draw(spriteBatch);
        }
    }
}