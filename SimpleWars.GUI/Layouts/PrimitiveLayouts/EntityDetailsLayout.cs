namespace SimpleWars.GUI.Layouts.PrimitiveLayouts
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Assets;
    using SimpleWars.GUI.Interfaces;
    using SimpleWars.GUI.PrimitiveComponents;
    using SimpleWars.Input;
    using SimpleWars.Models.Entities.DynamicEntities.BattleUnits;
    using SimpleWars.Models.Entities.Interfaces;
    using SimpleWars.Utils;

    public enum DetailCommand
    {
        None = 0,
        Close = 1,
        PickEntity = 2,
        GatherResource = 3,
        CommandMovement = 4,
        DestroyEntity = 5
    }

    public class EntityDetailsLayout : Layout
    {
        public EntityDetailsLayout(IEntity entity, Texture2D background, Vector3 projectedPosition)
            : base(background)
        {
            this.Entity = entity;

            this.Position = new Vector2(projectedPosition.X, projectedPosition.Y);
            this.SerializeEntity(entity);
        }

        public IEntity Entity { get; }

        public DetailCommand Command { get; private set; }

        public void AdjustPosition(Vector2 position)
        {
            Vector2 offset = this.Position - position;

            this.Position = position;

            foreach (var textNode in this.TextNodes)
            {
                textNode.Position -= offset;
            }

            foreach (var button in this.Buttons)
            {
                button.Position -= offset;
                button.TextNode.Position -= offset;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Input.LeftMouseClick())
            {
                float mouseX = Input.MousePos.X;
                float mouseY = Input.MousePos.Y;

                if (!(mouseX >= this.Position.X && mouseX <= this.Position.X + this.Background.Width * this.Dimensions.X
                && mouseY >= this.Position.Y && mouseY <= this.Position.Y + this.Background.Height * this.Dimensions.Y))
                {
                    this.Command = DetailCommand.Close;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private void SerializeEntity(IEntity entity)
        {
            var entityType = entity.GetType();
            string providerName;

            if (entityType.BaseType != null && entityType.BaseType.IsAbstract)
            {
                providerName = entityType.Name;
            }
            else if (entityType.BaseType != null)
            {
                providerName = entityType.BaseType.Name;
            }
            else
            {
                providerName = "Not specified";
            }

            var typeTextNode = new TextNode(
                this,
                new Vector2(20, 20),
                Vector2.One,
                providerName,
                SpriteFontManager.Instance.GetFont("Arial_18"),
                Color.White);

            this.TextNodes.Add(typeTextNode);

            if (entity is IResourceProvider)
            {
                this.Dimensions = new Vector2(240, 150);
                this.SerializeResourceProvider((IResourceProvider)entity);
            }
            else if (entity is ICombatUnit)
            {
                this.Dimensions = new Vector2(240, 200);
                this.SerializeCombatUnit((ICombatUnit)entity);
            }
        }

        private void SerializeCombatUnit(ICombatUnit unit)
        {
           string healthToDisplay = "Health: " + unit.Health + "/" + unit.MaxHealth;
            var healthTextNode = new TextNode(
                this,
                new Vector2(20, 50),
                Vector2.One,
                healthToDisplay,
                SpriteFontManager.Instance.GetFont("Arial_18"),
                Color.White);

            string damageDisplay = "Damage: " + unit.Damage;
            var damageTextNode = new TextNode(
                this,
                new Vector2(20, 80),
                Vector2.One,
                damageDisplay,
                SpriteFontManager.Instance.GetFont("Arial_18"),
                Color.White);

            string armorDisplay = "Armor: " + unit.Armor;
            var armorTextNode = new TextNode(
                this,
                new Vector2(20, 110),
                Vector2.One,
                armorDisplay,
                SpriteFontManager.Instance.GetFont("Arial_18"),
                Color.White);

            string attackRangeDisplay = "Range: " + unit.AttackRange;
            var attackRangeTextNode = new TextNode(
                this,
                new Vector2(20, 140),
                Vector2.One,
                attackRangeDisplay,
                SpriteFontManager.Instance.GetFont("Arial_18"),
                Color.White);

            this.TextNodes.Add(healthTextNode);
            this.TextNodes.Add(damageTextNode);
            this.TextNodes.Add(armorTextNode);
            this.TextNodes.Add(attackRangeTextNode);
        }

        private void SerializeResourceProvider(IResourceProvider provider)
        {
            var resourceType = provider.ResourceType;
            var resourceQuantity = provider.Quantity;

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

            this.TextNodes.Add(resourceTypeTextNode);
            this.TextNodes.Add(resourceQuantityTextNode);

            var pickProvider = new Button(
                this.Position + new Vector2(20, 110),
                PointTextures.TransparentGrayPoint,
                new Vector2(80, 30),
                Color.Black,
                2,
                () =>
                    {
                        this.Command = DetailCommand.PickEntity;
                    });

            var pickProviderTextNode = new TextNode(pickProvider, new Vector2(20, 0), Vector2.One, "Pick", SpriteFontManager.Instance.GetFont("Arial_18"), Color.White);
            pickProvider.TextNode = pickProviderTextNode;

            var gatherProvider = new Button(
                this.Position + new Vector2(110, 110),
                PointTextures.TransparentGrayPoint,
                new Vector2(110, 30),
                Color.Black, 
                2,
                () =>
                {
                    this.Command = DetailCommand.GatherResource;
                });

            var gatherProviderTextNode = new TextNode(gatherProvider, new Vector2(20, 0), Vector2.One, "Gather", SpriteFontManager.Instance.GetFont("Arial_18"), Color.White);

            gatherProvider.TextNode = gatherProviderTextNode;

            this.Buttons.Add(pickProvider);
            this.Buttons.Add(gatherProvider);
        }
    }
}