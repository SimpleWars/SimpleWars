namespace SimpleWars.Factories
{
    using ModelDTOs.Resources;
    using Models.Economy;

    public static class ResourceSetFactory
    {
        public static ResourceSet FromDto(ResourceSetDTO resSetDto)
        {
            return new ResourceSet(resSetDto.Id, new Resource(resSetDto.Gold.Quantity, ResourceType.Gold), new Resource(resSetDto.Wood.Quantity, ResourceType.Wood), new Resource(resSetDto.Food.Quantity, ResourceType.Food), new Resource(resSetDto.Rock.Quantity, ResourceType.Rock), new Resource(resSetDto.Metal.Quantity, ResourceType.Metal), new Resource(resSetDto.Population.Quantity, ResourceType.Population));
        }

        public static ResourceSetDTO ToDto(ResourceSet resSet)
        {
            return new ResourceSetDTO(resSet.OwnerId, new ResourceDTO(resSet.Gold.Id, resSet.Gold.Quantity, ResourceType.Gold), new ResourceDTO(resSet.Wood.Id, resSet.Wood.Quantity, ResourceType.Wood), new ResourceDTO(resSet.Food.Id, resSet.Food.Quantity, ResourceType.Food), new ResourceDTO(resSet.Rock.Id, resSet.Rock.Quantity, ResourceType.Rock), new ResourceDTO(resSet.Metal.Id, resSet.Metal.Quantity, ResourceType.Metal), new ResourceDTO(resSet.Population.Id, resSet.Population.Quantity, ResourceType.Population));
        }
    }
}