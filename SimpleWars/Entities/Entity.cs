namespace SimpleWars.Entities
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class Entity
    {
        private Model model;

        private Vector3 position;

        private Vector3 rotation;

        private Matrix rotationMatrix;

        private float scale;

        protected Entity(Model model, Vector3 position, float scale = 1)
        : this(model, position, Vector3.Zero, scale)
        {
        }

        protected Entity(Model model, Vector3 position, Vector3 rotation, float scale = 1)
        {
            this.Model = model;
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;

            this.RotationMatrix = Matrix.Identity;
        }

        public void Move(Vector3 direction)
        {
            this.Position += direction;
        }

        public void Rotate(Vector3 rotation)
        {
            this.Rotation += rotation;

            this.CalculateRotationMatrix();
        }

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

        public Vector3 Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return this.rotation;
            }

            set
            {
                this.rotation = value;
            }
        }

        public float Scale
        {
            get
            {
                return this.scale;
            }

            set
            {
                this.scale = value;
            }
        }

        public Matrix RotationMatrix
        {
            get
            {
                return this.rotationMatrix;
            }

            private set
            {
                this.rotationMatrix = value;
            }
        }    

        //public Matrix WorldMatrix => Matrix.CreateTranslation(-this.Position.Y, this.Position.X, this.Position.Z);
        public Matrix WorldMatrix => Matrix.CreateTranslation(this.Position);

        public Matrix ScaleMatrix => Matrix.CreateScale(this.Scale);

        public virtual Matrix GetTransformationMatrix()
        {
            return this.ScaleMatrix * this.RotationMatrix * this.WorldMatrix;
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

            this.RotationMatrix =  rotX * rotY * rotZ;
        }
    }
}