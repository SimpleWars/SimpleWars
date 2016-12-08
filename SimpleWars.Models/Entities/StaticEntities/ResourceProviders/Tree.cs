namespace SimpleWars.Models.Entities.StaticEntities.ResourceProviders
{
    using Microsoft.Xna.Framework;

    using SimpleWars.Assets;
    using SimpleWars.ModelDTOs.Resources;

    /// <summary>
    /// The tree.
    /// </summary>
    public class Tree : ResourceProvider
    {
        private const string Dir = "Models3D";

        private const string FileName = "tree";

        private const int BaseResourceQuantity = 50;

        private const ResourceType BaseResType = ResourceType.Wood;

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
            : base(BaseResourceQuantity, BaseResType, position, scale)
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
        public Tree(Vector3 position, Quaternion rotation, float scale = 1)
            : base(BaseResourceQuantity, BaseResType, position, rotation, scale)
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
        public Tree(Vector3 position, Quaternion rotation, float weight = 1, float scale = 1) 
            : base(BaseResourceQuantity, BaseResType, position, rotation, weight, scale)
        {
        }

        public Tree(int quantity, Vector3 position, Quaternion rotation, float weight = 1, float scale = 1)
            : base(quantity, BaseResType, position, rotation, weight, scale)
        {
        }

        public override void LoadModel()
        {
            this.Model = ModelsManager.Instance.GetModel(Dir, FileName);
        }
    }
}