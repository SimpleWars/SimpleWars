namespace SimpleWars.GUI.Layouts.PrimitiveLayouts
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Assets;
    using SimpleWars.GUI.Interfaces;
    using SimpleWars.GUI.PrimitiveComponents;
    using SimpleWars.Input;
    using SimpleWars.Models.Entities.Interfaces;

    public class EntityDetailsLayout : Layout
    {
        public EntityDetailsLayout(IEntity entity, Texture2D background, Vector3 projectedPosition)
            : base(background)
        {
            this.Entity = entity;
            // Just placeholder values for now. Will be properly calculated.
            this.Dimensions = new Vector2(150, 250);
            this.Position = new Vector2(projectedPosition.X, projectedPosition.Y - 300);
            this.SerializeEntity(entity);
        }

        public IEntity Entity { get; }

        public void AdjustPosition(Vector2 position)
        {
            Vector2 offset = this.Position - position;

            this.Position = position;

            foreach (var textNode in this.TextNodes)
            {
                textNode.Position -= offset;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private void SerializeEntity(IEntity entity)
        {
            if (entity is IResourceProvider)
            {
                var provider = (IResourceProvider)entity;
                var providerType = provider.GetType().BaseType;
                var providerName = providerType?.Name ?? "Not specified";
                var resourceType = provider.ResourceType;
                var resourceQuantity = provider.Quantity;

                var typeTextNode = new TextNode(
                    this,
                    new Vector2(20, 20),
                    Vector2.One,
                    providerName,
                    SpriteFontManager.Instance.GetFont("Arial_18"),
                    Color.White);

                var resourceTypeTextNode = new TextNode(
                    this,
                    new Vector2(20, 50),
                    Vector2.One,
                    resourceType,
                    SpriteFontManager.Instance.GetFont("Arial_18"),
                    Color.White);

                var resourceQuantityTextNode = new TextNode(
                    this,
                    new Vector2(20, 80),
                    Vector2.One,
                    resourceQuantity.ToString(),
                    SpriteFontManager.Instance.GetFont("Arial_18"),
                    Color.White);

                this.TextNodes.Add(typeTextNode);
                this.TextNodes.Add(resourceTypeTextNode);
                this.TextNodes.Add(resourceQuantityTextNode);
            } 
            else if (entity is IUnit)
            {
                var unit = (IUnit)entity;
            }
        }
    }
}