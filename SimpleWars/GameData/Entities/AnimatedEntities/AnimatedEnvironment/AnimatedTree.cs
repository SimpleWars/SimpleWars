namespace SimpleWars.GameData.Entities.AnimatedEntities.AnimatedEnvironment
{
    using Microsoft.Xna.Framework;

    using SimpleWars.Assets;

    /// <summary>
    /// The animated tree.
    /// </summary>
    public class AnimatedTree : AnimatedEntity
    {
        /// <summary>
        /// The asset dir.
        /// </summary>
        private const string AssetDir = "Models3D";

        /// <summary>
        /// The asset name.
        /// </summary>
        private const string AssetName = "treeanimated";

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedTree"/> class.
        /// </summary>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="scale">
        /// The scale.
        /// </param>
        public AnimatedTree(Vector3 position, float scale = 1)
            : base(position, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedTree"/> class.
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
        public AnimatedTree(Vector3 position, Vector3 rotation, float scale = 1)
            : base(position, rotation, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedTree"/> class.
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
        public AnimatedTree(Vector3 position, Vector3 rotation, float weight = 1, float scale = 1)
            : base(position, rotation, weight, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedTree"/> class.
        /// </summary>
        protected AnimatedTree()
            :base()
        {
        }

        /// <summary>
        /// The load model.
        /// </summary>
        public override void LoadModel()
        {
            this.Model = ModelsManager.Instance.GetModel(AssetDir, AssetName);
            this.Animation = SkinnedModelsManager.Instance.CreateAnimation(AssetDir, AssetName);
        }
    }
}