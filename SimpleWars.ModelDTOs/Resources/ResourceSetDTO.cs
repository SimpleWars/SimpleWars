namespace SimpleWars.ModelDTOs.Resources
{
    using System;

    using ProtoBuf;

    [ProtoContract]
    public class ResourceSetDTO
    {
        protected ResourceSetDTO()
        {
        }

        public ResourceSetDTO(Guid ownerId, ResourceDTO gold, ResourceDTO wood, ResourceDTO food, ResourceDTO rock, ResourceDTO metal, ResourceDTO population)
        {
            this.Id = ownerId;
            this.Gold = gold;
            this.Wood = wood;
            this.Food = food;
            this.Rock = rock;
            this.Metal = metal;
            this.Population = population;
        }

        [ProtoMember(7)]
        public Guid Id { get; set; }

        [ProtoMember(1)]
        public virtual ResourceDTO Gold { get; private set; }

        [ProtoMember(2)]
        public virtual ResourceDTO Wood { get; private set; }

        [ProtoMember(3)]
        public virtual ResourceDTO Food { get; private set; }

        [ProtoMember(4)]
        public virtual ResourceDTO Rock { get; private set; }

        [ProtoMember(5)]
        public virtual ResourceDTO Metal { get; private set; }

        [ProtoMember(6)]
        public virtual ResourceDTO Population { get; private set; }
    }
}