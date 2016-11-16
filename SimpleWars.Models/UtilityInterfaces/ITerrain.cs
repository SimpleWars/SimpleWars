namespace SimpleWars.Models.UtilityInterfaces
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The Terrain interface.
    /// </summary>
    public interface ITerrain
    {
        /// <summary>
        /// The get world height.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="z">
        /// The z.
        /// </param>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        float GetWorldHeight(float x, float z);

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