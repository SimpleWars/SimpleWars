namespace SimpleWars.Models.Entities.StaticEntities
{
    using System;

    using Microsoft.Xna.Framework;

    using SimpleWars.Models.Economy.Resources;
    using SimpleWars.Models.Entities.Interfaces;

    /// <summary>
    /// The resource provider.
    /// </summary>
    public abstract class ResourceProvider : Entity, IResourceProvider
    {
        #region Constructors
        protected ResourceProvider()
            : base()
        {
        }

        protected ResourceProvider(int quantity, string resourceType, Vector3 position, float scale = 1) 
            : this(quantity, resourceType, position, Quaternion.Identity, scale)
        {
        }

        protected ResourceProvider(int quantity, string resourceType, Vector3 position, Quaternion rotation, float scale = 1) 
            : this(quantity, resourceType, position, rotation, 1f, scale)
        {
        }

        protected ResourceProvider(int quantity, string resourceType, Vector3 position, Quaternion rotation, float weight, float scale) 
            : base(position, rotation, weight, scale)
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

        public string ResourceType { get; protected set; }

        public void Gather(int amount)
        {
            int mined = this.Quantity - amount < 0 ? Math.Abs(this.Quantity - amount) : amount;

            if (this.ResourceType == typeof(Gold).Name)
            {
                this.Player.Resources.Gold.Quantity += mined;
            }
            else if (this.ResourceType == typeof(Food).Name)
            {
                this.Player.Resources.Food.Quantity += mined;
            }
            else if (this.ResourceType == typeof(Wood).Name)
            {
                this.Player.Resources.Wood.Quantity += mined;
            }
            else if (this.ResourceType == typeof(Rock).Name)
            {
                this.Player.Resources.Rock.Quantity += mined;
            }
            else if (this.ResourceType == typeof(Metal).Name)
            {
                this.Player.Resources.Metal.Quantity += mined;
            }
            else if (this.ResourceType == typeof(Population).Name)
            {
                this.Player.Resources.Population.Quantity += mined;
            }

            this.Quantity -= amount;
        }

        public void Disappear()
        {
            // No disappear logic yet
        }
        #endregion
    }
}