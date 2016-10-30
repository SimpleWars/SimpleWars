﻿namespace SimpleWars.Display
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Display.Interfaces;
    using SimpleWars.Res.Interfaces;

    /// <summary>
    /// The game display.
    /// </summary>
    public abstract class GameDisplay : IGameDisplay
    {
        /// <summary>
        /// The load content.
        /// </summary>
        public abstract void LoadContent();

        /// <summary>
        /// The unload content.
        /// </summary>
        public abstract void UnloadContent();

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="spriteBatch">
        /// The sprite batch.
        /// </param>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}