namespace SimpleWars.GameData.Entities.Interfaces
{
    using Microsoft.Xna.Framework;

    using SkinnedModel;

    /// <summary>
    /// The AnimatedEntity interface.
    /// </summary>
    public interface IAnimatedEntity : IEntity
    {
        /// <summary>
        /// Gets the animation.
        /// </summary>
        AnimationPlayer Animation { get; }

        /// <summary>
        /// The update animation.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        void UpdateAnimation(GameTime gameTime);

        /// <summary>
        /// The change clip.
        /// </summary>
        /// <param name="clipName">
        /// The clip name.
        /// </param>
        void ChangeClip(string clipName);
    }
}