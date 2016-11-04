namespace SimpleWars.Camera
{
    using System.Linq.Expressions;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    using SimpleWars.Displays;
    using SimpleWars.InputManager;

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
        /// The inital camera position.
        /// </param>
        public CameraPerspective(float aspectRatio, Vector3 position)
            : this(aspectRatio, MathHelper.PiOver4, position, Vector3.Up, 0.1f, 500f)
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
        /// <param name="up">
        /// The view matrix up vector.
        /// </param>
        /// <param name="nearPlane">
        /// The projection matrix near plane.
        /// </param>
        /// <param name="farPlane">
        /// The projection matrix far plane.
        /// </param>
        public CameraPerspective(
            float aspectRatio, float fieldOfView, Vector3 position, 
            Vector3 up, 
            float nearPlane, float farPlane)
        {
            this.Position = position;
            this.Up = up;

            // Constant distance between the camera position and the look at vector
            this.LookAt = this.position - Vector3.One;


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
            this.viewMatrix = Matrix.CreateLookAt(this.Position, this.LookAt, this.Up);
            this.viewMatrixDirty = false;
        }

        /// <summary>
        /// Recreates our projection matrix, then signals that the projection
        /// matrix is clean.
        /// </summary>
        private void ReCreateProjectionMatrix()
        {
            this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(this.fieldOfView, this.AspectRatio, this.nearPlane, this.farPlane);
            this.projectionMatrixDirty = false;
        }

        /// <summary>
        /// Updates the camera when called.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public void Update(GameTime gameTime)
        {
            float xRatio = Input.Instance.MousePos.X / DisplayManager.Instance.Dimensions.X;
            float yRatio = Input.Instance.MousePos.Y / DisplayManager.Instance.Dimensions.Y;
            int scroll = Input.Instance.MouseScroll;
            float timeFraction = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float movement = timeFraction * CameraSpeed;

            //Camera movement when mouse is very close to any edge of the game window
            if (xRatio < 0.02f && xRatio > 0)
            {
                this.MoveRight(-movement);
            }
            else if (xRatio > 0.98f && xRatio < 1)
            {
                this.MoveRight(movement);
            }

            if (yRatio < 0.02f && yRatio > 0)
            {
                this.MoveForward(movement);
            }
            else if (yRatio > 0.98f && yRatio < 1)
            {
                this.MoveForward(-movement);
            }

            //if (Input.Instance.KeyDown(Keys.W))
            //{
            //    this.MoveForward(movement);
            //}
            //if (Input.Instance.KeyDown(Keys.A))
            //{
            //    this.MoveRight(-movement);
            //}
            //if (Input.Instance.KeyDown(Keys.S))
            //{
            //    this.MoveForward(-movement);
            //}
            //if (Input.Instance.KeyDown(Keys.D))
            //{
            //    this.MoveRight(movement);
            //}

            if (Input.Instance.KeyPressed(Keys.Q))
            {
                this.SnapViewAngle();
            }

            float deltaX = Input.Instance.PrevMountPos.X - Input.Instance.MousePos.X;
            float deltaY = Input.Instance.PrevMountPos.Y - Input.Instance.MousePos.Y;

            if (Input.Instance.MiddleButtonHold())
            {
                this.TurnRight(deltaX * timeFraction * 0.06f);

                this.TurnUp(-deltaY * timeFraction * 0.06f);
            }

            if (Input.Instance.RightMouseHold())
            {
                this.MoveRight(movement * deltaX * 0.1f);

                this.MoveForward(-movement * deltaY * 0.1f);
            }

            if (scroll > 0)
            {
                this.MoveUp(1);
            }
            else if (scroll < 0)
            {
                this.MoveUp(-1);
            }
        }

        /// <summary>
        /// Moves the camera left or right.
        /// </summary>
        /// <param name="amount">
        /// The amount to move (negative moves to the left, positive moves to the right)
        /// </param>
        private void MoveRight(float amount)
        {
            Vector3 forward = Vector3.Normalize(this.LookAt - this.Position);
            Vector3 right = Vector3.Cross(forward, this.Up);
            right.Normalize();

            right.Y = 0;

            right *= amount;
            this.Position += right;
            this.LookAt += right;
        }

        /// <summary>
        /// Moves the camera forward while maintaining height.
        /// If you want to stop the height maintenance just remove the
        /// forward.YourUpVector = 0 part
        /// </summary>
        /// <param name="amount">
        /// The amount to move (negative moves backwards, positive moves forward)
        /// </param>
        private void MoveForward(float amount)
        {
            Vector3 forward = Vector3.Normalize(this.LookAt - this.Position);

            forward.Y = 0;

            forward *= amount;

            this.Position += forward;
            this.LookAt += forward;
        }

        /// <summary>
        /// Moves the camera by the up axis you defined.
        /// </summary>
        /// <param name="amount">
        /// The amount to move (negative moves down, positive moves up)
        /// </param>
        private void MoveUp(float amount)
        {
            this.Position += this.Up * amount;
            this.LookAt += this.Up * amount;
        }

        /// <summary>
        /// Turns the camera left or right (yaw)
        /// It does not respect the pitch!
        /// </summary>
        /// <param name="amount">
        /// The amount to turn (negative turns left, positive turns right)
        /// </param>
        private void TurnRight(float amount)
        {
            Vector3 forward = Vector3.Normalize(this.LookAt - this.Position);
            Vector3 right = Vector3.Cross(forward, this.Up);
            right.Normalize();
            right *= amount;

            this.LookAt += right;
        }

        /// <summary>
        /// Turns the camera up or down (pitch)
        /// It does not respect the yaw!
        /// </summary>
        /// <param name="amount">
        /// The amount to turn (negative turns down, positive turns up)
        /// </param>
        private void TurnUp(float amount)
        {
            this.LookAt += this.Up * amount;
        }

        /// <summary>
        /// Snaps the look at point to (0, 0, 0)
        /// </summary>
        private void SnapViewAngle()
        {
            this.Position = new Vector3(30, 30, 30);
            this.LookAt = this.Position - Vector3.One;
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

        /// <summary>
        /// The position.
        /// </summary>
        private Vector3 position;

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        private Vector3 Position
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
        /// The look at.
        /// </summary>
        private Vector3 lookAt;

        /// <summary>
        /// Gets or sets the look at.
        /// </summary>
        private Vector3 LookAt
        {
            get
            {
                return this.lookAt;
            }

            set
            {
                this.viewMatrixDirty = true;
                this.lookAt = value;
            }
        }

        /// <summary>
        /// Gets the up vector for the camera.
        /// </summary>
        private Vector3 Up { get; }

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