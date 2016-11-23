namespace SimpleWars.Models.Entities.DynamicEntities.BattleUnits
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Assets;
    using SimpleWars.Models.Entities.Interfaces;
    using SimpleWars.Utils;

    public class Swordsman : CombatUnit
    {
        private const string Dir = "Models3D";
        private const string FileName = "swordsman";

        private const int BaseMaxHealth = 100;
        private const int BaseHealth = 100;
        private const int BaseDamage = 25;
        private const int BaseArmor = 10;
        private const float BaseSpeed = 5;
        private const float BaseAttackRange = 5;

        protected Swordsman()
        {
            this.MaxHealth = BaseMaxHealth;
            this.Speed = BaseSpeed;
            this.Damage = BaseDamage;
            this.Armor = BaseArmor;
            this.AttackRange = BaseAttackRange;
        }

        public Swordsman(Vector3 position, float scale = 1) : base(BaseMaxHealth, BaseHealth, BaseSpeed, BaseDamage, BaseArmor, BaseAttackRange, position, scale)
        {
        }

        public Swordsman(Vector3 position, Vector3 rotation, float scale = 1) : base(BaseMaxHealth, BaseHealth, BaseSpeed, BaseDamage, BaseArmor, BaseAttackRange, position, rotation, scale)
        {
        }

        public Swordsman(Vector3 position, Vector3 rotation, float weight = 1, float scale = 1) : base(BaseMaxHealth, BaseHealth, BaseSpeed, BaseDamage, BaseArmor, BaseAttackRange, position, rotation, weight, scale)
        {
        }

        public override void LoadModel()
        {
            this.Model = ModelsManager.Instance.GetModel(Dir, FileName);
        }
    }
}