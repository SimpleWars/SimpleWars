namespace SimpleWars.Models.Utils
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;

    using SimpleWars.Models.Entities.Interfaces;

    public static class Collision
    {
        public static bool CheckCollision(IEntity entity, IEnumerable<IEntity> others)
        {
            ICollection<BoundingSphere> boundingSpheres = new HashSet<BoundingSphere>();
            foreach (var mesh in entity.Model.Meshes)
            {
                boundingSpheres.Add(mesh.BoundingSphere.Transform(entity.TransformationMatrix));
            }

            return others.Where(other => other != entity).Any(other => other.Model.Meshes.Any(mesh => boundingSpheres.Any(bs => mesh.BoundingSphere.Transform(other.TransformationMatrix).Intersects(bs))));
        }
    }
}