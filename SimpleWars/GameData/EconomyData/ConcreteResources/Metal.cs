namespace SimpleWars.GameData.EconomyData.ConcreteResources
{
    /// <summary>
    /// The metal.
    /// </summary>
    public class Metal : Resource
    {
        /// <summary>
        /// The base limit.
        /// </summary>
        private const int BaseLimit = 0;

        protected Metal()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Metal"/> class.
        /// </summary>
        /// <param name="quantity">
        /// The quantity.
        /// </param>
        public Metal(int quantity) : base(quantity, BaseLimit)
        {
        }
    }
}