namespace SimpleWars.Models.Entities.Interfaces
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;

    using SimpleWars.Environment.Terrain.Interfaces;

    /// <summary>
    /// The Moveable interface.
    /// </summary>
    public interface IMoveable : IEntity
    {
        Vector3? Destination { get; set; }

        float Speed { get; }

        /// <summary>
        /// The move.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        /// <param name="terrain">
        /// The terrain.
        /// </param>
        void Move(GameTime gameTime, ITerrain terrain, IEnumerable<IEntity> others);

        /// <summary>
        /// Rotates around the world up axis.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        /// <param name="angle">
        /// The angle.
        /// </param>
        void Rotate(GameTime gameTime, float angle);
    }
}