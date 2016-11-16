namespace SimpleWars.Models.Economy.Resources
{
    /// <summary>
    /// The rock.
    /// </summary>
    public class Rock : Resource
    {
        /// <summary>
        /// The base limit.
        /// </summary>
        private const int BaseLimit = 500;

        protected Rock()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rock"/> class.
        /// </summary>
        /// <param name="quantity">
        /// The quantity.
        /// </param>
        public Rock(int quantity) : base(quantity, BaseLimit)
        {
        }
    }
}