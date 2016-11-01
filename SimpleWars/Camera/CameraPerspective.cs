using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using SimpleWars.Displays;
using SimpleWars.InputManager;

public class CameraPerspective
{
    private const float CameraSpeed = 15;
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
        this.position = Vector3.Transform(Vector3.Up, Matrix.CreateFromYawPitchRoll(0, this.pitch, this.yaw));

        //// Convert the relative position to the absolute position
        this.position *= this.zoom;
        this.position += this.lookAt;
        var up = new Vector3(0, 0, 0.8f);
        //Calculate a new viewmatrix
        this.viewMatrix = Matrix.CreateLookAt(this.position, this.lookAt, this.Up - up);
        this.viewMatrixDirty = false;

        Debug.WriteLine("pos: " + this.Position.X + " " + this.Position.Y + " " + this.Position.Z);
        Debug.WriteLine("look: " + this.LookAt.X + " " + this.LookAt.Y + " " + this.LookAt.Z);
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

    public void Update(GameTime gameTime)
    {
        float xRatio = Input.Instance.MousePos.X / DisplayManager.Instance.Dimensions.X;
        float yRatio = Input.Instance.MousePos.Y / DisplayManager.Instance.Dimensions.Y;
        int scroll = Input.Instance.MouseScroll;
        var movement = (float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed;

        // Camera movement when mouse is very close to any edge of the game window
        //if (xRatio < 0.05f && xRatio > 0)
        //{
        //    this.MoveCameraLeft((float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed);
        //}
        //else if (xRatio > 0.95f && xRatio < 1)
        //{
        //    this.MoveCameraRight((float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed);
        //}

        //if (yRatio < 0.05f && yRatio > 0)
        //{
        //    this.MoveCameraForward((float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed);
        //}
        //else if (yRatio > 0.95f && yRatio < 1)
        //{
        //    this.MoveCameraBackward((float)gameTime.ElapsedGameTime.TotalSeconds * CameraSpeed);
        //}

        if (Input.Instance.KeyDown(Keys.W))
        {
            this.MoveCameraForward(movement);
        }
        if (Input.Instance.KeyDown(Keys.A))
        {
            this.MoveCameraLeft(movement);
        }
        if (Input.Instance.KeyDown(Keys.S))
        {
            this.MoveCameraBackward(movement);
        }
        if (Input.Instance.KeyDown(Keys.D))
        {
            this.MoveCameraRight(movement);
        }

        if (Input.Instance.RightMouseHold())
        {
            float deltaX = Input.Instance.PrevMountPos.X - Input.Instance.MousePos.X;          
            this.Yaw += deltaX * (float)gameTime.ElapsedGameTime.TotalSeconds * 0.1f;

            // Pitch (look up, down) change with mouse. Probably wont use it
            //float deltaY = Input.Instance.PrevMountPos.Y - Input.Instance.MousePos.Y;
            //this.Pitch += deltaY * (float)gameTime.ElapsedGameTime.TotalSeconds * 0.1f;
        }

        // Yaw (look left, right) with arrow keys
        //if (Input.Instance.KeyDown(Keys.Left))
        //{
        //    this.Yaw -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        //}
        //else if (Input.Instance.KeyDown(Keys.Right))
        //{
        //    this.Yaw += (float)gameTime.ElapsedGameTime.TotalSeconds ;
        //}

        if (scroll > 0)
        {
            this.Zoom -= 1;
        }
        else if (scroll < 0)
        {
            this.Zoom += 1;
        }
    }

    private void MoveCameraRight(float amount)
    {
        Vector3 right = Vector3.Normalize(this.LookAt - this.Position); //calculate forward
        right = Vector3.Cross(right, this.Up); //calculate the real right
        right.Z = 0;
        right.Normalize();
        this.LookAt += right * amount;
    }

    private void MoveCameraLeft(float amount)
    {
        Vector3 left = Vector3.Normalize(this.LookAt - this.Position); //calculate forward
        left = Vector3.Cross(left, this.Up); //calculate the real left
        left.Z = 0;
        left.Normalize();
        this.LookAt -= left * amount;
    }

    private void MoveCameraForward(float amount)
    {
        Vector3 forward = Vector3.Normalize(this.LookAt - this.Position);
        forward.Z = 0;
        forward.Normalize();
        this.LookAt += forward * amount;
    }

    private void MoveCameraBackward(float amount)
    {
        Vector3 backward = Vector3.Normalize(this.LookAt - this.Position);
        backward.Z = 0;
        backward.Normalize();
        this.LookAt -= backward * amount;
    }

    private bool viewMatrixDirty = true;
    private bool projectionMatrixDirty = true;

    // Those are for 1st person perspective (pitch limited to ~100 degrees)
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
            //this.pitch = value;
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

        set
        {
            this.viewMatrixDirty = true;
            this.position = value;
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