namespace SimpleWars.InputManager
{
    using System.Diagnostics;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Displays;

    public static class RayCaster
    {
        private static Ray ray;

        private static void CastRay(Matrix projectionMatrix, Matrix viewMatrix)
        {
            GraphicsDevice device = DisplayManager.Instance.GraphicsDevice;
           
            float mouseX = Input.Instance.MousePos.X;
            float mouseY = Input.Instance.MousePos.Y;

            Vector3 nearPoint = device.Viewport.Unproject(
                new Vector3(mouseX, mouseY, 0),
                projectionMatrix,
                viewMatrix,
                Matrix.Identity);

            Vector3 farPoint = device.Viewport.Unproject(
                new Vector3(mouseX, mouseY, 1),
                projectionMatrix,
                viewMatrix,
                Matrix.Identity);

            Vector3 direction = Vector3.Normalize(farPoint - nearPoint);

            ray = new Ray(nearPoint, direction);
        }

        public static Ray GetMouseRay(Matrix projectionMatrix, 
            Matrix viewMatrix)
        {
            CastRay(projectionMatrix, viewMatrix);
            Debug.WriteLine("pos: " + ray.Position);
            Debug.WriteLine("dir: " + ray.Direction);
            return ray;
        }
    }
}