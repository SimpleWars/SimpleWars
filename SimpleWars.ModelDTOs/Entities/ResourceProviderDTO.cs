namespace SimpleWars.ModelDTOs.Entities
{
    using System;

    using ProtoBuf;

    using SimpleWars.ModelDTOs.Resources;

    [ProtoContract]
    public class ResourceProviderDTO : EntityDTO
    {
        public enum ProviderType
        {
            Tree
        }

        protected ResourceProviderDTO()
        {
        }

        public ResourceProviderDTO(
            int quantity,
            ResourceType resourceType,
            float posX,
            float posY,
            float posZ,
            float rotX,
            float rotY,
            float rotZ,
            float scale,
            float weight,
            Guid ownerId)
            : base(posX, posY, posZ, rotX, rotY, rotZ, scale, weight, ownerId)
        {
            this.Quantity = quantity;
            this.ResourceType = resourceType;
        }

        [ProtoMember(13)]
        public int Quantity { get; set; }

        [ProtoMember(14)]
        public ResourceType ResourceType { get; set; }

        public bool Depleted => this.Quantity > 0;

        [ProtoMember(16)]
        public ProviderType Type { get; set; }
    }
}