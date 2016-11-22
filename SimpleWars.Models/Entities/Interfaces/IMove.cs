namespace SimpleWars.Models.Entities.Interfaces
{
    using Microsoft.Xna.Framework;

    using SimpleWars.Environment.Terrain.Interfaces;

    /// <summary>
    /// The Moveable interface.
    /// </summary>
    public interface IMove : IEntity
    {
        /// <summary>
        /// The move.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        /// <param name="destionation">
        /// The direction.
        /// </param>
        /// <param name="terrain">
        /// The terrain.
        /// </param>
        void Move(GameTime gameTime, Vector3 destionation, ITerrain terrain);

        /// <summary>
        /// The rotate.
        /// </summary>
        /// <param name="angle">
        /// The angle.
        /// </param>
        /// <param name="gameTime">
        /// The game Time.
        /// </param>
        void Rotate(Vector3 angle, GameTime gameTime);
    }
}