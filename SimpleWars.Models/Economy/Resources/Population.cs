namespace SimpleWars.Models.Economy.Resources
{
    /// <summary>
    /// The population.
    /// </summary>
    public class Population : Resource
    {
        /// <summary>
        /// The base limit.
        /// </summary>
        private const int BaseLimit = 25;

        protected Population()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Population"/> class.
        /// </summary>
        /// <param name="quantity">
        /// The quantity.
        /// </param>
        public Population(int quantity) : base(quantity, BaseLimit)
        {
        }
    }
}