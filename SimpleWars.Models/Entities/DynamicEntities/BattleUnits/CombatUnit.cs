namespace SimpleWars.Models.Entities.DynamicEntities.BattleUnits
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.Xna.Framework;

    using SimpleWars.Environment.Terrain.Interfaces;
    using SimpleWars.Models.Entities.Interfaces;
    public abstract class CombatUnit : Unit, ICombatUnit
    {
        private int damage;

        private double timeSinceLastAttack;

        protected CombatUnit()
        {
        }

        protected CombatUnit(int maxHealth, int health, float speed, int damage, int armor, float attackRange, int timeBetweenAttacks, Vector3 position, float scale = 1) 
            : this(maxHealth, health, speed, damage, armor, attackRange, timeBetweenAttacks, position, Quaternion.Identity, 1, scale)
        {
        }

        protected CombatUnit(int maxHealth, int health, float speed, int damage, int armor, float attackRange, int timeBetweenAttacks, Vector3 position, Quaternion rotation, float scale = 1) 
            : this(maxHealth, health, speed, damage, armor, attackRange, timeBetweenAttacks, position, rotation, 1, scale)
        {       
        }

        protected CombatUnit(int maxHealth, int health, float speed, int damage, int armor, float attackRange, int timeBetweenAttacks, Vector3 position, Quaternion rotation, float weight = 1, float scale = 1) 
            : base(maxHealth, health, speed, armor, position, rotation, weight, scale)
        {
            this.AttackRange = attackRange;
            this.TimeBetweenAttacks = timeBetweenAttacks;
            this.Damage = damage;
        }

        [NotMapped]
        public IKillable Target { get; protected set; }

        /// <summary>
        /// Gets or sets the damageTaken.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// </exception>
        [NotMapped]
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

        [NotMapped]
        public int TimeBetweenAttacks { get; protected set; }

        public override void Update(GameTime gameTime, ITerrain terrain, IEnumerable<IEntity> others)
        {
            base.Update(gameTime, terrain, others);

            if (this.timeSinceLastAttack < this.TimeBetweenAttacks)
            {
                this.timeSinceLastAttack += gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            this.TryAttack();
        }

        [NotMapped]
        public float AttackRange { get; set; }

        public virtual void TryAttack()
        {
            if (this.Target == null)
            {
                return;
            }

            if (Vector3.Distance(this.Position, this.Target.Position) <= this.AttackRange
                && this.timeSinceLastAttack >= this.TimeBetweenAttacks)
            {
                this.Target.TakeDamage(this.Damage);
                this.timeSinceLastAttack = 0;
            }
            else
            {
                this.MovementDistance = Vector3.Distance(this.Target.Position, this.Position);
                this.MovementDirection = Vector3.Normalize(this.Target.Position - this.Position);
                this.MovementStartPosition = this.Position;
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