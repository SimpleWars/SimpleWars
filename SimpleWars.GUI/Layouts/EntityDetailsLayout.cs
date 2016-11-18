namespace SimpleWars.GUI.Layouts
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Data.Contexts;
    using SimpleWars.GUI.Interfaces;
    using SimpleWars.Models.Entities.Interfaces;
    using SimpleWars.Input;

    public class EntityDetailsLayout : ILayout
    {
        public EntityDetailsLayout(IEntity entity, GraphicsDevice device)
        {
            this.Buttons = new HashSet<IButton>();
            this.TextBoxes = new HashSet<ITextBox>();

            this.Background = new Texture2D(device, 1, 1);
            this.Background.SetData<Color>(new Color[] { Color.Black });

            // Just placeholder values for now. Will be properly calculated.
            this.Dimensions = new Vector2(150, 250);
            this.Position = new Vector2(Input.MousePos.X, Input.MousePos.Y - 50);

            this.SerializeEntity(entity);
        }

        public ICollection<IButton> Buttons { get; }

        public ICollection<ITextBox> TextBoxes { get; }

        public Texture2D Background { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Dimensions { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Implement Draw
        }

        public void Update(GameTime gameTime, GameContext context)
        {
            // TODO: Implement Update Logic
        }

        private void SerializeEntity(IEntity entity)
        {
            // TODO: Implement serialization to fill buttons and show entity specific text
        }
    }
}