namespace SimpleWars.Assets.Interfaces
{
    using SkinnedModel;

    /// <summary>
    /// The AssetAnimation3D interface.
    /// </summary>
    public interface IAssetAnimation3D : IAsset
    {
        /// <summary>
        /// Gets the skinning data of the animation.
        /// </summary>
        SkinningData SkinningData { get; }
    }
}