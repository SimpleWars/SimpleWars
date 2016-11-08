﻿namespace SimpleWars.InputManager
{
    using System.Diagnostics;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Displays;
    using SimpleWars.Terrain;

    public static class RayCaster
    {
        /// <summary>
        /// The range of the ray cast
        /// </summary>
        private const float Range = 400f;

        /// <summary>
        /// The search iterations for the binary search
        /// </summary>
        private const int SearchIterations = 250;

        /// <summary>
        /// Gets the point of the terrain that the mouse cursor is currently casting a ray to.
        /// Returns null if the ray range is surpassed.
        /// </summary>
        /// <param name="projectionMatrix">
        /// The projection matrix.
        /// </param>
        /// <param name="viewMatrix">
        /// The view matrix.
        /// </param>
        /// <param name="terrain">
        /// The terrain.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3?"/>
        /// </returns>
        public static Vector3? GetTerrainPoint(Matrix projectionMatrix, Matrix viewMatrix, HomeTerrain terrain)
        {
            Ray ray = CastRay(projectionMatrix, viewMatrix);

            if (IsIntersectionInRange(0, Range, ray, terrain))
            {
                Vector3 currentTerrainPoint = BinarySearch(0, Range, ray, terrain);
                return currentTerrainPoint;
            }

            return null;
        }

        /// <summary>
        /// Scales the ray and returns the scaled point.
        /// </summary>
        /// <param name="ray">
        /// The ray.
        /// </param>
        /// <param name="distance">
        /// The distance.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        private static Vector3 GetPointOnRay(Ray ray, float distance)
        {
            return ray.Position + ray.Direction * distance;
        }

        /// <summary>
        /// Checks if the specified points are in the ray range.
        /// </summary>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="finish">
        /// The finish.
        /// </param>
        /// <param name="ray">
        /// The ray.
        /// </param>
        /// <param name="terrain">
        /// The terrain.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsIntersectionInRange(
            float start, 
            float finish, 
            Ray ray, 
            HomeTerrain terrain)
        {
            Vector3 startPoint = GetPointOnRay(ray, start);
            Vector3 endPoint = GetPointOnRay(ray, finish);

            return !IsUnderGround(startPoint, terrain) && IsUnderGround(endPoint, terrain);
        }

        /// <summary>
        /// Checks if the specified point is under the terrain
        /// </summary>
        /// <param name="testPoint">
        /// The test point.
        /// </param>
        /// <param name="terrain">
        /// The terrain.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsUnderGround(Vector3 testPoint, HomeTerrain terrain)
        {
            float height = terrain.GetWorldHeight(testPoint.X, testPoint.Z);

            return testPoint.Y < height;
        }

        /// <summary>
        /// A simple binary search.
        /// Returns the closest point projected from the mouse cursor
        /// to the terrain surface.
        /// </summary>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="finish">
        /// The finish.
        /// </param>
        /// <param name="ray">
        /// The ray.
        /// </param>
        /// <param name="terrain">
        /// The terrain.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        private static Vector3 BinarySearch(float start, float finish, Ray ray, HomeTerrain terrain)
        {
            int count = 0;

            while (true)
            {
                float half = start + ((finish - start) / 2f);

                if (count >= SearchIterations)
                {
                    Vector3 endPoint = GetPointOnRay(ray, half);
                    return endPoint;
                }

                if (IsIntersectionInRange(start, half, ray, terrain))
                {
                    count += 1;
                    finish = half;
                }
                else
                {
                    count += 1;
                    start = half;
                }
            }
        }

        /// <summary>
        /// Casts a ray from the mouse cursor
        /// </summary>
        /// <param name="projectionMatrix">
        /// The projection matrix.
        /// </param>
        /// <param name="viewMatrix">
        /// The view matrix.
        /// </param>
        /// <returns>
        /// The <see cref="Ray"/>.
        /// </returns>
        private static Ray CastRay(Matrix projectionMatrix, Matrix viewMatrix)
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

            return new Ray(nearPoint, direction);
        }
    }
}