namespace SimpleWars.Assets.Interfaces
{
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The Asset3D interface.
    /// </summary>
    public interface IAsset3D : IAsset
    {
        /// <summary>
        /// Gets the model.
        /// </summary>
        Model Model { get; }
    }
}