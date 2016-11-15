namespace SimpleWars.GameData.Entities.DynamicEntities
{
    using System;

    using Microsoft.Xna.Framework;

    using SimpleWars.GameData.Entities.Interfaces;
    using SimpleWars.GameData.Terrain;
    using SimpleWars.GameData.Terrain.Interfaces;

    /// <summary>
    /// The dynamic entity.
    /// </summary>
    public abstract class Unit : Entity, IUnit
    {
        #region Private Fields

        private int health;

        private int damage;

        private int armor;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Unit"/> class.
        /// </summary>
        protected Unit()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Unit"/> class.
        /// </summary>
        /// <param name="health">
        /// The health.
        /// </param>
        /// <param name="damage">
        /// The damage.
        /// </param>
        /// <param name="armor">
        /// The armor.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="scale">
        /// The scale.
        /// </param>
        protected Unit(int health, int damage, int armor, Vector3 position, float scale = 1)
         : this(health, damage, armor, position, Vector3.Zero, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Unit"/> class.
        /// </summary>
        /// <param name="health">
        /// The health.
        /// </param>
        /// <param name="damage">
        /// The damage.
        /// </param>
        /// <param name="armor">
        /// The armor.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="rotation">
        /// The rotation.
        /// </param>
        /// <param name="scale">
        /// The scale.
        /// </param>
        protected Unit(int health, int damage, int armor, Vector3 position, Vector3 rotation, float scale = 1)
            : this(health, damage, armor, position, rotation, 1f, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Unit"/> class.
        /// </summary>
        /// <param name="health">
        /// The health.
        /// </param>
        /// <param name="damage">
        /// The damage.
        /// </param>
        /// <param name="armor">
        /// The armor.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="rotation">
        /// The rotation.
        /// </param>
        /// <param name="weight">
        /// The weight.
        /// </param>
        /// <param name="scale">
        /// The scale.
        /// </param>
        protected Unit(int health, int damage, int armor, Vector3 position, Vector3 rotation, float weight = 1, float scale = 1)
            : base(position, rotation, weight, scale)
        {
            this.Health = health;
            this.Damage = damage;
            this.Armor = armor;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the health.
        /// </summary>
        public int Health
        {
            get
            {
                return this.health;
            }

            set
            {
                if (value <= 0)
                {
                    this.health = 0;
                    this.Die();
                    return;
                }

                this.health = value;
            }
        }

        /// <summary>
        /// Gets or sets the damage.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// </exception>
        public int Damage
        {
            get
            {
                return this.damage;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Damage cannot be negative");
                }

                this.damage = value;
            }
        }

        /// <summary>
        /// Gets or sets the armor.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// </exception>
        public int Armor
        {
            get
            {
                return this.armor;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Armor cannot be negative");
                }

                this.armor = value;
            }
        }

        #endregion

        #region Unit Specific Methods

        /// <summary>
        /// The take damage.
        /// </summary>
        /// <param name="damage">
        /// The damage.
        /// </param>
        public void TakeDamage(int damage)
        {
            this.Health -= damage - this.Armor;
        }

        /// <summary>
        /// The die.
        /// </summary>
        public void Die()
        {
            // No death logic for now
        }

        #endregion

        #region Movement Methods
        /// <summary>
        /// Moves entity in world space.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        /// <param name="direction">
        /// The direction represented as world units in x, y, z axes.
        /// </param>
        /// <param name="terrain">
        /// The terrain.
        /// </param>
        public virtual void Move(GameTime gameTime, Vector3 direction, ITerrain terrain)
        {
            this.Position += direction;

            this.GravityAffect(gameTime, terrain);
        }

        /// <summary>
        /// Rotates entities around their own axes.
        /// </summary>
        /// <param name="angle">
        /// The rotation in degrees around x, y, z represented as vector.
        /// </param>
        public virtual void Rotate(Vector3 angle)
        {
            this.Rotation += angle;
        }
        #endregion
    }
}