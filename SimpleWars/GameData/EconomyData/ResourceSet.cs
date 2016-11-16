namespace SimpleWars.GameData.EconomyData
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using SimpleWars.GameData.EconomyData.ConcreteResources;
    using SimpleWars.GameData.EconomyData.Interfaces;
    using SimpleWars.User;

    /// <summary>
    /// The resource set.
    /// </summary>
    public class ResourceSet : IResourceSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceSet"/> class.
        /// </summary>
        protected ResourceSet()
        {            
        }

        public ResourceSet(bool initNew)
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
        [Key, ForeignKey("Player")]
        public int Id { get; private set; }

        /// <summary>
        /// Gets the gold.
        /// </summary>
        public virtual Gold Gold { get; private set; }

        /// <summary>
        /// Gets the wood.
        /// </summary>
        public virtual Wood Wood { get; private set; }

        /// <summary>
        /// Gets the food.
        /// </summary>
        public virtual Food Food { get; private set; }

        /// <summary>
        /// Gets the metal.
        /// </summary>
        public virtual Metal Metal { get; private set; }


        /// <summary>
        /// Gets the rock.
        /// </summary>
        public virtual Rock Rock { get; private set; }

        /// <summary>
        /// Gets the population.
        /// </summary>
        public virtual Population Population { get; private set; }

        public virtual Player Player { get; private set; }
    }
}