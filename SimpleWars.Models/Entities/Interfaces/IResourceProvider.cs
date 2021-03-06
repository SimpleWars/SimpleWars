﻿namespace SimpleWars.Models.Entities.Interfaces
{
    using SimpleWars.ModelDTOs.Resources;

    /// <summary>
    /// The ResourceProvider interface.
    /// </summary>
    public interface IResourceProvider : IEntity
    {
        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        int Quantity { get; set; }

        /// <summary>
        /// Gets the resource type.
        /// </summary>
        ResourceType ResourceType { get; }

        /// <summary>
        /// The gather.
        /// </summary>
        /// <param name="amount">
        /// The amount.
        /// </param>
        void Gather(int amount);

        /// <summary>
        /// The disappear.
        /// </summary>
        void Disappear();
    }
}