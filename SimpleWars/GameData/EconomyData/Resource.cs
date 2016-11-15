namespace SimpleWars.GameData.EconomyData
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using SimpleWars.GameData.EconomyData.Interfaces;

    /// <summary>
    /// The resource.
    /// </summary>
    public abstract class Resource : IResource
    {
        #region Private Fields
        /// <summary>
        /// The quantity.
        /// </summary>
        private int quantity;

        /// <summary>
        /// The limit.
        /// </summary>
        private int limit;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Resource"/> class.
        /// </summary>
        protected Resource()
        {
        }
        

        /// <summary>
        /// Initializes a new instance of the <see cref="Resource"/> class.
        /// </summary>
        /// <param name="quantity">
        /// The quantity.
        /// </param>
        public Resource(int quantity, int limit)
        {
            this.Limit = limit;
            this.Quantity = quantity;
        }
        #endregion

        /// <summary>
        /// Gets the id.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        #region Properties
        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// </exception>
        public int Quantity
        {
            get
            {
                return this.quantity;
            }

            set
            {
                if (value < 0)
                {
                    this.Quantity = 0;
                }

                this.quantity = value > this.Limit ? this.Limit : value;
            }
        }

        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// </exception>
        public int Limit
        {
            get
            {
                return this.limit;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Resource storage limit cannot be negative");
                }

                this.limit = value;
            }
        }

        #endregion

        #region Utilities
        public override bool Equals(object obj)
        {
            if (!(obj is Resource))
            {
                throw new InvalidOperationException("You are equality comparing resource with no resource");
            }

            return this.Id == ((Resource)obj).Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
        #endregion
}