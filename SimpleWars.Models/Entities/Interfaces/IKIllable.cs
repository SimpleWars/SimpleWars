namespace SimpleWars.Models.Entities.Interfaces
{
    public interface IKillable : IEntity
    {
        int MaxHealth { get; }

        int Health { get; }

        /// <summary>
        /// Gets or sets the armor.
        /// </summary>
        int Armor { get; set; }

        bool IsAlive { get; }

        /// <summary>
        /// The take damageTaken.
        /// </summary>
        /// <param name="damageTaken">
        /// The damageTaken.
        /// </param>
        void TakeDamage(int damageTaken);
    }
}