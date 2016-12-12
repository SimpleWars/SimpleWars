namespace SimpleWars.Models.Entities.StaticEntities.ResourceProviders
{
    using System;

    using Microsoft.Xna.Framework;

    using Assets;
    using ModelDTOs.Resources;

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
        /// <param name="rotation">
        /// The rotation.
        /// </param>
        /// <param name="weight">
        /// The weight.
        /// </param>
        /// <param name="scale">
        /// The scale.
        /// </param>
        public Tree(Guid id, Guid ownerId, Vector3 position, Quaternion rotation, float weight = 1, float scale = 1, int quantity = BaseResourceQuantity) 
            : base(id, ownerId, quantity, BaseResType, position, rotation, weight, scale)
        {
        }

        public override void LoadModel()
        {
            this.Model = ModelsManager.Instance.GetModel(Dir, FileName);
        }
    }
}