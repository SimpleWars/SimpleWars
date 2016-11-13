namespace SimpleWars.GameData.Terrain
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The terrain.
    /// </summary>
    public abstract class Terrain
    {
        /// <summary>
        /// The transformation matrix.
        /// </summary>
        private Matrix transformationMatrix;

        /// <summary>
        /// The world matrix.
        /// </summary>
        private Matrix worldMatrix;

        /// <summary>
        /// The rotation matrix.
        /// </summary>
        private Matrix rotationMatrix;

        /// <summary>
        /// The scale matrix.
        /// </summary>
        private Matrix scaleMatrix;

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
        /// Indicates if the transformation matrix needs recalculation
        /// </summary>
        private bool transformationMatrixDirty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Terrain"/> class.
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
        protected Terrain(Vector3? position, Vector3? rotation, float scale = 1)
        {
            this.transformationMatrixDirty = true;
            this.Position = position ?? Vector3.Zero;
            this.Rotation = rotation ?? Vector3.Zero;
            this.Scale = scale;
        }

        /// <summary>
        /// Gets the transformation matrix.
        /// </summary>
        protected Matrix TransformationMatrix
        {
            get
            {
                if (this.transformationMatrixDirty)
                {
                    this.transformationMatrix = 
                        this.scaleMatrix * 
                        this.rotationMatrix * 
                        this.worldMatrix;
                }

                return this.transformationMatrix;
            }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        protected Vector3 Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;
                this.worldMatrix = Matrix.CreateTranslation(this.Position);
                this.transformationMatrixDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        protected Vector3 Rotation
        {
            get
            {
                return this.rotation;
            }

            set
            {
                this.rotation = value;
                this.rotationMatrix =
                    Matrix.CreateRotationX(MathHelper.ToRadians(this.Rotation.X)) *
                    Matrix.CreateRotationY(MathHelper.ToRadians(this.Rotation.Y)) *
                    Matrix.CreateRotationZ(MathHelper.ToRadians(this.Rotation.Z));
                this.transformationMatrixDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        protected float Scale
        {
            get
            {
                return this.scale;
            }

            set
            {
                this.scale = value;
                this.scaleMatrix = Matrix.CreateScale(this.Scale);
                this.transformationMatrixDirty = true;
            }
        }


        /// <summary>
        /// Calculates the height of the terrain in world space for the specified x and z
        /// </summary>
        /// <param name="x">
        /// The x coordinate
        /// </param>
        /// <param name="z">
        /// The z coordinate
        /// </param>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        public abstract float GetWorldHeight(float x, float z);

        /// <summary>
        /// Draws the terrain
        /// </summary>
        /// <param name="viewMatrix">
        /// The view Matrix.
        /// </param>
        /// <param name="projectionMatrix">
        /// The projection Matrix.
        /// </param>
        public abstract void Draw(Matrix viewMatrix, Matrix projectionMatrix);
    }
}