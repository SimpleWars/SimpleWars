namespace SimpleWars.DisplayManagement.Displays
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Assets;
    using SimpleWars.GUI.Layouts.WrapperLayouts;
    using SimpleWars.Users.Enums;
    using SimpleWars.Utils;

    /// <summary>
    /// The menu display.
    /// </summary>
    public class InitialDisplay : Display
    {
        /// <summary>
        /// The main frame.
        /// </summary>
        private Rectangle mainFrame;

        private Texture2D background;

        private InitialLayout initialGui;

        /// <summary>
        /// The load content.
        /// </summary>
        public override void LoadContent()
        {
            this.mainFrame = new Rectangle(
                0, 
                0, 
                (int)DisplayManager.Instance.Dimensions.X,
                (int)DisplayManager.Instance.Dimensions.Y);

            this.background = TexturesManager.Instance.GetTexture("Menu", "background");

            this.initialGui = new InitialLayout(PointTextures.TransparentPoint);
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
        public override void Update(GameTime gameTime)
        {
            this.initialGui.Update(gameTime);

            if (this.initialGui.LoginState == LoginState.Successful || this.initialGui.RegisterState == RegisterState.Successful)
            {
                DisplayManager.Instance.ChangeDisplay(new HomeWorldDisplay());
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
            base.Draw(spriteBatch);
            spriteBatch.Draw(this.background, null, this.mainFrame);

            this.initialGui.Draw(spriteBatch);
        }
    }
}