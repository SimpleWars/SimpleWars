namespace SimpleWars.Models.Entities.DynamicEntities.BattleUnits
{
    using Microsoft.Xna.Framework;
    using SimpleWars.Assets;

    public class Swordsman : CombatUnit
    {
        private const string Dir = "Models3D";
        private const string FileName = "swordsman";

        private const int BaseMaxHealth = 100;
        private const int BaseHealth = 100;
        private const int BaseDamage = 25;
        private const int BaseArmor = 10;
        private const int BaseAttackSpeed = 20;
        private const float BaseSpeed = 5;
        private const float BaseAttackRange = 5;

        public Swordsman(Vector3 position, float scale = 1) : base(BaseMaxHealth, BaseHealth, BaseSpeed, BaseDamage, BaseArmor, BaseAttackRange, BaseAttackSpeed, position, scale)
        {
        }

        public Swordsman(Vector3 position, Quaternion rotation, float scale = 1) : base(BaseMaxHealth, BaseHealth, BaseSpeed, BaseDamage, BaseArmor, BaseAttackRange, BaseAttackSpeed, position, rotation, scale)
        {
        }

        public Swordsman(Vector3 position, Quaternion rotation, float weight = 1, float scale = 1) : base(BaseMaxHealth, BaseHealth, BaseSpeed, BaseDamage, BaseArmor, BaseAttackRange, BaseAttackSpeed, position, rotation, weight, scale)
        {
        }

        public Swordsman(Vector3 position, Quaternion rotation, int health = BaseHealth, float weight = 1, float scale = 1) 
            : base(BaseMaxHealth, health, BaseSpeed, BaseDamage, BaseArmor, BaseAttackRange, BaseAttackSpeed, position, rotation, weight, scale)
        { }

        public override void LoadModel()
        {
            this.Model = ModelsManager.Instance.GetModel(Dir, FileName);
        }
    }
}