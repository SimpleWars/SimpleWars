namespace SimpleWars.Factories
{
    using Microsoft.Xna.Framework;

    using SimpleWars.Extensions;
    using SimpleWars.ModelDTOs.Entities;
    using SimpleWars.Models.Entities.StaticEntities;
    using SimpleWars.Models.Entities.StaticEntities.ResourceProviders;

    public static class ResProvFactory
    {
        public static ResourceProvider FromDto(ResourceProviderDTO resProvDto)
        {
            switch (resProvDto.Type)
            {
                case ResourceProviderDTO.ProviderType.Tree:
                    Vector3 pos = new Vector3(resProvDto.PosX, resProvDto.PosY, resProvDto.PosZ);
                    Vector3 rotEuler = new Vector3(resProvDto.RotX, resProvDto.RotY, resProvDto.RotZ);
                    Quaternion rot = rotEuler.ToQuaternion();
                    return new Tree(resProvDto.Id, resProvDto.OwnerId, pos, rot, resProvDto.Weight, resProvDto.Scale, resProvDto.Quantity);                  
                default:
                    return null;
            }
        }

        public static ResourceProviderDTO ToDto(ResourceProvider resProv)
        {
            return new ResourceProviderDTO(
                        resProv.Id,
                        resProv.Quantity,
                        resProv.ResourceType,
                        resProv.Position.X,
                        resProv.Position.Y,
                        resProv.Position.Z,
                        resProv.Rotation.X,
                        resProv.Rotation.Y,
                        resProv.Rotation.Z,
                        resProv.Scale,
                        resProv.Weight,
                        resProv.OwnerId);
        }
    }
}