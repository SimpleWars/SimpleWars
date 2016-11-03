using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using SimpleWars.Displays;
using SimpleWars.InputManager;

public class CameraPerspective
{
    private const float CameraSpeed = 20;
    private const float Smoothing = 10;
    public CameraPerspective(float aspectRatio, Vector3 lookAt)
        : this(aspectRatio, MathHelper.PiOver4, lookAt, Vector3.Backward, Vector3.Up, -3f, 0.1f, 500f)
    { }

    public CameraPerspective(
            float aspectRatio, float fieldOfView, Vector3 lookAt, 
            Vector3 up, Vector3 forward, float constPitch, 
            float nearPlane, float farPlane)
    {
        this.aspectRatio = aspectRatio;
        this.fieldOfView = fieldOfView;
        this.lookAt = lookAt;
        this.Up = up;
        this.Forward = forward;
        this.nearPlane = nearPlane;
        this.farPlane = farPlane;

        this.ChangeConstPitch(constPitch);
    }

    /// <summary>
    /// Recreates our view matrix, then signals that the view matrix
    /// is clean.
    /// </summary>
    private void ReCreateViewMatrix()
    {
        // Calculate the relative position of the camera   
        // Yaw and roll are swapped because I use Z for up vector          
        // If pitch changes it will also change the rotation axis for yaw           
        this.position = Vector3.Transform(this.Forward, Matrix.CreateFromYawPitchRoll(0, 0, this.yaw));

        this.position *= Smoothing;
        this.position += this.lookAt;

        // Calculate a new viewmatrix
        // lookAt + constPitch pitches the camera in the desired direction without affecting the yaw axis.
        this.viewMatrix = Matrix.CreateLookAt(this.position, this.lookAt + this.constPitch, this.Up);
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

        //Camera movement when mouse is very close to any edge of the game window
        if (xRatio < 0.02f && xRatio > 0)
        {
            this.MoveCameraRight(-movement);
        }
        else if (xRatio > 0.98f && xRatio < 1)
        {
            this.MoveCameraRight(movement);
        }

        if (yRatio < 0.02f && yRatio > 0)
        {
            this.MoveCameraForward(movement);
        }
        else if (yRatio > 0.98f && yRatio < 1)
        {
            this.MoveCameraForward(-movement);
        }

        // Camera movement with WASD (third person effect)
        //if (Input.Instance.KeyDown(Keys.W))
        //{
        //    this.MoveCameraForward(movement);
        //}
        //if (Input.Instance.KeyDown(Keys.A))
        //{
        //    this.MoveCameraLeft(movement);
        //}
        //if (Input.Instance.KeyDown(Keys.S))
        //{
        //    this.MoveCameraBackward(movement);
        //}
        //if (Input.Instance.KeyDown(Keys.D))
        //{
        //    this.MoveCameraRight(movement);
        //}

        float timeFraction = (float)gameTime.ElapsedGameTime.TotalSeconds;
        float deltaX = Input.Instance.PrevMountPos.X - Input.Instance.MousePos.X;
        float deltaY = Input.Instance.PrevMountPos.Y - Input.Instance.MousePos.Y;

        if (Input.Instance.MiddleButtonHold())
        {
            // Yaw (look left, right) with mouse.
            this.Yaw += deltaX * timeFraction * 0.1f;

            // Pitch(look up, down) with mouse. 
            this.ChangeConstPitch(deltaY * timeFraction * 0.8f);
        }

        if (Input.Instance.RightMouseHold())
        {
            this.MoveCameraForward(-deltaY * 0.04f);
            this.MoveCameraRight(deltaX * 0.04f);
        }

        // Move up and down at an angle => zooming illusion
        if (scroll > 0)
        {
            this.MoveCameraUp(-1);
            this.MoveCameraForward(1);
        }
        else if (scroll < 0)
        {
            
            this.MoveCameraUp(1);
            this.MoveCameraForward(-1);
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


    private void MoveCameraForward(float amount)
    {
        Vector3 forward = Vector3.Normalize(this.LookAt - this.Position);
        forward.Z = 0;
        forward.Normalize();

        this.LookAt += forward * amount;
    }

    private void MoveCameraUp(float amount)
    {
        this.LookAt += new Vector3(0, 0, amount);
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
        }
    }

    private Vector3 constPitch;

    public void ChangeConstPitch(float amount)
    {
        this.constPitch += this.Up * amount;
        this.viewMatrixDirty = true;
    }

    private float yaw;
    public float Yaw
    {
        get { return this.yaw; }
        set
        {
            this.yaw = value;
            this.viewMatrixDirty = true;
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

    private Vector3 Forward { get; set; }

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