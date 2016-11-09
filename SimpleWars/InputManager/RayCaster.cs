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

        /// <summary>
        /// The number of binary splits in the search
        /// </summary>
        private const uint BinarySplits = 50;

        private const float SeamlessDistance = 0.0001f;

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
        public static Vector3 GetTerrainPoint(
            Matrix projectionMatrix, 
            Matrix viewMatrix, 
            HomeTerrain terrain)
        {
            Ray ray = CastRay(projectionMatrix, viewMatrix);

            Vector3 currentTerrainPoint = BinarySplitSearch(0, Range, ray, terrain, BinarySplits);

            if (!IsIntersectionInRange(0, Range, ray, terrain))
            {
                currentTerrainPoint.Y = terrain.GetWorldHeight(currentTerrainPoint.X, currentTerrainPoint.Z);
            }
          
            return currentTerrainPoint;          
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
        /// The number of splits for the binary search.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        private static Vector3 BinarySplitSearch(float start, float finish, Ray ray, HomeTerrain terrain, uint binarySplits = 0)
        {
            Vector3[] endpoints = new Vector3[binarySplits + 1];

            // The section size of each split
            float sectionSize = binarySplits > 0 ? (finish - start) / binarySplits : finish - start;
            for (uint i = 0; i < binarySplits; i++)
            {
                float s = i * sectionSize;
                float f = (i + 1) * sectionSize;

                endpoints[i] = BinarySearch(s, f, ray, terrain);
            }

            endpoints[endpoints.Length - 1] = BinarySearch(start, finish, ray, terrain);

            // Calculates the best result from all splits and master
            Vector3 winner = endpoints[0];
           
            foreach (var endpoint in endpoints)
            {
                float winnerHeight = terrain.GetWorldHeight(winner.X, winner.Z);
                Vector3 winnerVertice = new Vector3(winner.X, winnerHeight, winner.Z);
                float winnerDistance = Vector3.Distance(winner, winnerVertice);

                if (winnerDistance <= SeamlessDistance)
                {
                    return winnerVertice;
                }

                float height = terrain.GetWorldHeight(endpoint.X, endpoint.Z);
                Vector3 currentVertice = new Vector3(endpoint.X, height, endpoint.Z);           
                float currentDistance = Vector3.Distance(endpoint, currentVertice);
       
                if (currentDistance < winnerDistance)
                {
                    winner = endpoint;
                }
            }

            return winner;           
        }

        /// <summary>
        /// Simple binary search
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
                    return GetPointOnRay(ray, half);
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