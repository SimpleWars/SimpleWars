﻿namespace SimpleWars.Display
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Display.Interfaces;

    /// <summary>
    /// The display manager.
    /// </summary>
    public class DisplayManager : IDisplayManager
    {
        /// <summary>
        /// The instance.
        /// </summary>
        private static DisplayManager instance;


        /// <summary>
        /// Prevents a default instance of the <see cref="DisplayManager"/> class from being created. 
        /// Initializes a new instance of the <see cref="DisplayManager"/> class.
        /// </summary>
        private DisplayManager()
        {
            this.CurrentDisplay = new MenuDisplay();
            this.Dimensions = new Vector2(1280, 720);
        }

        /// <summary>
        /// The instance.
        /// </summary>
        public static DisplayManager Instance = instance ?? (instance = new DisplayManager());

        /// <summary>
        /// Gets the dimensions.
        /// </summary>
        public Vector2 Dimensions { get; private set; }

        /// <summary>
        /// Gets the content.
        /// </summary>
        public ContentManager Content { get; private set; }

        /// <summary>
        /// Gets the current display.
        /// </summary>
        public IGameDisplay CurrentDisplay { get; set; }

        /// <summary>
        /// The load content.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        public void LoadContent(ContentManager content)
        {
            this.Content = new ContentManager(content.ServiceProvider, content.RootDirectory);
            this.CurrentDisplay.LoadContent();
        }

        /// <summary>
        /// The unload content.
        /// </summary>
        public void UnloadContent()
        {
            this.Content.Unload();
            this.CurrentDisplay.UnloadContent();
        }

        public void ChangeDisplay(IGameDisplay display)
        {
            this.CurrentDisplay.UnloadContent();
            this.CurrentDisplay = display;
            this.CurrentDisplay.LoadContent();
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public void Update(GameTime gameTime)
        {
            this.CurrentDisplay.Update(gameTime);
        }

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="spriteBatch">
        /// The sprite batch.
        /// </param>
        public void Draw(SpriteBatch spriteBatch)
        {
            this.CurrentDisplay.Draw(spriteBatch);
        }
    }
}