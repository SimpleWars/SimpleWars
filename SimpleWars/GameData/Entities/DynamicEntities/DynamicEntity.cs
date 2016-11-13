namespace SimpleWars.GameData.Entities.DynamicEntities
{
    using Microsoft.Xna.Framework;

    using SimpleWars.GameData.Terrain;

    /// <summary>
    /// The dynamic entity.
    /// </summary>
    public abstract class DynamicEntity : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicEntity"/> class.
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
        protected DynamicEntity(string assetDir, string assetName, Vector3 position, float scale = 1)
         : base(assetDir, assetName, position, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicEntity"/> class.
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
        protected DynamicEntity(string assetDir, string assetName, Vector3 position, Vector3 rotation, float scale = 1)
            : base(assetDir, assetName, position, rotation, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicEntity"/> class.
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
        protected DynamicEntity(string assetDir, string assetName, Vector3 position, Vector3 rotation, float weight = 1, float scale = 1)
            : base(assetDir, assetName, position, rotation, weight, scale)
        {
        }

        /// <summary>
        /// Moves entity in world space.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        /// <param name="direction">
        /// The direction represented as world units in x, y, z axes.
        /// </param>
        /// <param name="terrain">
        /// The terrain.
        /// </param>
        public virtual void Move(GameTime gameTime, Vector3 direction, Terrain terrain)
        {
            this.Position += direction;

            this.GravityAffect(gameTime, terrain);
        }

        /// <summary>
        /// Rotates entities around their own axes.
        /// </summary>
        /// <param name="angle">
        /// The rotation in degrees around x, y, z represented as vector.
        /// </param>
        public virtual void Rotate(Vector3 angle)
        {
            this.Rotation += angle;
        }
    }
}