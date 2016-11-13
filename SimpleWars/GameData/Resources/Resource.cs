namespace SimpleWars.GameData.Resources
{
    using System;

    public class Resource
    {
        private int quantity;

        public Resource(ResourceType type, int quantity)
        {
            this.Quantity = quantity;
            this.Type = type;
        }

        public ResourceType Type { get; }

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
                    throw new ArgumentException("Resource quantity cannot be negative!");
                }

                this.quantity = value;
            }
        }
    }
}