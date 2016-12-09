namespace SimpleWars.Models.Entities.StaticEntities
{
    using System;

    using Microsoft.Xna.Framework;

    using SimpleWars.ModelDTOs.Resources;
    using SimpleWars.Models.Entities.Interfaces;

    /// <summary>
    /// The resource provider.
    /// </summary>
    public abstract class ResourceProvider : Entity, IResourceProvider
    {
        #region Constructors
        protected ResourceProvider(Guid id, Guid ownerId, int quantity, ResourceType resourceType, Vector3 position, Quaternion rotation, float weight, float scale) 
            : base(id, ownerId, position, rotation, weight, scale)
        {
            this.Quantity = quantity;
            this.ResourceType = resourceType;
        }
        #endregion

        #region Resource Provider Implementation
        private int quantity;

        public int Quantity
        {
            get
            {
                return this.quantity;
            }

            set
            {
                if (value <= 0)
                {
                    this.quantity = 0;
                    this.Disappear();
                    return;
                }

                this.quantity = value;
            }
        }

        public ResourceType ResourceType { get; set; }

        public void Gather(int amount)
        {
            int mined = this.Quantity - amount < 0 ? Math.Abs(this.Quantity - amount) : amount;

            this.Quantity -= amount;
        }

        public void Disappear()
        {
            // No disappear logic yet
        }
        #endregion
    }
}