namespace SimpleWars.Models.Utils
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;

    using Entities.Interfaces;

    public static class Collision
    {
        /// <summary>
        /// Checks if the entity collides with any of the other entities.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="others">
        /// The other entities.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool CheckCollision(IEntity entity, IEnumerable<IEntity> others)
        {
            ICollection<BoundingSphere> boundingSpheres = new HashSet<BoundingSphere>();
            foreach (var mesh in entity.Model.Meshes)
            {
                boundingSpheres.Add(mesh.BoundingSphere.Transform(entity.TransformationMatrix));
            }

            float minCollisionRange = boundingSpheres.Average(bs => bs.Radius) * 2;

            return others
                    .Where(other =>
                    other != entity
                    && Vector3.Distance(entity.Position, other.Position) < minCollisionRange)
                    .Any(other =>
                    other.Model.Meshes
                    .Any(mesh =>
                    boundingSpheres
                    .Any(bs => mesh.BoundingSphere
                    .Transform(other.TransformationMatrix)
                    .Intersects(bs))));
        }

        /// <summary>
        /// Gets all other entities that the entity collides with.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="others">
        /// The other entities.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<IEntity> GetCollisions(
            IEntity entity, 
            IEnumerable<IEntity> others)
        {
            ICollection<BoundingSphere> boundingSpheres = new HashSet<BoundingSphere>();
            foreach (var mesh in entity.Model.Meshes)
            {
                boundingSpheres.Add(mesh.BoundingSphere.Transform(entity.TransformationMatrix));
            }

            float minCollisionRange = boundingSpheres.Max(bs => bs.Radius) * 2;

            return
                others.Where(
                    other => other != entity && Vector3.Distance(entity.Position, other.Position) < minCollisionRange)
                    .Where(
                    other =>
                    other.Model.Meshes.Any(
                    mesh =>
                    boundingSpheres
                    .Any(bs => 
                    mesh.BoundingSphere
                    .Transform(other.TransformationMatrix)
                    .Intersects(bs))));
        }

        /// <summary>
        /// Checks for collision between 2 entities
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="other">
        /// The other entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool CheckSingleCollision(IEntity entity, IEntity other)
        {
            ICollection<BoundingSphere> boundingSpheres = new HashSet<BoundingSphere>();
            foreach (var mesh in entity.Model.Meshes)
            {
                boundingSpheres.Add(mesh.BoundingSphere.Transform(entity.TransformationMatrix));
            }

            float minCollisionRange = boundingSpheres.Max(bs => bs.Radius) * 2;

            return Vector3.Distance(entity.Position, other.Position) < minCollisionRange
                && other.Model.Meshes
                .Any(mesh =>
                boundingSpheres
                .Any(bs =>
                mesh.BoundingSphere
                .Transform(other.TransformationMatrix)
                .Intersects(bs)));

        }

        /// <summary>
        /// Returns direction vector pointing at the 
        /// shortest path that the entity can take to go 
        /// around the other entity.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="other">
        /// The other entity.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        public static Vector3 GetGoRoundDirection(IMoveable entity, IEntity other)
        {
            Vector3 direction = Vector3.Normalize(Vector3.Cross(
                Vector3.Up,
                Vector3.Normalize(entity.Position - other.Position)));

            if (Vector3.Distance(entity.Position + direction, entity.Destination.Value)
                > Vector3.Distance(entity.Position, entity.Destination.Value))
            {
                direction = -direction;
            }

            return direction;
        }
    }
}