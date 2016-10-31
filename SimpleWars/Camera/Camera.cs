namespace SimpleWars.Camera
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.InputManager;

    public class Camera
    {
        private readonly Vector3 upVector = Vector3.UnitZ;

        private const float FieldOfView = MathHelper.PiOver4;
        private const float NearPlaneDistance = 0.1f;
        private const float FarPlaneDistance = 1000f;
        private const float AspectRatio = 1280 / (float)720;

        private static readonly Vector3 LookAt = new Vector3(0, -1, -0.5f);

        private Vector3 position = new Vector3(0, 20, 10);

        private static float angle = 0.0f;

        public Matrix ViewMatrix
        {
            get
            {
                var lookAtVector = LookAt;
                var rotationMatrix = Matrix.CreateRotationZ(angle);

                lookAtVector = Vector3.Transform(lookAtVector, rotationMatrix);
                lookAtVector += this.position;

                return Matrix.CreateLookAt(this.position, lookAtVector, this.upVector);
            }
        }

        public Matrix ProjectionMatrix => Matrix.CreatePerspectiveFieldOfView(
            FieldOfView, AspectRatio, NearPlaneDistance, FarPlaneDistance);

        public void Update(GameTime gameTime)
        {
            if (!Input.Instance.RightMouseHold())
                return;

        
            float xRatio = Input.Instance.MousePos().X / (float)1280;
            float yRatio = Input.Instance.MousePos().Y / (float)720;

            if (xRatio < 0.2f)
            {
                angle += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (xRatio < 0.8f)
            {
                if (yRatio < 0.5f)
                {
                    var forwardVector = new Vector3(0, -1, 0);

                    var rotationMatrix = Matrix.CreateRotationZ(angle);
                    forwardVector = Vector3.Transform(forwardVector, rotationMatrix);

                    const float UnitsPerSecond = 5;

                    this.position += forwardVector * UnitsPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    var backwardVector = new Vector3(0, 1, 0);

                    var rotationMatrix = Matrix.CreateRotationZ(angle);
                    backwardVector = Vector3.Transform(backwardVector, rotationMatrix);

                    const float UnitsPerSecond = 5;

                    this.position += backwardVector * UnitsPerSecond *
                        (float)gameTime.ElapsedGameTime.TotalSeconds;                
                }
            }
            else
            {
                angle -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

        }
    }
}