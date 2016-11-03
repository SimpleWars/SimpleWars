namespace SimpleWars.Entities
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

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
        protected Entity(Model model, Vector3 position, Vector3 rotation, float scale = 1)
        {
            this.Model = model;
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
        }

        /// <summary>
        /// Moves entity in world space.
        /// </summary>
        /// <param name="direction">
        /// The direction represented as world units in x, y, z axes.
        /// </param>
        public void Move(Vector3 direction)
        {
            this.Position += direction;
        }

        /// <summary>
        /// Rotates entity around own x, y, z axes.
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
                this.WorldMatrix = Matrix.CreateTranslation(this.position);
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

        // Cornflower blue color code
        protected static Vector3 fogColor = new Vector3(0.392157f, 0.584314f, 0.929412f);

        // the unit distance at which fog starts being calculated
        protected static float fogStart = 50f;

        // the unit distance at which fog ends and models behind it are hidden
        protected static float fogEnd = 500f;

        // static objects with no animation and bones would use the default draw
        public virtual void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
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