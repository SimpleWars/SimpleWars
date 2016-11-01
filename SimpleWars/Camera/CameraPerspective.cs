using Microsoft.Xna.Framework;

using SimpleWars.Displays;
using SimpleWars.InputManager;

public class CameraPerspective
{
    private const float CameraSpeed = 4;
    public CameraPerspective(float aspectRatio, Vector3 lookAt)
        : this(aspectRatio, MathHelper.PiOver4, lookAt, Vector3.Backward, 0.1f, float.MaxValue)
    { }

    public CameraPerspective(float aspectRatio, float fieldOfView, Vector3 lookAt, Vector3 up, float nearPlane, float farPlane)
    {
        this.aspectRatio = aspectRatio;
        this.fieldOfView = fieldOfView;
        this.lookAt = lookAt;
        this.Up = up;
        this.nearPlane = nearPlane;
        this.farPlane = farPlane;
    }

    /// <summary>
    /// Recreates our view matrix, then signals that the view matrix
    /// is clean.
    /// </summary>
    private void ReCreateViewMatrix()
    {
        // Calculate the relative position of the camera                        
        this.position = Vector3.Transform(this.Up, Matrix.CreateFromYawPitchRoll(this.yaw, this.pitch, 0));

        // Convert the relative position to the absolute position
        this.position *= this.zoom;
        this.position += this.lookAt;

        //Calculate a new viewmatrix
        this.viewMatrix = Matrix.CreateLookAt(this.position, this.lookAt, this.Up);
        this.viewMatrixDirty = false;
    }

    /// <summary>
    /// Recreates our projection matrix, then signals that the projection
    /// matrix is clean.
    /// </summary>
    private void ReCreateProjectionMatrix()
    {
        this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, this.AspectRatio, this.nearPlane, this.farPlane);
        this.projectionMatrixDirty = false;
    }

    public void Update(GameTime gameTime)
    {
        //if (!Input.Instance.RightMouseHold())
        //{
        //    return;
        //}

        float xRatio = Input.Instance.MousePos.X / DisplayManager.Instance.Dimensions.X;
        float yRatio = Input.Instance.MousePos.Y / DisplayManager.Instance.Dimensions.Y;

        //float deltaX = Input.Instance.PrevMountPos.X - Input.Instance.MousePos.X;
        //float deltaY = Input.Instance.PrevMountPos.Y - Input.Instance.MousePos.Y;

        //this.Yaw += deltaX * (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;
        //this.Pitch += deltaY * (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;

        if (xRatio < 0.05f && xRatio > 0)
        {
            this.Yaw -= (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;
            this.Pitch += (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;
        }
        else if (xRatio > 0.95f && xRatio < 1)
        {
            this.Yaw += (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;
            this.Pitch -= (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;
        }

        //if (yRatio < 0.05f && yRatio > 0)
        //{
        //    movement += this.moveCameraUp;
        //}
        //else if (yRatio > 0.95f && yRatio < 1)
        //{
        //    movement += this.moveCameraDown;
        //}
    }

    public void MoveCameraRight(float amount)
    {
        Vector3 right = Vector3.Normalize(this.LookAt - this.Position); //calculate forward
        right = Vector3.Cross(right, this.Up); //calculate the real right
        right.Y = 0;
        right.Normalize();
        this.LookAt += right * amount;
    }
      
    public void MoveCameraForward(float amount)
    {
        Vector3 forward = Vector3.Normalize(this.LookAt - this.Position);
        forward.Y = 0;
        forward.Normalize();
        this.LookAt += forward * amount;
    }

    private bool viewMatrixDirty = true;
    private bool projectionMatrixDirty = true;

    private const float MinPitch = -MathHelper.PiOver2 + 0.3f;
    private const float MaxPitch = MathHelper.PiOver2 - 0.3f;
    private float pitch;
    public float Pitch
    {
        get { return this.pitch; }
        set
        {
            this.viewMatrixDirty = true;
            this.pitch = MathHelper.Clamp(value, MinPitch, MaxPitch);
        }
    }

    private float yaw;
    public float Yaw
    {
        get { return this.yaw; }
        set
        {
            this.viewMatrixDirty = true;
            this.yaw = value;
        }
    }

    private float fieldOfView;
    public float FieldOfView
    {
        get { return this.fieldOfView; }
        set
        {
            this.projectionMatrixDirty = true;
            this.fieldOfView = value;
        }
    }

    private float aspectRatio;
    public float AspectRatio
    {
        get { return this.aspectRatio; }
        set
        {
            this.projectionMatrixDirty = true;
            this.aspectRatio = value;
        }
    }

    private float nearPlane;
    public float NearPlane
    {
        get { return this.nearPlane; }
        set
        {
            this.projectionMatrixDirty = true;
            this.nearPlane = value;
        }
    }

    private float farPlane;
    public float FarPlane
    {
        get { return this.farPlane; }
        set
        {
            this.projectionMatrixDirty = true;
            this.farPlane = value;
        }
    }

    private const float MinZoom = 1;
    private const float MaxZoom = float.MaxValue;
    private float zoom = 1;
    public float Zoom
    {
        get { return this.zoom; }
        set
        {
            this.viewMatrixDirty = true;
            this.zoom = MathHelper.Clamp(value, MinZoom, MaxZoom);
        }
    }


    private Vector3 position;

    private Vector3 Position
    {
        get
        {
            if (this.viewMatrixDirty)
            {
                this.ReCreateViewMatrix();
            }
            return this.position;
        }
    }

    private Vector3 lookAt;

    private Vector3 LookAt
    {
        get { return lookAt; }
        set
        {
            this.viewMatrixDirty = true;
            this.lookAt = value;
        }
    }

    private Vector3 Up { get; set; }

    public Matrix ViewProjectionMatrix => this.ViewMatrix * this.ProjectionMatrix;

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