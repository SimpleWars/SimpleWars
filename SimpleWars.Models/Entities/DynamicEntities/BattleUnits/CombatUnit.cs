namespace SimpleWars.Models.Entities.DynamicEntities.BattleUnits
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.Xna.Framework;

    using SimpleWars.Environment.Terrain.Interfaces;
    using SimpleWars.Models.Entities.Interfaces;
    using SimpleWars.Models.Utils;

    public abstract class CombatUnit : Unit, ICombatUnit
    {
        private const int MaxTimeBetweenAttacksInMs = 10000;

        private int damage;

        private double timeSinceLastAttack;

        protected CombatUnit(Guid id, Guid ownerId, int maxHealth, int health, float speed, int damage, int armor, float attackRange, int attackSpeed, Vector3 position, Quaternion rotation, float weight = 1, float scale = 1) 
            : base(id, ownerId, maxHealth, health, speed, armor, position, rotation, weight, scale)
        {
            this.AttackRange = attackRange;
            this.AttackSpeed = attackSpeed;
            this.Damage = damage;
            this.AttackDelay = MaxTimeBetweenAttacksInMs / (double)this.AttackSpeed;
        }

        
        public IKillable Target { get; protected set; }

        /// <summary>
        /// Gets or sets the damageTaken.
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
 
        protected bool CanAttack => this.timeSinceLastAttack >= this.AttackDelay;
      
        protected double AttackDelay { get; set; }

        
        public int AttackSpeed { get; protected set; }

        public override void Update(GameTime gameTime, ITerrain terrain, IEnumerable<IEntity> others)
        {
            base.Update(gameTime, terrain, others);

            if (this.timeSinceLastAttack < this.AttackDelay)
            {
                this.timeSinceLastAttack += gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            this.TryAttack();
        }

        
        public float AttackRange { get; set; }

        public virtual void TryAttack()
        {
            if (this.Target == null)
            {
                return;
            }

            bool inRange = Collision.CheckSingleCollision(this, this.Target)
                            || Vector3.Distance(this.Position, this.Target.Position) <= this.AttackRange;

            if (inRange && this.CanAttack)
            {
                this.Target.TakeDamage(this.Damage);
                this.timeSinceLastAttack = 0;
            }

            if (inRange)
            {
                this.AdjustRotationImmediate(Vector3.Normalize(this.Target.Position - this.Position));
            }
            else
            {
                this.Destination = this.Target.Position;
            }
        }

        public virtual void ChangeAttackTarget(IKillable target)
        {
            if (target != this)
            {
                this.Target = target;
            }
        }
    }
}