namespace SimpleWars.Models.Utils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;

    using SimpleWars.Models.Entities.Interfaces;

    public static class Collision
    {
        public static Tuple<bool, float> CheckCollision(IEntity entity, IEnumerable<IEntity> others)
        {
            ICollection<BoundingSphere> boundingSpheres = new HashSet<BoundingSphere>();
            foreach (var mesh in entity.Model.Meshes)
            {
                boundingSpheres.Add(mesh.BoundingSphere.Transform(entity.TransformationMatrix));
            }

            float minCollisionRange = boundingSpheres.Average(bs => bs.Radius) * 2;

            IEntity collisionOther = others
                .Where(other => other != entity 
                    && Vector3.Distance(entity.Position, other.Position) < minCollisionRange)
                    .FirstOrDefault(other => 
                    other.Model.Meshes
                    .Any(mesh => boundingSpheres
                    .Any(bs => mesh.BoundingSphere.Transform(other.TransformationMatrix)
                    .Intersects(bs))));

            if (collisionOther != null)
            {
                Vector3 positionChecked = entity.Position;
                Vector3 positionOther = collisionOther.Position;

                float angle =
                    (float)Math.Acos(Vector3.Dot(Vector3.Normalize(positionChecked), Vector3.Normalize(positionOther)));

                return new Tuple<bool, float>(true, angle);
            }

            return new Tuple<bool, float>(false, 0);
        }
    }
}