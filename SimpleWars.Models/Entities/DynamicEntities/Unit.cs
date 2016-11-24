namespace SimpleWars.Models.Entities.DynamicEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;

    using Microsoft.Xna.Framework;

    using SimpleWars.Environment.Terrain.Interfaces;
    using SimpleWars.Models.Economy.Interfaces;
    using SimpleWars.Models.Entities.Interfaces;
    using SimpleWars.Models.Utils;

    /// <summary>
    /// The dynamic entity.
    /// </summary>
    public abstract class Unit : Entity, IUnit
    {
        #region Private Fields

        private int health;

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
        /// The damageToTake.
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
        protected Unit(int maxHealth, int health, float speed, int armor, Vector3 position, float scale = 1)
         : this(maxHealth, health, speed, armor, position, Quaternion.Identity, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Unit"/> class.
        /// </summary>
        /// <param name="health">
        /// The health.
        /// </param>
        /// <param name="damage">
        /// The damageToTake.
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
        protected Unit(int maxHealth, int health, float speed, int armor, Vector3 position, Quaternion rotation, float scale = 1)
            : this(maxHealth, health, speed, armor, position, rotation, 1f, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Unit"/> class.
        /// </summary>
        /// <param name="health">
        /// The health.
        /// </param>
        /// <param name="damage">
        /// The damageToTake.
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
        protected Unit(int maxHealth, int health, float speed, int armor, Vector3 position, Quaternion rotation, float weight = 1, float scale = 1)
            : base(position, rotation, weight, scale)
        {
            this.MaxHealth = maxHealth;
            this.Speed = speed;
            this.IsAlive = true;
            this.Health = health;
            this.Armor = armor;
        }
        #endregion

        #region Properties
        [NotMapped]
        public int MaxHealth { get; protected set; }

        /// <summary>
        /// Gets or sets the health.
        /// </summary>
        public int Health
        {
            get
            {
                return this.health;
            }

            private set
            {
                if (value > this.MaxHealth)
                {
                    this.health = this.MaxHealth;
                    return;
                }

                if (value <= 0)
                {
                    this.health = 0;
                    this.IsAlive = false;
                    return;
                }

                this.health = value;
            }
        }

        [NotMapped]
        public Vector3? Destination { get; set; }

        [NotMapped]
        public float Speed { get; protected set; }

        /// <summary>
        /// Gets or sets the armor.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// </exception>
        [NotMapped]
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

        public bool IsAlive { get; protected set; }
        #endregion

        #region Utility Methods

        /// <summary>
        /// The take damage.
        /// </summary>
        /// <param name="damageToTake">
        /// The damage taken.
        /// </param>
        public void TakeDamage(int damageToTake)
        {
            this.Health -= damageToTake - this.Armor;
        }

        public virtual void Update(GameTime gameTime, ITerrain terrain, IEnumerable<IEntity> others)
        {
            this.Move(gameTime, terrain, others);
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
        /// The destination represented as world units in x, y, z axes.
        /// </param>
        /// <param name="terrain">
        /// The terrain.
        /// </param>
        public virtual void Move(GameTime gameTime, ITerrain terrain, IEnumerable<IEntity> others)
        {
            if (this.Destination == null)
            {
                return;
            }

            float timeFactor = (float)gameTime.ElapsedGameTime.TotalSeconds * this.Speed;
            Vector3 startPosition = this.Position;
            Vector3 direction = Vector3.Normalize(this.Destination.Value - this.Position);

            this.Position += direction * timeFactor;
            this.AdjustRotation(direction, timeFactor);

            Tuple<bool, Vector3?> collisionResult = Collision.CheckCollision(this, others);

            if (collisionResult.Item1)
            {
                this.Position = startPosition;
                if (collisionResult.Item2 != null)
                {
                    Vector3 directionOffset = collisionResult.Item2.Value;
                    this.Position += directionOffset * timeFactor;
                    this.AdjustRotation(directionOffset, timeFactor);
                }
            }

            if (Vector3.Distance(this.Position, this.Destination.Value) < 0.2f)
            {
                this.Destination = null;
            }

            this.GravityAffect(gameTime, terrain);
        }

        protected virtual void AdjustRotation(Vector3 direction, float timeFactor)
        {
            direction.Y = 0;

            float dot = Vector3.Dot(Vector3.Forward, direction);

            float rotAngle = (float)Math.Acos(dot);
            Vector3 rotAxis = Vector3.Normalize(Vector3.Cross(Vector3.Forward, direction));

            this.Rotation = Quaternion.Slerp(
                this.Rotation,
                Quaternion.CreateFromAxisAngle(rotAxis, rotAngle),
                timeFactor);
        }

        public virtual void Rotate(GameTime gameTime, float angle)
        {
            float rotFraction = (float)gameTime.ElapsedGameTime.TotalSeconds * this.Speed;
            this.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, angle * rotFraction);
        }
        #endregion
    }
}