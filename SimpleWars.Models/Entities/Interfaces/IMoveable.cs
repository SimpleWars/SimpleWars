namespace SimpleWars.Models.Entities.Interfaces
{
    using Microsoft.Xna.Framework;

    using SimpleWars.Models.UtilityInterfaces;

    /// <summary>
    /// The Moveable interface.
    /// </summary>
    public interface IMoveable : IEntity
    {
        /// <summary>
        /// The move.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <param name="terrain">
        /// The terrain.
        /// </param>
        void Move(GameTime gameTime, Vector3 direction, ITerrain terrain);

        /// <summary>
        /// The rotate.
        /// </summary>
        /// <param name="angle">
        /// The angle.
        /// </param>
        void Rotate(Vector3 angle);
    }
}