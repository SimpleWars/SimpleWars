namespace SimpleWars.Entities.StaticEntities
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The static entity.
    /// </summary>
    public abstract class StaticEntity : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaticEntity"/> class.
        /// </summary>
        /// <param name="assetDir">
        /// The asset dir.
        /// </param>
        /// <param name="assetName">
        /// The asset name.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="scale">
        /// The scale.
        /// </param>
        protected StaticEntity(string assetDir, string assetName, Vector3 position, float scale = 1)
            : base(assetDir, assetName, position, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticEntity"/> class.
        /// </summary>
        /// <param name="assetDir">
        /// The asset dir.
        /// </param>
        /// <param name="assetName">
        /// The asset name.
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
        protected StaticEntity(string assetDir, string assetName, Vector3 position, Vector3 rotation, float scale = 1)
            : base(assetDir, assetName, position, rotation, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticEntity"/> class.
        /// </summary>
        /// <param name="assetDir">
        /// The asset dir.
        /// </param>
        /// <param name="assetName">
        /// The asset name.
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
        protected StaticEntity(string assetDir, string assetName, Vector3 position, Vector3 rotation, float weight, float scale)
            : base(assetDir, assetName, position, rotation, weight, scale)
        {
        }
    }
}