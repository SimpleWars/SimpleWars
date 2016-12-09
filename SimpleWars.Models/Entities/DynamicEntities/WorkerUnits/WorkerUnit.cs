namespace SimpleWars.Models.Entities.DynamicEntities.WorkerUnits
{
    using System;

    using Microsoft.Xna.Framework;

    using SimpleWars.Models.Entities.Interfaces;
    /// <summary>
    /// No implementation yet.
    /// </summary>
    public class WorkerUnit : Unit, IWorker
    {
        public override void LoadModel()
        {
            // no model for workers yet
        }

        public void Build()
        {
            throw new System.NotImplementedException();
        }

        public void GatherResource(IResourceProvider provider)
        {
            throw new System.NotImplementedException();
        }

        public WorkerUnit(Guid id, Guid ownerId, int maxHealth, int health, float speed, int armor, Vector3 position, Quaternion rotation, float weight = 1, float scale = 1)
            : base(id, ownerId, maxHealth, health, speed, armor, position, rotation, weight, scale)
        {
        }
    }
}