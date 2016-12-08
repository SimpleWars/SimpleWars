﻿namespace ModelDTOs.Resources
{
    using ProtoBuf;

    [ProtoContract]
    public class ResourceDTO
    {
        protected ResourceDTO()
        {
        }

        [ProtoMember(1)]
        public int Id { get; protected set; }

        [ProtoMember(2)]
        public int Quantity { get; set; }

        [ProtoMember(3)]
        public ResourceType ResourceType { get; protected set; } 
    }
}