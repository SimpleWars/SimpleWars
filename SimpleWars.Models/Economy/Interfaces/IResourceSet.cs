namespace SimpleWars.Models.Economy.Interfaces
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The ResourceSet interface.
    /// </summary>
    public interface IResourceSet
    {
        Guid OwnerId { get; }

        /// <summary>
        /// Gets the gold.
        /// </summary>
        IResource Gold { get; }

        /// <summary>
        /// Gets the wood.
        /// </summary>
        IResource Wood { get; }

        /// <summary>
        /// Gets the food.
        /// </summary>
        IResource Food { get; }

        /// <summary>
        /// Gets the rock.
        /// </summary>
        IResource Rock { get; }

        /// <summary>
        /// Gets the metal.
        /// </summary>
        IResource Metal { get; }

        /// <summary>
        /// Gets the population.
        /// </summary>
        IResource Population { get; }
    }
}