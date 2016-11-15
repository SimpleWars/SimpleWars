namespace SimpleWars.GameData.EconomyData.ConcreteResources
{
    /// <summary>
    /// The food.
    /// </summary>
    public class Food : Resource
    {
        /// <summary>
        /// The base limit.
        /// </summary>
        private const int BaseLimit = 50;

        protected Food()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Food"/> class.
        /// </summary>
        /// <param name="quantity">
        /// The quantity.
        /// </param>
        public Food(int quantity) : base(quantity, BaseLimit)
        {
        }
    }
}