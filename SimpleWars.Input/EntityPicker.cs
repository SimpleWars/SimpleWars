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
    public static class EntityPicker
    {
        /// <summary>
        /// Gets the entity picked.
        /// </summary>
        public static IEntity EntityPicked { get; set; }

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
            EntityPicked = RayCaster.CastToEntities(device, projectionMatrix, viewMatrix, entities);
            if (HasPicked())
            {
                EntityPicked.IsHighlighted = true;
            }
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