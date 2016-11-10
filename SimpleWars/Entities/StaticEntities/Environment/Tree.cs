namespace SimpleWars.Entities.StaticEntities.Environment
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.DisplayManagement;

    /// <summary>
    /// The tree.
    /// </summary>
    public class Tree : StaticEntity
    {
        private const string Dir = "Models3D";

        private const string FileName = "tree";

        /// <summary>
        /// Initializes a new instance of the <see cref="Tree"/> class.
        /// </summary>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="scale">
        /// The scale.
        /// </param>
        public Tree(Vector3 position, float scale = 1)
            : base(Dir, FileName, position, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tree"/> class.
        /// </summary>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="rotation">
        /// The rotation.
        /// </param>
        /// <param name="scale">
        /// The scale.
        /// </param>
        public Tree(Vector3 position, Vector3 rotation, float scale = 1)
            : base(Dir, FileName, position, rotation, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tree"/> class.
        /// </summary>
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
        public Tree(Vector3 position, Vector3 rotation, float weight = 1, float scale = 1) 
            : base(Dir, FileName, position, rotation, weight, scale)
        {
        }
    }
}