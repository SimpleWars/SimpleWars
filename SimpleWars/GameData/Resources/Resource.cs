namespace SimpleWars.GameData.Resources
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class Resource
    {
        private int quantity;

        protected Resource()
        {
        }

        public Resource(ResourceType type, int quantity)
        {
            this.Quantity = quantity;
            this.Type = type;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        public ResourceType Type { get; private set; }

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

        public override bool Equals(object obj)
        {
            if (!(obj is Resource))
            {
                throw new InvalidOperationException("You are equality comparing resource with no resource");
            }

            return this.Type == ((Resource)obj).Type;
        }

        public override int GetHashCode()
        {
            return this.Type.GetHashCode();
        }
    }
}