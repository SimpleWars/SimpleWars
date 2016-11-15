namespace SimpleWars.GameData.Entities.AnimatedEntities
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.GameData.Entities.Interfaces;
    using SimpleWars.Utils;

    using SkinnedModel;

    /// <summary>
    /// The animated entity.
    /// </summary>
    public abstract class AnimatedEntity : Entity, IAnimatedEntity
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedEntity"/> class.
        /// </summary>
        protected AnimatedEntity()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedEntity"/> class.
        /// </summary>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="scale">
        /// The scale.
        /// </param>
        protected AnimatedEntity(Vector3 position, float scale = 1)
            : this(position, Vector3.Zero, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedEntity"/> class.
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
        protected AnimatedEntity(Vector3 position, Vector3 rotation, float scale = 1)
            : this(position, rotation, 1f, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedEntity"/> class.
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
        protected AnimatedEntity(Vector3 position, Vector3 rotation, float weight = 1, float scale = 1)
            : base(position, rotation, weight, scale)
        {
        }
        #endregion

        #region Animation Related
        /// <summary>
        /// Gets or sets the animation.
        /// </summary>
        [NotMapped]
        public AnimationPlayer Animation { get; protected set; }

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
        #endregion

        #region Animation Draw
        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="viewMatrix">
        /// The view matrix.
        /// </param>
        /// <param name="projectionMatrix">
        /// The projection matrix.
        /// </param>
        public override void Draw(Matrix viewMatrix, Matrix projectionMatrix)
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
        #endregion
    }
}