namespace SimpleWars.Models.Entities.DynamicEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;

    using Environment.Terrain.Interfaces;
    using Extensions;
    using Interfaces;
    using Utils;

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
        protected Unit(Guid id, Guid ownerId, int maxHealth, int health, float speed, int armor, Vector3 position, Quaternion rotation, float weight = 1, float scale = 1)
            : base(id, ownerId, position, rotation, weight, scale)
        {
            this.MaxHealth = maxHealth;
            this.Speed = speed;
            this.Health = health;
            this.Armor = armor;
        }
        #endregion

        #region Properties
        
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
                    this.Modified = true;
                    return;
                }

                if (value <= 0)
                {
                    this.health = 0;
                    this.Modified = true;
                    return;
                }

                this.health = value;
                this.Modified = true;
            }
        }
    
        public Vector3? Destination { get; set; }
      
        public float Speed { get; protected set; }

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

        public bool IsAlive => this.Health > 0;

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

            IEnumerable<IEntity> collidees = Collision.GetCollisions(this, others).ToArray();

            if (collidees.Any())
            {
                Vector3 bestPath = startPosition;
                Vector3 bestDirectionOffset = Collision.GetGoRoundDirection(this, collidees.First());

                Vector3 currentPath = startPosition;

                foreach (var other in collidees)
                {
                    Vector3 directionOffset = Collision.GetGoRoundDirection(this, other);
                    currentPath += directionOffset * timeFactor;
                    if (Vector3.Distance(currentPath, this.Destination.Value)
                        < Vector3.Distance(bestPath, this.Destination.Value))
                    {
                        bestPath = currentPath;
                        bestDirectionOffset = directionOffset;
                    }
                }

                this.Position = bestPath;
                this.AdjustRotation(bestDirectionOffset, timeFactor);
            }

            if (Vector3.Distance(this.Position, this.Destination.Value) < 0.1f)
            {
                this.Destination = null;
            }

            this.GravityAffect(gameTime, terrain);
        }

        public virtual void Rotate(GameTime gameTime, float angle)
        {
            float rotFraction = (float)gameTime.ElapsedGameTime.TotalSeconds * this.Speed;
            this.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, angle * rotFraction);
        }

        protected virtual void AdjustRotation(Vector3 direction, float timeFactor)
        {
            direction.Y = 0;

            this.Rotation = Quaternion.Slerp(
                this.Rotation,
                direction.FromDirectionToQuaternion(),
                timeFactor);
        }

        protected virtual void AdjustRotationImmediate(Vector3 direction)
        {
            direction.Y = 0;

            this.Rotation = direction.FromDirectionToQuaternion();
        }
        #endregion
    }
}