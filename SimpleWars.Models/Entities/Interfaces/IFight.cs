namespace SimpleWars.Models.Entities.Interfaces
{
    using System;

    using Microsoft.Xna.Framework;

    public interface IFight : IEntity
    {
        IKillable Target { get; }

        int AttackSpeed { get; }

        /// <summary>
        /// Gets or sets the damage.
        /// </summary>
        int Damage { get; set; }

        float AttackRange { get; set; }

        void TryAttack();

        void ChangeAttackTarget(IKillable target);
    }
}