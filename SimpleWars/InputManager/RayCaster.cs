namespace SimpleWars.InputManager
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
        private const float Range = 100f;

        /// <summary>
        /// The search iterations for the binary search
        /// </summary>
        private const int SearchIterations = 100;

        private const uint BinarySplits = 50;

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
                Vector3 currentTerrainPoint = BinarySplitSearch(0, Range, ray, terrain, BinarySplits);
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
        /// Custom split binary search implementation.
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
        /// <param name="binarySplits">
        /// The number of recursive splits for the binary search.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        private static Vector3 BinarySplitSearch(float start, float finish, Ray ray, HomeTerrain terrain, uint binarySplits = 0)
        {
            // I'm preventing stack overflow
            // if someone goes crazy with the splits
            if (binarySplits > 256)
            {
                binarySplits = 256;
            }

            Vector3[] endpoints = new Vector3[binarySplits + 1];


            // The section size of each split
            float sectionSize = binarySplits > 0 ? (finish - start) / binarySplits : finish - start;
            for (int i = 0; i < binarySplits; i++)
            {
                float s = i * sectionSize;
                float f = (i + 1) * sectionSize;

                endpoints[i] = BinarySplitSearch(s, f, ray, terrain);
            }

            // Normal binary search used by each split
            int count = 0;
            while (true)
            {
                float half = start + ((finish - start) / 2f);

                if (count >= SearchIterations)
                {
                    Vector3 endPoint = GetPointOnRay(ray, half);
                    endpoints[endpoints.Length - 1] = endPoint;
                    break;
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

            // If it's part of the recursive splits returns it directly
            if (binarySplits == 0)
            {
                return endpoints[0];
            }

            // Calculates the best result from all splits and master
            Vector3 winner = endpoints[0];
            foreach (var endpoint in endpoints)
            {
                float height = terrain.GetWorldHeight(endpoint.X, endpoint.Z);
                Vector3 currentTerrainVertice = new Vector3(endpoint.X, height, endpoint.Z);

                float winnerHeight = terrain.GetWorldHeight(winner.X, winner.Z);
                Vector3 winnerTerrainVertice = new Vector3(winner.X, winnerHeight, winner.Z);

                if (Vector3.Distance(endpoint, currentTerrainVertice)
                    < Vector3.Distance(winner, winnerTerrainVertice))
                {
                    winner = endpoint;
                }
            }

            return winner;           
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