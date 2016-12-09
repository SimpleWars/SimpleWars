namespace SimpleWars.Models.Economy.Interfaces
{
    using SimpleWars.ModelDTOs.Resources;

    /// <summary>
    /// The Resource interface.
    /// </summary>
    public interface IResource
    {
        int Id { get; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        int Quantity { get; set; }

        ResourceType ResourceType { get; set; }
    }
}