namespace SimpleWars.GameData.Entities.DynamicEntities
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.AssetsManagement;
    using SimpleWars.Utils;

    using SkinnedModel;

    /// <summary>
    /// The animated entity.
    /// </summary>
    public abstract class AnimatedEntity : DynamicEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedEntity"/> class.
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
        protected AnimatedEntity(string assetDir, string assetName, Vector3 position, float scale = 1)
            : this(assetDir, assetName, position, Vector3.Zero, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedEntity"/> class.
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
        protected AnimatedEntity(string assetDir, string assetName, Vector3 position, Vector3 rotation, float scale = 1)
            : this(assetDir, assetName, position, rotation, 1f, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedEntity"/> class.
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
        protected AnimatedEntity(string assetDir, string assetName, Vector3 position, Vector3 rotation, float weight = 1, float scale = 1)
            : base(assetDir, assetName, position, rotation, weight, scale)
        {
            this.Animation = SkinnedModelsManager.Instance.CreateAnimation(assetDir, assetName);
        }

        /// <summary>
        /// Gets the animation.
        /// </summary>
        [NotMapped]
        public AnimationPlayer Animation { get; private set; }

        /// <summary>
        /// The update animation.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public virtual void UpdateAnimation(GameTime gameTime)
        {
            this.Animation.Update(gameTime.ElapsedGameTime, true, this.TransformationMatrix);
        }

        /// <summary>
        /// The change clip.
        /// </summary>
        /// <param name="clipName">
        /// The clip name.
        /// </param>
        public virtual void ChangeClip(string clipName)
        {
            this.Animation.ChangeClip(clipName);
        }

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="viewMatrix">
        /// The view matrix.
        /// </param>
        /// <param name="projectionMatrix">
        /// The projection matrix.
        /// </param>
        public new virtual void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            Matrix[] bones = this.Animation.GetSkinTransforms();

            foreach (var mesh in this.Model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    Light.Sunlight(effect, effect.SpecularColor);

                    effect.PreferPerPixelLighting = true;

                    effect.SetBoneTransforms(bones);

                    effect.FogEnabled = true;
                    effect.FogColor = this.FogColor;
                    effect.FogStart = this.FogStart;
                    effect.FogEnd = this.FogEnd;

                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                }

                mesh.Draw();
            }
        }
    }
}