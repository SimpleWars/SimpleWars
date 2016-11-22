namespace SimpleWars.Models.Entities.DynamicEntities.WorkerUnits
{
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
    }
}