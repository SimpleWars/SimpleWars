namespace SimpleWars.Models.Economy
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using SimpleWars.ModelDTOs.Resources;
    using SimpleWars.Models.Economy.Interfaces;

    /// <summary>
    /// The resource.
    /// </summary>
    public class Resource : IResource
    {
        #region Private Fields
        /// <summary>
        /// The quantity.
        /// </summary>
        private int quantity;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Resource"/> class.
        /// </summary>
        /// <param name="quantity">
        /// The quantity.
        /// </param>
        public Resource(int quantity, ResourceType resourceType)
        {
            this.Quantity = quantity;
            this.ResourceType = resourceType;
        }
        #endregion

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
                    this.quantity = 0;
                    return;
                }

                this.quantity = value;
            }
        }

        public ResourceType ResourceType { get; set; }

        #endregion
    }
}