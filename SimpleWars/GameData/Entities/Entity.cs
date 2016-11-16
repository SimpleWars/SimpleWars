namespace SimpleWars.GameData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Assets;
    using SimpleWars.GameData.Entities.Interfaces;
    using SimpleWars.GameData.Terrain;
    using SimpleWars.GameData.Terrain.Interfaces;
    using SimpleWars.GameData.Terrain.Terrains;
    using SimpleWars.User;
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
        : this(position, Vector3.Zero, scale)
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
        protected Entity(Vector3 position, Vector3 rotation, float scale = 1)
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
        protected Entity(Vector3 position, Vector3 rotation, float weight, float scale)
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

        #region Idiotic ORM Transforms
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
                return this.Rotation.X;
            }

            private set
            {
                this.Rotation = new Vector3(value, this.Rotation.Y, this.Rotation.Z);
            }
        }

        public float RotY
        {
            get
            {
                return this.Rotation.Y;
            }

            private set
            {
                this.Rotation = new Vector3(this.Rotation.X, value, this.Rotation.Z);
            }
        }

        public float RotZ
        {
            get
            {
                return this.Rotation.Y;
            }

            private set
            {
                this.Rotation = new Vector3(this.Rotation.X, this.Rotation.Y, value);
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

        /// <summary>
        /// Gets or sets the rotation for the entity.
        /// </summary>
        [NotMapped]
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

        //public override bool Equals(object obj)
        //{
        //    if (!(obj is IEntity))
        //    {
        //        throw new InvalidOperationException("You are trying to equality compare entity with non entity");
        //    }

        //    return this.Id == ((IEntity)obj).Id;
        //}

        //public override int GetHashCode()
        //{
        //    return this.Id;
        //}
        #endregion
    }
}