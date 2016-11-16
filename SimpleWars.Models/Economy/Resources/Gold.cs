namespace SimpleWars.Models.Economy.Resources
{
    /// <summary>
    /// The gold.
    /// </summary>
    public class Gold : Resource
    {
        /// <summary>
        /// The base limit.
        /// </summary>
        private const int BaseLimit = 1000;

        protected Gold()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Gold"/> class.
        /// </summary>
        /// <param name="quantity">
        /// The quantity.
        /// </param>
        public Gold(int quantity)
            : base(quantity, BaseLimit)
        {
        }
    }
}