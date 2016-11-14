namespace SimpleWars.GameData.Resources
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration.Conventions;

    using SimpleWars.GameData.Resources.ConcreteResources;
    using SimpleWars.User;

    public class ResourceSet
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Gold Gold { get; set; }

        public Wood Wood { get; set; }

        public Food Food { get; set; }

        public Metal Metal { get; set; }

        public Rock Rock { get; set; }

        public Population Population { get; set; }
    }
}