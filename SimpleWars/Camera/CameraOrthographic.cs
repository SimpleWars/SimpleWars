namespace SimpleWars.Camera
{
    using Microsoft.Xna.Framework;

    using SimpleWars.DisplayManagement;
    using SimpleWars.InputManagement;

    public class CameraOrthographic
    {
        private const float NearPlaneDistance = -2000;
        private const float FarPlaneDistance = 2000;
        private const float CameraSpeed = 4f;

        private const float ProjectionWidth = 60;
        private const float ProjectionHeight = 30;

        private readonly float width;
        private readonly float height;

        private readonly Vector3 upVector = Vector3.Backward;
        private readonly Vector3 moveCameraLeft = new Vector3(-1, 0, 0);
        private readonly Vector3 moveCameraRight = new Vector3(1, 0, 0);
        private readonly Vector3 moveCameraUp = new Vector3(0, 0, -1);
        private readonly Vector3 moveCameraDown = new Vector3(0, 0, 1);

        public CameraOrthographic()
        {
            this.width = DisplayManager.Instance.Dimensions.X;
            this.height = DisplayManager.Instance.Dimensions.Y;

            this.ViewMatrix = Matrix.CreateLookAt(new Vector3(0, 120, 60), Vector3.Zero, this.upVector);
        }

        public Matrix ViewMatrix { get; private set; }

        public Matrix ProjectionMatrix => Matrix.CreateOrthographic(
            ProjectionWidth, ProjectionHeight, NearPlaneDistance, FarPlaneDistance);

        public void Update(GameTime gameTime)
        {
            float xRatio = Input.MousePos.X / this.width;
            float yRatio = Input.MousePos.Y / this.height;

            Vector3 movement = Vector3.Zero;

            if (xRatio < 0.05f && xRatio > 0)
            {
                movement += this.moveCameraRight;
            }
            else if (xRatio > 0.95f && xRatio < 1)
            {
                movement += this.moveCameraLeft;
            }

            if (yRatio < 0.05f && yRatio > 0)
            {
                movement += this.moveCameraUp;
            }
            else if (yRatio > 0.95f && yRatio < 1)
            {
                movement += this.moveCameraDown;
            }


            this.CalculateCameraMovement(movement, gameTime);
        }

        private void CalculateCameraMovement(Vector3 movementVector, GameTime gameTime)
        {
            this.ViewMatrix =
                this.ViewMatrix
                * Matrix.CreateTranslation(
                    movementVector
                    * (float)gameTime.ElapsedGameTime.TotalSeconds
                    * CameraSpeed);
        }
    }
}