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
        public ResourceSet()
        {
            this.Gold = new Gold(0);
            this.Wood = new Wood(0);
            this.Food = new Food(0);
            this.Metal = new Metal(0);
            this.Rock = new Rock(0);
            this.Population = new Population(0);
        }

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