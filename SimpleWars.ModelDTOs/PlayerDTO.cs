namespace SimpleWars.ModelDTOs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ProtoBuf;

    using SimpleWars.ModelDTOs.Entities;
    using SimpleWars.ModelDTOs.Resources;

    [ProtoContract]
    public class PlayerDTO
    {
        protected PlayerDTO()
        {
            this.ResourceProviders = new HashSet<ResourceProviderDTO>();
            this.Units = new HashSet<UnitDTO>();
            this.UnitsMap = new Dictionary<Guid, UnitDTO>();
            this.ResProvMap = new Dictionary<Guid, ResourceProviderDTO>();
        }

        [ProtoMember(1)]
        public Guid Id { get; set; }

        [ProtoMember(2)]
        public string Username { get; private set; }

        [ProtoMember(4)]
        public int WorldSeed { get; set; }

        [ProtoMember(7)]
        public bool LoggedIn { get; set; }

        [ProtoMember(14)]
        public virtual ResourceSetDTO ResourceSet { get; private set; }

        [ProtoMember(15)]
        public virtual ICollection<ResourceProviderDTO> ResourceProviders { get; private set; }

        [ProtoMember(16)]
        public virtual ICollection<UnitDTO> Units { get; private set; }

        public Dictionary<Guid, UnitDTO> UnitsMap { get; private set; }

        public Dictionary<Guid, ResourceProviderDTO> ResProvMap { get; private set; }

        public void MapEntites()
        {
            this.UnitsMap = this.Units.ToDictionary(u => u.Id, u => u);
            this.ResProvMap = this.ResourceProviders.ToDictionary(rp => rp.Id, rp => rp);
        }
    }
}