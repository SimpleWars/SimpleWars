namespace SimpleWars.Models.Entities.Interfaces
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;

    using SimpleWars.Environment.Terrain.Interfaces;
    using SimpleWars.Models.Economy.Interfaces;

    /// <summary>
    /// The Unit interface.
    /// </summary>
    public interface IUnit : IMoveable, IKillable
    {
        void Update(GameTime gameTime, ITerrain terrain, IEnumerable<IEntity> others);
    }
}