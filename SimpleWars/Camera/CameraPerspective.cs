namespace SimpleWars.Camera
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    using SimpleWars.DisplayManagement;
    using SimpleWars.Environment.Terrain;
    using SimpleWars.GUI.Interfaces;
    using SimpleWars.Input;

    /// <summary>
    /// Perspective camera in 3D space.
    /// </summary>
    public class CameraPerspective
    {
        /// <summary>
        /// The camera speed.
        /// </summary>
        private const float CameraSpeed = 20;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraPerspective"/> class.
        /// </summary>
        /// <param name="aspectRatio">
        /// The screen aspect ratio (width / height).
        /// </param>
        /// <param name="position">
        /// The initial camera position.
        /// </param>
        public CameraPerspective(float aspectRatio, Vector3 position)
            : this(aspectRatio, MathHelper.PiOver4, position, Quaternion.Identity, 0.1f, 1000f)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraPerspective"/> class.
        /// </summary>
        /// <param name="aspectRatio">
        /// The screen aspect ratio
        /// </param>
        /// <param name="position">
        /// The position of the camera
        /// </param>
        /// <param name="rotation">
        /// The rotation of the camera
        /// </param>
        public CameraPerspective(float aspectRatio, Vector3 position, Quaternion rotation)
            : this(aspectRatio, MathHelper.PiOver4, position, rotation, 0.1f, 1000f)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraPerspective"/> class.
        /// </summary>
        /// <param name="aspectRatio">
        /// The screen aspect ratio (width / height).
        /// </param>
        /// <param name="fieldOfView">
        /// The perspective projection field of view.
        /// </param>
        /// <param name="position">
        /// The inital camera position.
        /// </param>
        /// <param name="rotation">
        /// The initial camera rotation.
        /// </param>
        /// <param name="nearPlane">
        /// The projection matrix near plane.
        /// </param>
        /// <param name="farPlane">
        /// The projection matrix far plane.
        /// </param>
        public CameraPerspective(
            float aspectRatio,
            float fieldOfView,
            Vector3 position,
            Quaternion rotation,
            float nearPlane, 
            float farPlane)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.aspectRatio = aspectRatio;
            this.fieldOfView = fieldOfView;
            this.nearPlane = nearPlane;
            this.farPlane = farPlane;
        }

        /// <summary>
        /// Recreates our view matrix, then signals that the view matrix
        /// is clean.
        /// </summary>
        private void ReCreateViewMatrix()
        {
            this.viewMatrix = Matrix.CreateTranslation(-this.Position) 
                * Matrix.CreateFromQuaternion(this.Rotation);
            this.viewMatrixDirty = false;
        }

        /// <summary>
        /// Recreates our projection matrix, then signals that the projection
        /// matrix is clean.
        /// </summary>
        private void ReCreateProjectionMatrix()
        {
            this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(this.fieldOfView, this.AspectRatio, this.NearPlane, this.FarPlane);
            this.projectionMatrixDirty = false;
        }

        /// <summary>
        /// Updates the camera when called.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public void Update(GameTime gameTime, Terrain terrain)
        {
            float xRatio = Input.MousePos.X / DisplayManager.Instance.Dimensions.X;
            float yRatio = Input.MousePos.Y / DisplayManager.Instance.Dimensions.Y;
            int scroll = Input.MouseScroll;
            float timeFraction = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float movement = timeFraction * CameraSpeed;

            //Camera movement when mouse is close to any edge of the game window
            if (xRatio < 0.02f && xRatio > 0)
            {
                this.MoveLeft(-movement, terrain);
            }
            else if (xRatio > 0.98f && xRatio < 1)
            {
                this.MoveLeft(movement, terrain);
            }

            if (yRatio < 0.02f && yRatio > 0)
            {
                this.MoveForward(-movement, terrain);
            }
            else if (yRatio > 0.98f && yRatio < 1)
            {
                this.MoveForward(movement, terrain);
            }

            //if (Input.Instance.KeyDown(Keys.W))
            //{
            //    this.MoveForward(movement);
            //}
            //if (Input.Instance.KeyDown(Keys.A))
            //{
            //    this.MoveLeft(-movement);
            //}
            //if (Input.Instance.KeyDown(Keys.S))
            //{
            //    this.MoveForward(-movement);
            //}
            //if (Input.Instance.KeyDown(Keys.D))
            //{
            //    this.MoveLeft(movement);
            //}

            if (Input.KeyPressed(Keys.Q))
            {
                this.SnapView();
            }

            float deltaX = Input.PreviousMousePos.X - Input.MousePos.X;
            float deltaY = Input.PreviousMousePos.Y - Input.MousePos.Y;

            if (Input.MiddleButtonHold())
            {
                this.TurnRight(deltaX * timeFraction * 2);

                this.TurnUp(-deltaY * timeFraction * 2);
            }

            if (Input.RightMouseHold())
            {
                this.MoveLeft(movement * deltaX * 0.1f, terrain);

                this.MoveForward(movement * deltaY * 0.1f, terrain);
            }

            if (scroll > 0)
            {
                this.ZoomIn(-1, terrain);
            }
            else if (scroll < 0)
            {
                this.ZoomIn(1, terrain);
            }
        }

        /// <summary>
        /// Moves the camera left or right while maintaining height.
        /// </summary>
        /// <param name="amount">
        /// The amount to move (negative moves to the right, positive moves to the left)
        /// </param>
        /// <param name="terrain">
        /// The terrain
        /// </param>
        private void MoveLeft(float amount, Terrain terrain)
        {
            Vector3 axis = this.AxisX;
            Vector3 delta = this.Position + axis * amount;

            float height = terrain.GetWorldHeight(delta.X, delta.Z) + 1;

            delta.Y = delta.Y <= height ? height: delta.Y;

            this.Position = delta;
        }

        /// <summary>
        /// Moves the camera forward while maintaining height.
        /// If you want to stop the height maintenance just remove the
        /// axis.YourUpVector = 0 part
        /// </summary>
        /// <param name="amount">
        /// The amount to move (negative moves forward, positive moves backward)
        /// </param>
        /// <param name="terrain">
        /// The terrain
        /// </param>
        private void MoveForward(float amount, Terrain terrain)
        {
            Vector3 axis = this.AxisZ;
            axis.Y = 0;
            Vector3 delta = this.Position + axis * amount;

            float height = terrain.GetWorldHeight(delta.X, delta.Z) + 1;

            delta.Y = delta.Y <= height ? height : delta.Y;

            this.Position = delta;
        }

        /// <summary>
        /// Zooms camera
        /// </summary>
        /// <param name="amount">
        /// The amount to zoom (negative zooms in, positive zooms out)
        /// </param>
        /// <param name="terrain">
        /// The terrain
        /// </param>
        private void ZoomIn(float amount, Terrain terrain)
        {
            Vector3 axis = this.AxisZ;
            Vector3 delta = this.Position + axis * amount;

            float height = terrain.GetWorldHeight(delta.X, delta.Z) + 1;

            delta.Y = delta.Y <= height ? height : delta.Y;

            this.Position = delta;
        }

        /// <summary>
        /// Turns the camera left or right (yaw)
        /// It does not respect the pitch!
        /// </summary>
        /// <param name="degrees">
        /// The amount to turn (negative turns left, positive turns right)
        /// </param>
        private void TurnRight(float degrees)
        {
            float radians = MathHelper.ToRadians(degrees);
            Vector3 axis = this.AxisY;

            Quaternion rot = Quaternion.CreateFromAxisAngle(axis, radians);
            rot.X = 0;
            rot.Z = 0;

            this.Rotation *= rot;
        }

        /// <summary>
        /// Turns the camera up or down (pitch)
        /// It does not respect the yaw!
        /// </summary>
        /// <param name="degrees">
        /// The amount to turn (negative turns down, positive turns up)
        /// </param>
        private void TurnUp(float degrees)
        {
            float radians = MathHelper.ToRadians(degrees);
            Vector3 axis = this.AxisX;
            this.Rotation *= Quaternion.CreateFromAxisAngle(axis, radians);
        }

        /// <summary>
        /// Snaps camera to initial position
        /// </summary>
        private void SnapView()
        {
            this.Position = new Vector3(-50, -30, 0);
            this.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(-90))
                            * Quaternion.CreateFromAxisAngle(Vector3.Backward, MathHelper.ToRadians(-15));
        }

        /// <summary>
        /// Indicates if the view matrix needs to be recalculated
        /// </summary>
        private bool viewMatrixDirty = true;

        /// <summary>
        /// Indicates if the projection matrix needs to be recalculated
        /// </summary>
        private bool projectionMatrixDirty = true;

        /// <summary>
        /// The field of view.
        /// </summary>
        private float fieldOfView;

        /// <summary>
        /// Gets or sets the field of view.
        /// </summary>
        public float FieldOfView
        {
            get { return this.fieldOfView; }
            set
            {
                this.projectionMatrixDirty = true;
                this.fieldOfView = value;
            }
        }

        /// <summary>
        /// The aspect ratio.
        /// </summary>
        private float aspectRatio;

        /// <summary>
        /// Gets or sets the aspect ratio.
        /// </summary>
        public float AspectRatio
        {
            get { return this.aspectRatio; }
            set
            {
                this.projectionMatrixDirty = true;
                this.aspectRatio = value;
            }
        }

        /// <summary>
        /// The near plane.
        /// </summary>
        private float nearPlane;

        /// <summary>
        /// Gets or sets the near plane.
        /// </summary>
        public float NearPlane
        {
            get { return this.nearPlane; }
            set
            {
                this.projectionMatrixDirty = true;
                this.nearPlane = value;
            }
        }

        /// <summary>
        /// The far plane.
        /// </summary>
        private float farPlane;

        /// <summary>
        /// Gets or sets the far plane.
        /// </summary>
        public float FarPlane
        {
            get { return this.farPlane; }
            set
            {
                this.projectionMatrixDirty = true;
                this.farPlane = value;
            }
        }

        private Vector3 AxisX => new Vector3(this.viewMatrix.M11, this.viewMatrix.M21, this.viewMatrix.M31);

        private Vector3 AxisY => new Vector3(this.viewMatrix.M12, this.viewMatrix.M22, this.viewMatrix.M32);

        private Vector3 AxisZ => new Vector3(this.ViewMatrix.M13, this.ViewMatrix.M23, this.ViewMatrix.M33);

        /// <summary>
        /// The rotation.
        /// </summary>
        private Quaternion rotation;

        private Quaternion Rotation
        {
            get
            {
                return this.rotation;
            }

            set
            {
                this.rotation = value;
                this.viewMatrixDirty = true;
            }
        }

        /// <summary>
        /// The position.
        /// </summary>
        private Vector3 position;

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.viewMatrixDirty = true;
                this.position = value;
            }
        }

        /// <summary>
        /// The view matrix.
        /// </summary>
        private Matrix viewMatrix;

        /// <summary>
        /// Gets the view matrix.
        /// </summary>
        public Matrix ViewMatrix
        {
            get
            {
                if (this.viewMatrixDirty)
                {
                    this.ReCreateViewMatrix();
                }

                return this.viewMatrix;
            }
        }

        /// <summary>
        /// The projection matrix.
        /// </summary>
        private Matrix projectionMatrix;

        /// <summary>
        /// Gets the projection matrix.
        /// </summary>
        public Matrix ProjectionMatrix
        {
            get
            {
                if (this.projectionMatrixDirty)
                {
                    this.ReCreateProjectionMatrix();
                }

                return this.projectionMatrix;
            }
        }
    }
}