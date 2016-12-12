namespace SimpleWars.Models.Economy
{
    using System;

    using Interfaces;

    /// <summary>
    /// The resource set.
    /// </summary>
    public class ResourceSet : IResourceSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceSet"/> class.
        /// </summary>
        public ResourceSet(Guid ownerId, IResource gold, IResource wood, IResource food, IResource rock, IResource metal, IResource population)
        {
            this.OwnerId = ownerId;
            this.Gold = gold;
            this.Wood = wood;
            this.Food = food;
            this.Rock = rock;
            this.Metal = metal;
            this.Population = population;
        }
        /// <summary>
        /// The owner Id.
        /// Its a guid
        /// </summary>
        public Guid OwnerId { get; private set; }

        /// <summary>
        /// Gets the gold.
        /// </summary>
        public IResource Gold { get; private set; }

        /// <summary>
        /// Gets the wood.
        /// </summary>
        public IResource Wood { get; private set; }

        /// <summary>
        /// Gets the food.
        /// </summary>
        public IResource Food { get; private set; }

        /// <summary>
        /// Gets the metal.
        /// </summary>
        public IResource Metal { get; private set; }


        /// <summary>
        /// Gets the rock.
        /// </summary>
        public IResource Rock { get; private set; }

        /// <summary>
        /// Gets the population.
        /// </summary>
        public IResource Population { get; private set; }
    }
}