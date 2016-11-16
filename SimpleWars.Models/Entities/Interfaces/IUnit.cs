namespace SimpleWars.Models.Entities.Interfaces
{
    /// <summary>
    /// The Unit interface.
    /// </summary>
    public interface IUnit : IMoveable
    {
        /// <summary>
        /// Gets or sets the health.
        /// </summary>
        int Health { get; set; }

        /// <summary>
        /// Gets or sets the damage.
        /// </summary>
        int Damage { get; set; }

        /// <summary>
        /// Gets or sets the armor.
        /// </summary>
        int Armor { get; set; }

        /// <summary>
        /// The take damage.
        /// </summary>
        /// <param name="damage">
        /// The damage.
        /// </param>
        void TakeDamage(int damage);

        /// <summary>
        /// The die.
        /// </summary>
        void Die();
    }
}