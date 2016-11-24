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
        /// The damageTaken.
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
        /// The damageTaken.
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
        /// The damageTaken.
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

        public Vector3 MovementDirection { get; protected set; }

        public Vector3 MovementStartPosition { get; protected set; }

        public Quaternion OrientDirection { get; protected set; }

        [NotMapped]
        public float? MovementDistance { get; protected set; }

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
        /// <param name="damageTaken">
        /// The damage taken.
        /// </param>
        public void TakeDamage(int damageTaken)
        {
            this.Health -= damageTaken - this.Armor;
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
            if (this.MovementDistance == null)
            {
                return;
            }


            float movementFactor = (float)gameTime.ElapsedGameTime.TotalSeconds * this.Speed;
            Vector3 prevPosition = this.Position;

            this.Position += this.MovementDirection * movementFactor;

            this.Rotation = Quaternion.Slerp(
                this.Rotation,
                this.OrientDirection,
                (float)gameTime.ElapsedGameTime.TotalSeconds * this.Speed);

            Tuple<bool, float> collisionResult = Collision.CheckCollision(this, others);

            if (collisionResult.Item1)
            {
                this.Position = prevPosition;
                this.MovementDistance = null;
                return;
            }

            if (Vector3.Distance(this.Position, this.MovementStartPosition) >= this.MovementDistance.Value)
            {
                this.MovementDistance = null;
            }

            this.GravityAffect(gameTime, terrain);
        }

        public virtual void ChangeDestination(Vector3 destination)
        {
            this.MovementDirection = Vector3.Normalize(destination - this.Position);           
            this.MovementDistance = Vector3.Distance(this.Position, destination);
            this.MovementStartPosition = this.Position;

            Vector3 lockedDirection = new Vector3(this.MovementDirection.X, 0, this.MovementDirection.Z);

            float dot = Vector3.Dot(Vector3.Forward, lockedDirection);

            if (Math.Abs(dot + 1.0f) < 0.000001f)
            {
                // vector a and b point exactly in the opposite direction, 
                // so it is a 180 degrees turn around the up-axis
                this.OrientDirection = new Quaternion(Vector3.Up, MathHelper.ToRadians(180.0f));
            }
            if (Math.Abs(dot - (1.0f)) < 0.000001f)
            {
                // vector a and b point exactly in the same direction
                // so we return the identity quaternion
                this.OrientDirection = Quaternion.Identity;
            }

            float rotAngle = (float)Math.Acos(dot);
            Vector3 rotAxis = Vector3.Normalize(Vector3.Cross(Vector3.Forward, lockedDirection));
            Quaternion orientation = Quaternion.CreateFromAxisAngle(rotAxis, rotAngle);
            this.OrientDirection = orientation;
        }

        public virtual void Rotate(GameTime gameTime, float angle)
        {
            float rotFraction = (float)gameTime.ElapsedGameTime.TotalSeconds * this.Speed;
            this.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, angle * rotFraction);
        }
        #endregion
    }
}