using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    return new Swordsman(pos, rot, unitDto.Health, unitDto.Weight, unitDto.Scale);
                default:
                    return null;
            }
        }
    }
}
