namespace SimpleWars.Models.Entities.Interfaces
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Environment.Terrain.Interfaces;
    using SimpleWars.Models.Users;

    /// <summary>
    /// The Entity interface.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets the model.
        /// </summary>
        Model Model { get; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        Player Player { get; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        Vector3 Rotation { get; set; }

        /// <summary>
        /// Gets the transformation matrix.
        /// </summary>
        Matrix TransformationMatrix { get; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        float Scale { get; set; }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        float Weight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is highlighted.
        /// </summary>
        bool IsHighlighted { get; set; }

        /// <summary>
        /// The load model.
        /// </summary>
        void LoadModel();

        /// <summary>
        /// The snap to terrain height.
        /// </summary>
        /// <param name="terrain">
        /// The terrain.
        /// </param>
        void SnapToTerrainHeight(ITerrain terrain);

        /// <summary>
        /// The gravity affect.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        /// <param name="terrain">
        /// The terrain.
        /// </param>
        void GravityAffect(GameTime gameTime, ITerrain terrain);

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="viewMatrix">
        /// The view matrix.
        /// </param>
        /// <param name="projectionMatrix">
        /// The projection matrix.
        /// </param>
        void Draw(Matrix viewMatrix, Matrix projectionMatrix);
    }
}