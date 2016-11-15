namespace SimpleWars.GameData.EconomyData.Interfaces
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The Resource interface.
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int Id { get; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        int Limit { get; set; }
    }
}