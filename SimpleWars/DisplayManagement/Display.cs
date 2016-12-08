namespace SimpleWars.DisplayManagement
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    
    using SimpleWars.DisplayManagement.Interfaces;
    using SimpleWars.GUI.Interfaces;

    /// <summary>
    /// The game display.
    /// </summary>
    public abstract class Display : IDisplay
    {
        protected Display()
        {
            this.Guis = new List<IGui>();
        }

        public ICollection<IGui> Guis { get; set; }

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
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var gui in this.Guis)
            {
                gui.Draw(spriteBatch);
            }
        }
    }
}