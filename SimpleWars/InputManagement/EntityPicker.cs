namespace SimpleWars.InputManagement
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.Xna.Framework;

    using SimpleWars.Entities;
    using SimpleWars.Terrain;

    /// <summary>
    /// The entity picker.
    /// </summary>
    public static class EntityPicker
    {
        /// <summary>
        /// Gets the entity picked.
        /// </summary>
        public static Entity EntityPicked { get; private set; }

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
            Matrix projectionMatrix, 
            Matrix viewMatrix, 
            IEnumerable<Entity> entities)
        {
            EntityPicked = RayCaster.CastToEntities(projectionMatrix, viewMatrix, entities);
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
        public static void DragEntity(Matrix projectionMatrix, Matrix viewMatrix, Terrain terrain)
        {
            if (!HasPicked())
            {
                return;
            }

            Vector3 position = RayCaster.GetTerrainPoint(projectionMatrix, viewMatrix, terrain);

            EntityPicked.Position = position;
        }
    }
}