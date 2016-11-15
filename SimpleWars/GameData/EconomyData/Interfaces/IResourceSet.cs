namespace SimpleWars.GameData.EconomyData.Interfaces
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using SimpleWars.GameData.EconomyData.ConcreteResources;

    /// <summary>
    /// The ResourceSet interface.
    /// </summary>
    public interface IResourceSet
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int Id { get; }

        /// <summary>
        /// Gets the gold.
        /// </summary>
        Gold Gold { get; }

        /// <summary>
        /// Gets the wood.
        /// </summary>
        Wood Wood { get; }

        /// <summary>
        /// Gets the food.
        /// </summary>
        Food Food { get; }

        /// <summary>
        /// Gets the rock.
        /// </summary>
        Rock Rock { get; }

        /// <summary>
        /// Gets the metal.
        /// </summary>
        Metal Metal { get; }

        /// <summary>
        /// Gets the population.
        /// </summary>
        Population Population { get; }
    }
}