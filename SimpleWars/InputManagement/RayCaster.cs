namespace SimpleWars.InputManagement
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.DisplayManagement;
    using SimpleWars.Terrain;

    /// <summary>
    /// The ray caster.
    /// </summary>
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

        /// <summary>
        /// The seamless distance.
        /// Usually the binary search achieves precision of 1x10^-7
        /// so its set to pretty high value.
        /// </summary>
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
            Terrain terrain)
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
            return ray.Position + (ray.Direction * distance);
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
            Terrain terrain)
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
        private static bool IsUnderGround(Vector3 testPoint, Terrain terrain)
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
        private static Vector3 BinarySplitSearch(
            float start, 
            float finish, 
            Ray ray, 
            Terrain terrain, 
            uint binarySplits = 1)
        {
            binarySplits = binarySplits == 0 ? 1 : binarySplits;

            Vector3[] endpoints = new Vector3[binarySplits];

            // The section size of each split
            float sectionSize = binarySplits > 0 ? (finish - start) / binarySplits : finish - start;

            bool found = false;
            int iterations = 0;

            for (uint i = 0; i < binarySplits; i++)
            {
                iterations++;
                float s = i * sectionSize;
                float f = (i + 1) * sectionSize;

                bool intersectionFound = false;
                Vector3 endpoint = BinarySearch(s, f, ray, terrain, out intersectionFound);

                if (intersectionFound)
                {
                    found = true;
               
                    float height = terrain.GetWorldHeight(endpoint.X, endpoint.Z);
                    Vector3 vertice = new Vector3(endpoint.X, height, endpoint.Z);
                    float distance = Vector3.Distance(endpoint, vertice);

                    // If the point found is in the seamless range
                    // skip the rest of the search and return it
                    if (distance <= SeamlessDistance)
                    {
                        return endpoint;
                    }
                }

                endpoints[i] = endpoint;
            }

            // If no point was in intersection range with terrain during search
            // returns furthest point in range
            if (!found)
            {               
                Vector3 furthestEndpoint = BinarySearch(start, finish, ray, terrain, out found);

                return furthestEndpoint;
            }

            // Calculates the best result from all splits
            // Only reachable if no seamless result is found during split search
            Vector3 winner = endpoints[0];

            foreach (var endpoint in endpoints)
            {
                float winnerHeight = terrain.GetWorldHeight(winner.X, winner.Z);
                Vector3 winnerVertice = new Vector3(winner.X, winnerHeight, winner.Z);
                float winnerDistance = Vector3.Distance(winner, winnerVertice);

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
        /// <param name="intersectionFound">
        /// Out parameter indicating whether any of the points 
        /// found during search
        /// was in intersection range
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        private static Vector3 BinarySearch(
            float start, 
            float finish, 
            Ray ray, 
            Terrain terrain, 
            out bool intersectionFound)
        {
            int count = 0;
            bool found = false;

            while (true)
            {
                float half = start + ((finish - start) / 2f);

                if (count >= SearchIterations)
                {
                    intersectionFound = found;

                    return GetPointOnRay(ray, half);
                }

                if (IsIntersectionInRange(start, half, ray, terrain))
                {
                    found = true;

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
           
            float mouseX = Input.MousePos.X;
            float mouseY = Input.MousePos.Y;

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