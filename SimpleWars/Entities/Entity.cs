namespace SimpleWars.Entities
{
    using System.Diagnostics;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Terrain;

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
        /// The weight of the entity.
        /// </summary>
        private float weight;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="scale">
        /// The scale.
        /// </param>
        protected Entity(Model model, Vector3 position, float scale = 1)
        : this(model, position, Vector3.Zero, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="model">
        /// The model.
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
        protected Entity(Model model, Vector3 position, Vector3 rotation, float scale)
            : this(model, position, rotation, 1f, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="model">
        /// The model.
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
        protected Entity(Model model, Vector3 position, Vector3 rotation, float weight, float scale)
        {
            this.Model = model;
            this.Position = position;
            this.Rotation = rotation;
            this.weight = weight;
            this.Scale = scale;
        }

        /// <summary>
        /// Moves entity in world space.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        /// <param name="direction">
        /// The direction represented as world units in x, y, z axes.
        /// </param>
        /// <param name="terrain">
        /// The terrain.
        /// </param>
        public void Move(GameTime gameTime, Vector3 direction, HomeTerrain terrain)
        {
            float x = this.Position.X + direction.X;
            float z = this.Position.Z + direction.Z;
            float height = terrain.GetWorldHeight(x, z);

            if (this.Position.Y <= height)
            {
                this.Position = new Vector3(x, height, z);
            }
            else
            {
                float y = this.Position.Y - this.weight * (float)gameTime.ElapsedGameTime.TotalSeconds;
                y = y < height ? height : y;
                this.Position = new Vector3(x, y, z);
            }
        }

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
        /// Rotates entities around their own axes.
        /// </summary>
        /// <param name="angle">
        /// The rotation in degrees around x, y, z represented as vector.
        /// </param>
        public void Rotate(Vector3 angle)
        {
            this.Rotation += angle;
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

            set
            {
                model = value;
            }
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

        private bool transformationMatrixDirty = true;

        private Matrix transformationMatrix;

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

            private set
            {
                this.transformationMatrix = value;
                this.transformationMatrixDirty = true;
            }
        }

        protected Matrix ScaleMatrix { get; private set; }
        protected Matrix RotationMatrix { get; private set; }
        protected Matrix WorldMatrix { get; private set; }

        // Cornflowerblue color code
        protected static Vector3 fogColor = new Vector3(0.392157f, 0.584314f, 0.929412f);

        // the unit distance at which fog starts being calculated
        protected static float fogStart = 100f;

        // the unit distance at which fog ends and models behind it are hidden
        protected static float fogEnd = 600f;

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
            //Matrix[] transforms = new Matrix[this.model.Bones.Count];
            //this.model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in this.Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.FogEnabled = true;
                    effect.FogColor = fogColor;
                    effect.FogStart = fogStart;
                    effect.FogEnd = fogEnd;

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