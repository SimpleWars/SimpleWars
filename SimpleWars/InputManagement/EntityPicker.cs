namespace SimpleWars.InputManagement
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.Xna.Framework;

    using SimpleWars.Entities;
    using SimpleWars.Terrain;

    public static class EntityPicker
    {
        public static Entity EntityPicked { get; private set; }

        public static bool HasPicked()
        {
            return EntityPicked != null;
        }

        public static void PickEntity(
            Matrix projectionMatrix, 
            Matrix viewMatrix, 
            IEnumerable<Entity> entities)
        {
            Debug.WriteLine("Attempt to pick");
            EntityPicked = RayCaster.CastToEntities(projectionMatrix, viewMatrix, entities);
        }

        public static void PlaceEntity()
        {
            if (!HasPicked())
            {
                return;
            }

            Debug.WriteLine("Place");

            EntityPicked = null;
        }

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