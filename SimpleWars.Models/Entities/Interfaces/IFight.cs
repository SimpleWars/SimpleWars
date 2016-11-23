namespace SimpleWars.Models.Entities.Interfaces
{
    public interface IFight : IEntity
    {
        IKillable Target { get; }

        /// <summary>
        /// Gets or sets the damage.
        /// </summary>
        int Damage { get; set; }

        float AttackRange { get; set; }

        void TryAttack();

        void ChangeAttackTarget(IKillable target);
    }
}