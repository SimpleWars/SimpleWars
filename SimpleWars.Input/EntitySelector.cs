namespace SimpleWars.Input
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Environment.Terrain.Interfaces;
    using SimpleWars.Models.Entities.Interfaces;

    /// <summary>
    /// The entity picker.
    /// </summary>
    public static class EntitySelector
    {
        private static IEntity entityPicked;

        /// <summary>
        /// Gets or sets the entity picked.
        /// </summary>
        public static IEntity EntityPicked
        {
            get
            {
                return entityPicked;
            }

            set
            {
                entityPicked = value;
                SelectEntity(value);
            }
        }

        public static IEntity EntitySelected { get; set; }

        /// <summary>
        /// The has picked.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool HasPicked()
        {
            return EntityPicked != null;
        }

        public static bool HasSelected()
        {
            return EntitySelected != null;
        }

        /// <summary>
        /// The pick entity.
        /// </summary>
        /// <param name="projectionMatrix">
        /// The projection matrix.
        /// </param>
        /// <param name="viewMatrix">
        /// The view matrix.
        /// </param>
        /// <param name="entities">
        /// The entities.
        /// </param>
        public static void PickEntity(
            GraphicsDevice device,
            Matrix projectionMatrix, 
            Matrix viewMatrix, 
            IEnumerable<IEntity> entities)
        {
            if (HasPicked())
            {
                PlaceEntity();
            }

            EntityPicked = RayCaster.CastToEntities(device, projectionMatrix, viewMatrix, entities);

            if (HasPicked())
            {
                EntityPicked.IsHighlighted = true;
            }
        }

        public static void PickEntity(IEntity entity)
        {
            if (HasPicked())
            {
                PlaceEntity();
            }

            EntityPicked = entity;

            if (HasPicked())
            {
                EntityPicked.IsHighlighted = true;
            }
        }

        public static void SelectEntity(
            GraphicsDevice device,
            Matrix projectionMatrix,
            Matrix viewMatrix,
            IEnumerable<IEntity> entities)
        {
            if (HasSelected())
            {
                EntitySelected.IsHighlighted = false;
            }

            EntitySelected = RayCaster.CastToEntities(device, projectionMatrix, viewMatrix, entities);

            if (HasSelected())
            {
                EntitySelected.IsHighlighted = true;
            }
        }

        public static void SelectEntity(IEntity entity)
        {
            if (HasSelected())
            {
                EntitySelected.IsHighlighted = false;
            }

            EntitySelected = entity;
            if (HasSelected())
            {
                EntitySelected.IsHighlighted = true;
            }
        }

        public static void Deselect()
        {
            if (!HasPicked())
            {
                return;
            }

            EntitySelected.IsHighlighted = false;
            EntitySelected = null;
        }

        /// <summary>
        /// The place entity.
        /// </summary>
        public static void PlaceEntity()
        {
            if (!HasPicked())
            {
                return;
            }

            EntityPicked.IsHighlighted = false;
            EntityPicked = null;
        }

        /// <summary>
        /// The drag entity.
        /// </summary>
        /// <param name="projectionMatrix">
        /// The projection matrix.
        /// </param>
        /// <param name="viewMatrix">
        /// The view matrix.
        /// </param>
        /// <param name="terrain">
        /// The terrain.
        /// </param>
        public static void DragEntity(GraphicsDevice device, Matrix projectionMatrix, Matrix viewMatrix, ITerrain terrain)
        {
            if (!HasPicked())
            {
                return;
            }

            Vector3 position = RayCaster.GetTerrainPoint(device, projectionMatrix, viewMatrix, terrain);

            EntityPicked.Position = position;
        }
    }
}