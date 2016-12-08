namespace SimpleWars.Models.Users.Interfaces
{
    using System.Collections.Generic;
    using SimpleWars.Models.Economy;
    using SimpleWars.Models.Entities.DynamicEntities;
    using SimpleWars.Models.Entities.StaticEntities;

    /// <summary>
    /// The Player interface.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Gets the home seed.
        /// </summary>
        int HomeSeed { get; }

        /// <summary>
        /// Gets the resource set.
        /// </summary>
        ResourceSet ResourceSet { get; }

        /// <summary>
        /// Gets the static entities.
        /// </summary>
        ICollection<ResourceProvider> ResourceProviders { get; }

        /// <summary>
        /// Gets the units.
        /// </summary>
        ICollection<Unit> Units { get; }
    }
}