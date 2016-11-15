namespace SimpleWars.GameData.EconomyData.ConcreteResources
{
    /// <summary>
    /// The wood.
    /// </summary>
    public class Wood : Resource
    {
        /// <summary>
        /// The base limit.
        /// </summary>
        private const int BaseLimit = 500;

        protected Wood()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wood"/> class.
        /// </summary>
        /// <param name="quantity">
        /// The quantity.
        /// </param>
        public Wood(int quantity) : base(quantity, BaseLimit)
        {
        }
    }
}