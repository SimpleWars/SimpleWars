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
        public static Tuple<bool, Vector3?> CheckCollision(IEntity entity, IEnumerable<IEntity> others)
        {
            ICollection<BoundingSphere> boundingSpheres = new HashSet<BoundingSphere>();
            foreach (var mesh in entity.Model.Meshes)
            {
                boundingSpheres.Add(mesh.BoundingSphere.Transform(entity.TransformationMatrix));
            }

            float minCollisionRange = boundingSpheres.Average(bs => bs.Radius) * 2;

            IEntity collided = others
                    .Where(other => 
                    other != entity 
                    && Vector3.Distance(entity.Position, other.Position) < minCollisionRange)
                    .FirstOrDefault(other => 
                    other.Model.Meshes
                    .Any(mesh => 
                    boundingSpheres
                    .Any(bs => mesh.BoundingSphere.Transform(other.TransformationMatrix)
                    .Intersects(bs))));

            if (collided != null && entity is IMoveable)
            {
                IMoveable movingEntity = (IMoveable)entity;
                if (movingEntity.Destination != null)
                {
                        Vector3 direction = Vector3.Normalize(Vector3.Cross(
                            Vector3.Up,
                            Vector3.Normalize(movingEntity.Position - collided.Position)));
                        direction.Y = 0;
              

                    if (Vector3.Distance(movingEntity.Position + direction, movingEntity.Destination.Value)
                        > Vector3.Distance(movingEntity.Position, movingEntity.Destination.Value))
                    {
                        direction = -direction;
                    }

                    return new Tuple<bool, Vector3?>(true, direction);
                }
            }
            else if (collided != null)
            {
                return new Tuple<bool, Vector3?>(true, null);
            }

            return new Tuple<bool, Vector3?>(false, null);
        }
    }
}