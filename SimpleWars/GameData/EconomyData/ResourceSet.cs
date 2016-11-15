namespace SimpleWars.GameData.EconomyData
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using SimpleWars.GameData.EconomyData.ConcreteResources;
    using SimpleWars.GameData.EconomyData.Interfaces;

    /// <summary>
    /// The resource set.
    /// </summary>
    public class ResourceSet : IResourceSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceSet"/> class.
        /// </summary>
        public ResourceSet()
        {
            this.Gold = new Gold(0);
            this.Wood = new Wood(0);
            this.Food = new Food(0);
            this.Metal = new Metal(0);
            this.Rock = new Rock(0);
            this.Population = new Population(0);
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        /// <summary>
        /// Gets the gold.
        /// </summary>
        public Gold Gold { get; private set; }

        /// <summary>
        /// Gets the wood.
        /// </summary>
        public Wood Wood { get; private set; }

        /// <summary>
        /// Gets the food.
        /// </summary>
        public Food Food { get; private set; }

        /// <summary>
        /// Gets the metal.
        /// </summary>
        public Metal Metal { get; private set; }

        /// <summary>
        /// Gets the rock.
        /// </summary>
        public Rock Rock { get; private set; }

        /// <summary>
        /// Gets the population.
        /// </summary>
        public Population Population { get; private set; }
    }
}