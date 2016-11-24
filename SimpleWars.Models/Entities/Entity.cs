namespace SimpleWars.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Environment.Terrain.Interfaces;
    using SimpleWars.Models.Entities.DynamicEntities.BattleUnits;
    using SimpleWars.Models.Entities.Interfaces;
    using SimpleWars.Models.Users;
    using SimpleWars.Utils;

    /// <summary>
    /// The entity.
    /// </summary>
    public abstract class Entity : IEntity
    {
        #region Private Fields
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
        private Quaternion rotation;

        private float? rotX;

        private float? rotY;

        private float? rotZ;

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
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class. 
        /// Empty constructor for stupid ORM
        /// </summary>
        protected Entity()
        {
            this.LoadModel();
            this.FogStart = 100;
            this.FogEnd = 600;

            // Cornflower blue
            this.FogColor = Color.CornflowerBlue.ToVector3();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="scale">
        /// The scale.
        /// </param>
        protected Entity(Vector3 position, float scale = 1)
        : this(position, Quaternion.Identity, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
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
        protected Entity(Vector3 position, Quaternion rotation, float scale = 1)
            : this(position, rotation, 1f, scale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
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
        protected Entity(Vector3 position, Quaternion rotation, float weight, float scale)
        {
            this.LoadModel();
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Weight = weight;

            this.FogStart = 100;
            this.FogEnd = 600;

            // Cornflower blue
            this.FogColor = Color.CornflowerBlue.ToVector3();
        }
        #endregion

        /// <summary>
        /// Gets the id.
        /// </summary>
        [Key]
        public int Id { get; private set; }

        #region Idiotic Transforms for EF ( who the hell builds games with ef? :| )
        public float PosX
        {
            get
            {
                return this.Position.X;
            }

            private set
            {
                this.Position = new Vector3(value, this.Position.Y, this.Position.Z);
            }
        }

        public float PosY
        {
            get
            {
                return this.Position.Y;
            }

            private set
            {
                this.Position = new Vector3(this.Position.X, value, this.Position.Z);
            }
        }

        public float PosZ
        {
            get
            {
                return this.Position.Z;
            }

            private set
            {
                this.Position = new Vector3(this.Position.X, this.Position.Y, value);
            }
        }

        public float RotX
        {
            get
            {
                return
                    (float)
                    Math.Atan2(
                        -2 * (this.Rotation.Y * this.Rotation.Z - this.Rotation.W * this.Rotation.X),
                        this.Rotation.W * this.Rotation.W - this.Rotation.X * this.Rotation.X
                        - this.Rotation.Y * this.Rotation.Y + this.Rotation.Z * this.Rotation.Z);
            }

            private set
            {
                this.rotX = value;
                this.GenerateQuaternion();
            }
        }

        public float RotY
        {
            get
            {
                return (float)Math.Asin(2 * (this.Rotation.X * this.Rotation.Z + this.Rotation.W * this.Rotation.Y));
            }

            private set
            {
                this.rotY = value;
                this.GenerateQuaternion();
            }
        }

        public float RotZ
        {
            get
            {
                return
                    (float)
                    Math.Atan2(
                        -2 * (this.Rotation.X * this.Rotation.Y - this.Rotation.W * this.Rotation.Z),
                        this.Rotation.W * this.Rotation.W + this.Rotation.X * this.Rotation.X
                        - this.Rotation.Y * this.Rotation.Y - this.Rotation.Z * this.Rotation.Z);
            }

            private set
            {
                this.rotZ = value;
                this.GenerateQuaternion();
            }
        }
        #endregion

        #region Public World Transformations
        /// <summary>
        /// Gets or sets the position of the entity.
        /// </summary>
        [NotMapped]
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

        [NotMapped]
        public Quaternion Rotation
        {
            get
            {
                return this.rotation;
            }

            set
            {
                this.rotation = value;
                this.RotationMatrix = Matrix.CreateFromQuaternion(this.rotation);
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
        public float Weight { get; set; }

        /// <summary>
        /// Gets the transformation matrix (entity position with rotation and scale applied).
        /// </summary>
        [NotMapped]
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
        #endregion

        #region Model Data
        /// <summary>
        /// Gets or sets the model of the entity.
        /// </summary>
        [NotMapped]
        public Model Model
        {
            get
            {
                return this.model;
            }

            protected set
            {
                this.model = value;
            }
        }
        #endregion

        #region Owner Data

        /// <summary>
        /// Gets the owner.
        /// </summary>
        public virtual Player Player { get; private set; }
        #endregion

        #region Shader Options
        /// <summary>
        /// Gets or sets a value indicating whether 
        /// the entity is highlighted by the mouse cursor
        /// </summary>
        [NotMapped]
        public bool IsHighlighted { get; set; }

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
        #endregion

        #region Private World Transforms
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
        #endregion

        #region Gravity Methods
        /// <summary>
        /// Snaps the entity Y position to the terrain Y at the specified point
        /// </summary>
        /// <param name="terrain">
        /// The terrain.
        /// </param>
        public void SnapToTerrainHeight(ITerrain terrain)
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
        public void GravityAffect(GameTime gameTime, ITerrain terrain)
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
        #endregion

        #region Model Loaders
        /// <summary>
        /// The load model.
        /// </summary>
        public abstract void LoadModel();
        #endregion

        #region Draw
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
            foreach (ModelMesh mesh in this.Model.Meshes)
            {
                List<Vector3> originalColors = new List<Vector3>();
                foreach (BasicEffect effect in mesh.Effects)
                {
                    Light.Sunlight(effect, effect.SpecularColor);

                    effect.PreferPerPixelLighting = true;

                    if (this.IsHighlighted)
                    {
                        originalColors.Add(effect.DiffuseColor);
                        effect.DiffuseColor = Color.LightGreen.ToVector3();
                    }

                    effect.FogEnabled = true;
                    effect.FogColor = this.FogColor;
                    effect.FogStart = this.FogStart;
                    effect.FogEnd = this.FogEnd;
                    
                    effect.World = this.TransformationMatrix;        
                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                }

                mesh.Draw();

                if (this.IsHighlighted)
                {
                    for (int i = 0; i < originalColors.Count; i++)
                    {
                        BasicEffect effect = mesh.Effects[i] as BasicEffect;
                        if (effect != null)
                        {
                            effect.DiffuseColor = originalColors[i];
                        }
                    }
                }
            }
        }
        #endregion

        #region Utility Methods

        /// <summary>
        /// Creates a quaternion based on euler angles.
        /// Only used when entity is initialized from Entity Framework.
        /// </summary>
        private void GenerateQuaternion()
        {
            if (this.rotX == null || this.rotY == null || this.rotZ == null)
            {
                return;
            }

            float c1 = (float)Math.Cos(this.rotX.Value / 2);
            float s1 = (float)Math.Sin(this.rotX.Value / 2);
            float c2 = (float)Math.Cos(this.rotY.Value / 2);
            float s2 = (float)Math.Sin(this.rotY.Value / 2);
            float c3 = (float)Math.Cos(this.rotZ.Value / 2);
            float s3 = (float)Math.Sin(this.rotZ.Value / 2);

            this.rotation.W = c1 * c2 * c3 - s1 * s2 * s3;
            this.rotation.X = s1 * c2 * c3 + c1 * s2 * s3;
            this.rotation.Y = c1 * s2 * c3 - s1 * c2 * s3;
            this.rotation.Z = c1 * c2 * s3 + s1 * s2 * c3;

            this.RotationMatrix = Matrix.CreateFromQuaternion(this.Rotation);
            this.transformationMatrixDirty = true;
        }
        #endregion
    }
}