namespace SimpleWars.Entities.DynamicEntities
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.AssetsManagement;
    using SimpleWars.Utils;

    using SkinnedModel;

    public abstract class AnimatedEntity : DynamicEntity
    {
        protected string dir;

        protected string filename;

        protected AnimatedEntity(string assetDir, string assetName, Vector3 position, float scale = 1)
            : this(assetDir, assetName, position, Vector3.Zero, scale)
        {
        }

        protected AnimatedEntity(string assetDir, string assetName, Vector3 position, Vector3 rotation, float scale = 1)
            : this(assetDir, assetName, position, rotation, 1f, scale)
        {
        }

        protected AnimatedEntity(string assetDir, string assetName, Vector3 position, Vector3 rotation, float weight = 1, float scale = 1)
            : base(assetDir, assetName, position, rotation, weight, scale)
        {
            this.Model = ModelsManager.Instance.GetModel(assetDir, assetName);
            this.Animation = SkinnedModelsManager.Instance.CreateAnimation(assetDir, assetName);

            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Weight = weight;
        }

        /// <summary>
        /// Gets the animation.
        /// </summary>
        public AnimationPlayer Animation { get; private set; }

        public virtual void UpdateAnimation(GameTime gameTime)
        {
            this.Animation.Update(gameTime.ElapsedGameTime, true, this.TransformationMatrix);
        }

        public virtual void ChangeClip(string clipName)
        {
            this.Animation.ChangeClip(clipName);
        }

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