﻿namespace SimpleWars.Models.Entities.DynamicEntities.BattleUnits
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

        protected CombatUnit()
        {
        }

        protected CombatUnit(int maxHealth, int health, float speed, int damage, int armor, float attackRange, int attackSpeed, Vector3 position, float scale = 1) 
            : this(maxHealth, health, speed, damage, armor, attackRange, attackSpeed, position, Quaternion.Identity, 1, scale)
        {
        }

        protected CombatUnit(int maxHealth, int health, float speed, int damage, int armor, float attackRange, int attackSpeed, Vector3 position, Quaternion rotation, float scale = 1) 
            : this(maxHealth, health, speed, damage, armor, attackRange, attackSpeed, position, rotation, 1, scale)
        {       
        }

        protected CombatUnit(int maxHealth, int health, float speed, int damage, int armor, float attackRange, int attackSpeed, Vector3 position, Quaternion rotation, float weight = 1, float scale = 1) 
            : base(maxHealth, health, speed, armor, position, rotation, weight, scale)
        {
            this.AttackRange = attackRange;
            this.AttackSpeed = attackSpeed;
            this.Damage = damage;
            this.AttackDelay = MaxTimeBetweenAttacksInMs / (double)this.AttackSpeed;
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
        protected bool CanAttack => this.timeSinceLastAttack >= this.AttackDelay;

        [NotMapped]
        protected double AttackDelay { get; set; }

        [NotMapped]
        public int AttackSpeed { get; protected set; }

        public override void Update(GameTime gameTime, ITerrain terrain, IEnumerable<IEntity> others)
        {
            if (this.timeSinceLastAttack < this.AttackDelay)
            {
                this.timeSinceLastAttack += gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            bool inRange = this.TryAttack();

            if (!inRange)
            {
                base.Move(gameTime, terrain, others);
            }
        }

        [NotMapped]
        public float AttackRange { get; set; }

        public virtual bool TryAttack()
        {
            if (this.Target == null)
            {
                return false;
            }

            if ((Collision.CheckSingleCollision(this, this.Target) || 
                Vector3.Distance(this.Position, this.Target.Position) <= this.AttackRange))
            {
                if (this.CanAttack)
                {
                    this.Target.TakeDamage(this.Damage);
                    this.timeSinceLastAttack = 0;
                }
                
                return true;
            }
            else
            {
                this.Destination = this.Target.Position;
                return false;
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