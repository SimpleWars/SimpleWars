namespace SimpleWars.Factories
{
    using Microsoft.Xna.Framework;

    using SimpleWars.Extensions;
    using SimpleWars.ModelDTOs.Entities;
    using SimpleWars.Models.Entities.DynamicEntities;
    using SimpleWars.Models.Entities.DynamicEntities.BattleUnits;

    public static class UnitFactory
    {
        public static Unit FromDto(UnitDTO unitDto)
        {
            switch (unitDto.Type)
            {
                case UnitDTO.UnitType.Swordsman:
                    Vector3 pos = new Vector3(unitDto.PosX, unitDto.PosY, unitDto.PosZ);
                    Vector3 rotEuler = new Vector3(unitDto.RotX, unitDto.RotY, unitDto.RotZ);
                    Quaternion rot = rotEuler.ToQuaternion();
                    return new Swordsman(unitDto.Id, unitDto.OwnerId, pos, rot, unitDto.Weight, unitDto.Scale, unitDto.Health);
                default:
                    return null;
            }
        }

        public static UnitDTO ToDto(Unit unit)
        {
            var unitDto = new UnitDTO(
                unit.Id,
                unit.Health,
                unit.Position.X,
                unit.Position.Y,
                unit.Position.Z,
                unit.Rotation.X,
                unit.Rotation.Y,
                unit.Rotation.Z,
                unit.Scale,
                unit.Weight,
                unit.OwnerId);

            switch (unit.GetType().Name)
            {
                case "Swordsman":
                    unitDto.Type = UnitDTO.UnitType.Swordsman;
                    break;
            }

            return unitDto;
        }
    }
}
