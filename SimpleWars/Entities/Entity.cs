namespace SimpleWars.Entities
{
    using System.Diagnostics;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.AssetsManagement;
    using SimpleWars.Terrain;
    using SimpleWars.Terrain.Terrains;
    using SimpleWars.Utils;

    /// <summary>
    /// The entity.
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// The model.
        /// </summary>
        private Model model;

        /// <summary>
        /// The position.
        /// </summary>
        private Vector3 position;

        /// <summary>
        /// The rotation.
        /// </summary>
        private Vector3 rotation;

        /// <summary>
        /// The scale.
        /// </summary>
        private float scale;

        /// <summary>
        /// Indicates if the transformation matrix should be recalculated.
        /// </summary>
        private bool transformationMatrixDirty = true;

        /// <summary>
        /// The transformation matrix.
        /// </summary>
        private Matrix transformationMatrix;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
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
        protected Entity(string assetDir, string assetName, Vector3 position, float scale = 1)
        : this(assetDir, assetName, position, Vector3.Zero, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
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
        protected Entity(string assetDir, string assetName, Vector3 position, Vector3 rotation, float scale = 1)
            : this(assetDir, assetName, position, rotation, 1f, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
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
        protected Entity(string assetDir, string assetName, Vector3 position, Vector3 rotation, float weight, float scale)
        {
            this.Model = ModelsManager.Instance.GetModel(assetDir, assetName);
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Weight = weight;

            this.FogStart = 100;
            this.FogEnd = 600;

            // Cornflower blue
            this.FogColor = new Vector3(0.392157f, 0.584314f, 0.929412f);
        }

        /// <summary>
        /// Gets or sets the position of the entity.
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;
                this.WorldMatrix = Matrix.CreateTranslation(this.Position);
                this.transformationMatrixDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the rotation for the entity.
        /// </summary>
        public Vector3 Rotation
        {
            get
            {
                return this.rotation;
            }

            set
            {
                this.rotation = value;
                this.CalculateRotationMatrix();
                this.transformationMatrixDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the scale for the entity model.
        /// </summary>
        public float Scale
        {
            get
            {
                return this.scale;
            }

            set
            {
                this.scale = value;
                this.ScaleMatrix = Matrix.CreateScale(this.scale);
                this.transformationMatrixDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the weight of the entity.
        /// </summary>
        public float Weight { get; protected set; }

        /// <summary>
        /// Gets the transformation matrix (entity position with rotation and scale applied).
        /// </summary>
        public Matrix TransformationMatrix
        {
            get
            {
                if (this.transformationMatrixDirty)
                {
                    this.transformationMatrix = this.ScaleMatrix * this.RotationMatrix * this.WorldMatrix;
                    this.transformationMatrixDirty = false;
                }

                return this.transformationMatrix;
            }
        }

        /// <summary>
        /// Gets or sets the model of the entity.
        /// </summary>
        public Model Model
        {
            get
            {
                return model;
            }

            protected set
            {
                this.model = value;
            }
        }

        /// <summary>
        /// Gets or sets the fog color. Cornflower blue by default
        /// </summary>
        protected Vector3 FogColor { get; set; }

        /// <summary>
        /// Gets or sets the fog start. 100 by default.
        /// </summary>
        protected float FogStart { get; set; }

        /// <summary>
        /// Gets or sets the fog end. 600 by default.
        /// </summary>
        protected float FogEnd { get; set; }

        /// <summary>
        /// Gets the scale matrix.
        /// </summary>
        protected Matrix ScaleMatrix { get; private set; }

        /// <summary>
        /// Gets the rotation matrix.
        /// </summary>
        protected Matrix RotationMatrix { get; private set; }

        /// <summary>
        /// Gets the world matrix.
        /// </summary>
        protected Matrix WorldMatrix { get; private set; }

        /// <summary>
        /// Snaps the entity Y position to the terrain Y at the specified point
        /// </summary>
        /// <param name="terrain">
        /// The terrain.
        /// </param>
        public void SnapToTerrainHeight(HomeTerrain terrain)
        {
            float height = terrain.GetWorldHeight(this.Position.X, this.Position.Z);
            this.Position = new Vector3(this.Position.X, height, this.Position.Z);
        }

        /// <summary>
        /// Affects entities with gravity
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        /// <param name="terrain">
        /// The terrain.
        /// </param>
        public void GravityAffect(GameTime gameTime, Terrain terrain)
        {
            float height = terrain.GetWorldHeight(this.Position.X, this.Position.Z);

            if (this.Position.Y < height)
            {
                this.Position = new Vector3(this.Position.X, height, this.Position.Z);
            }
            else if (this.Position.Y > height || this.Weight < 0)
            {
                float y =
                    this.Position.Y -
                    (this.Weight *
                    (float)gameTime.ElapsedGameTime.TotalSeconds);

                y = y < height ? height : y;

                this.Position = new Vector3(this.Position.X, y, this.Position.Z);
            }
        }

        /// <summary>
        /// The draw. Static objects with no animation and bones would use the default draw
        /// </summary>
        /// <param name="viewMatrix">
        /// The view matrix.
        /// </param>
        /// <param name="projectionMatrix">
        /// The projection matrix.
        /// </param>
        public virtual void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            // Matrix[] transforms = new Matrix[this.model.Bones.Count];
            // this.model.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in this.Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    Light.Sunlight(effect, effect.SpecularColor);

                    effect.PreferPerPixelLighting = true;

                    effect.FogEnabled = true;
                    effect.FogColor = this.FogColor;
                    effect.FogStart = this.FogStart;
                    effect.FogEnd = this.FogEnd;

                    effect.World = this.TransformationMatrix;        
                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                }

                mesh.Draw();
            }
        }

        /// <summary>
        /// The calculate rotation matrix.
        /// </summary>
        protected virtual void CalculateRotationMatrix()
        {
            Vector3 radians = new Vector3(
                MathHelper.ToRadians(this.Rotation.X),
                MathHelper.ToRadians(this.Rotation.Y),
                MathHelper.ToRadians(this.Rotation.Z));

            Matrix rotX = Matrix.CreateRotationX(radians.X);
            Matrix rotY = Matrix.CreateRotationY(radians.Y);
            Matrix rotZ = Matrix.CreateRotationZ(radians.Z);

            this.RotationMatrix = rotX * rotY * rotZ;
        }
    }
}