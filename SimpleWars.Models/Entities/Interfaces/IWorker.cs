namespace SimpleWars.Models.Entities.Interfaces
{
    public interface IWorker : IUnit 
    {
        /// <summary>
        /// Empty for now
        /// </summary>
        void Build();

        void GatherResource(IResourceProvider provider);
    }
}