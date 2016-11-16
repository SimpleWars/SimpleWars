namespace SimpleWars.GameData.Entities.StaticEntities.Environment
{
    using System;

    using Microsoft.Xna.Framework;

    using SimpleWars.Assets;

    /// <summary>
    /// The tree.
    /// </summary>
    public class Tree : ResourceProvider
    {
        private const string Dir = "Models3D";

        private const string FileName = "tree";

        private const int BaseResourceQuantity = 50;

        private const string BaseResourceType = "Wood";

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
            : base(BaseResourceQuantity, BaseResourceType, position, scale)
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
            : base(BaseResourceQuantity, BaseResourceType, position, rotation, scale)
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
            : base(BaseResourceQuantity, BaseResourceType, position, rotation, weight, scale)
        {
        }

        protected Tree()
            :base()
        {
        }

        public override void LoadModel()
        {
            this.Model = ModelsManager.Instance.GetModel(Dir, FileName);
        }
    }
}