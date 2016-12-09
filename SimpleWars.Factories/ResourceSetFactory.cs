namespace SimpleWars.Factories
{
    using SimpleWars.ModelDTOs.Resources;
    using SimpleWars.Models.Economy;

    public static class ResourceSetFactory
    {
        public static ResourceSet FromDto(ResourceSetDTO resSetDto)
        {
            return new ResourceSet(resSetDto.Id, new Resource(resSetDto.Gold.Quantity, ResourceType.Gold), new Resource(resSetDto.Wood.Quantity, ResourceType.Wood), new Resource(resSetDto.Food.Quantity, ResourceType.Food), new Resource(resSetDto.Rock.Quantity, ResourceType.Rock), new Resource(resSetDto.Metal.Quantity, ResourceType.Metal), new Resource(resSetDto.Population.Quantity, ResourceType.Population));
        }
    }
}