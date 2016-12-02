namespace SimpleWars.Models.Users.Interfaces
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.Xna.Framework;

    using SimpleWars.Models.Economy;
    using SimpleWars.Models.Entities;
    using SimpleWars.Models.Entities.DynamicEntities;
    using SimpleWars.Models.Entities.Interfaces;
    using SimpleWars.Models.Entities.StaticEntities;

    /// <summary>
    /// The Player interface.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int Id { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        [Required]
        string Username { get; }

        /// <summary>
        /// Gets the hashed password.
        /// </summary>
        [Required]
        string HashedPassword { get; }

        /// <summary>
        /// Gets the home seed.
        /// </summary>
        int HomeSeed { get; }

        /// <summary>
        /// Gets the world map pos.
        /// </summary>
        
        Vector2 WorldMapPos { get; }

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