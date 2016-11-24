namespace SimpleWars.Models.Entities.Interfaces
{
    using System;

    public interface IFight : IEntity
    {
        IKillable Target { get; }

        int AttackSpeed { get; }

        /// <summary>
        /// Gets or sets the damage.
        /// </summary>
        int Damage { get; set; }

        float AttackRange { get; set; }

        bool TryAttack();

        void ChangeAttackTarget(IKillable target);
    }
}