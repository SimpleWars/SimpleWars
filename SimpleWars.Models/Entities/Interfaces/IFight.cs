namespace SimpleWars.Models.Entities.Interfaces
{
    public interface IFight : IEntity
    {
        /// <summary>
        /// Gets or sets the damage.
        /// </summary>
        int Damage { get; set; }

        float AttackRange { get; set; }

        void Attack(IKillable target);
    }
}