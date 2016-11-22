namespace SimpleWars.Models.Entities.Interfaces
{
    using System.Collections.Generic;

    using SimpleWars.Models.Economy.Interfaces;

    /// <summary>
    /// The Unit interface.
    /// </summary>
    public interface IUnit : IMove, IKillable
    {
    }
}