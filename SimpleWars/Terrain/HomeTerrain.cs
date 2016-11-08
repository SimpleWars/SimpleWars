namespace SimpleWars.Terrain
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Displays;
    using SimpleWars.Entities;

    /// <summary>
    /// The home terrain.
    /// </summary>
    public class HomeTerrain : Entity
    {
        /// <summary>
        /// The device that draws the flat 2d terrain.
        /// </summary>
        private readonly GraphicsDevice device;

        /// <summary>
        /// The texture for the flat 2d terrain.
        /// </summary>
        private readonly Texture2D texture;

        /// <summary>
        /// The flat 2d terrain terrain vertices.
        /// </summary>
        private VertexPositionNormalTexture[] terrainVertices;

        /// <summary>
        /// The effect used to draw flat 2d terrain.
        /// </summary>
        private BasicEffect effect;

        /// <summary>
        /// The vertex indices.
        /// </summary>
        private int[] indices;

        /// <summary>
        /// Gets the height at the specific point of the terrain.
        /// </summary>
        private float[,] heights { get; set; }

        /// <summary>
        /// The size of the terrain. Square shape.
        /// </summary>
        private const int Size = 800;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeTerrain"/> class.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="scale">
        /// The scale.
        /// </param>
        public HomeTerrain(Model model, Vector3 position, float scale = 1) 
            : base(model, position, scale)
        {
            this.device = DisplayManager.Instance.GraphicsDevice;

            this.InitTerrain();
        }

        public HomeTerrain(Model model, Texture2D terrainTexture, Vector3 position)
            : base(model, position)
        {
            this.device = DisplayManager.Instance.GraphicsDevice;
            this.texture = terrainTexture;

            this.InitTerrain();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeTerrain"/> class.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="rotation">
        /// The rotation.
        /// </param>
        /// <param name="scale">
        /// The scale.
        /// </param>
        public HomeTerrain(Model model, Vector3 position, Vector3 rotation, float scale = 1)
            : base(model, position, rotation, scale)
        {
            this.device = DisplayManager.Instance.GraphicsDevice;

            this.InitTerrain();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeTerrain"/> class.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="texture">
        /// The texture.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="rotation">
        /// The rotation.
        /// </param>
        /// <param name="scale">
        /// The scale.
        /// </param>
        public HomeTerrain(Model model, Texture2D terrainTexture, Vector3 position, Vector3 rotation, float scale = 1)
            : base(model, position, rotation, scale)
        {
            this.device = DisplayManager.Instance.GraphicsDevice;
            this.texture = terrainTexture;

            this.InitTerrain();
        }

        /// <summary>
        /// Calculates the height of the terrain in world space for the specified x and z
        /// </summary>
        /// <param name="x">
        /// The x coord
        /// </param>
        /// <param name="z">
        /// The z coord
        /// </param>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        public float GetWorldHeight(float x, float z)
        {
            float terrainX = x - this.Position.X;
            float terrainZ = z - this.Position.Z;
            float gridSquareSize = Size / ((float)this.heights.GetLength(0) - 1);

            int gridX = (int)Math.Floor(terrainX / gridSquareSize);
            int gridZ = (int)Math.Floor(terrainZ / gridSquareSize);

            if (gridX < 0 
                || gridZ < 0 
                || gridX >= this.heights.GetLength(0) - 1 
                || gridZ >= this.heights.GetLength(1) - 1)
            {
                return 0;
            }

            float coordX = (terrainX % gridSquareSize) / gridSquareSize;
            float coordZ = (terrainZ % gridSquareSize) / gridSquareSize;

            float height;

            if (coordX <= (1 - coordZ))
            {
                height = this.BarycentricInterpolation(
                    new Vector3(0, this.heights[gridX, gridZ], 0), 
                    new Vector3(1, this.heights[gridX + 1, gridZ], 0), 
                    new Vector3(0, this.heights[gridX, gridZ + 1], 1), 
                    new Vector2(coordX, coordZ));
            }
            else
            {
                height = this.BarycentricInterpolation(
                    new Vector3(1, this.heights[gridX + 1, gridZ], 0), 
                    new Vector3(1, this.heights[gridX + 1, gridZ + 1], 1), 
                    new Vector3(0, this.heights[gridX, gridZ + 1], 1), 
                    new Vector2(coordX, coordZ));
            }

            return height;
        }

        /// <summary>
        /// Draw flat 2d terrain in 3d space
        /// </summary>
        /// <param name="viewMatrix">
        /// The view Matrix.
        /// </param>
        /// <param name="projectionMatrix">
        /// The projection Matrix.
        /// </param>
        public void DrawProceduralTerrain(Matrix viewMatrix, Matrix projectionMatrix)
        {
            this.effect.View = viewMatrix;
            this.effect.Projection = projectionMatrix;
            this.effect.World = this.WorldMatrix;

            this.device.RasterizerState = RasterizerState.CullClockwise;
            foreach (var pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                this.device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, this.terrainVertices, 0, this.terrainVertices.Length, this.indices, 0, this.indices.Length / 3);
            }

            this.device.RasterizerState = RasterizerState.CullNone;
        }

        /// <summary>
        /// The custom terrain draw using 3D terrain model.
        /// </summary>
        /// <param name="viewMatrix">
        /// The view matrix.
        /// </param>
        /// <param name="projectionMatrix">
        /// The projection matrix.
        /// </param>
        public override void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            // This will be used for custom terrains that have many meshes on them
            // and that need to be relative to their root which is the only part of the model 
            // that has to be transformed to world space
            //Matrix[] transforms = new Matrix[this.Model.Bones.Count];
            //this.Model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (var mesh in this.Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.FogEnabled = true;
                    effect.FogStart = fogStart;
                    effect.FogEnd = fogEnd;
                    effect.FogColor = fogColor;

                    effect.PreferPerPixelLighting = true;
                    effect.TextureEnabled = true;

                    // the transform would still have to be properly rotated and scaled
                    // this is done by multiplying the scale matrix to the the rotation matrix
                    // and then to the transform matrix
                    //effect.World = transforms[mesh.ParentBone.Index];

                    // my current terrain has only 1 mesh and I'm good with default entity world transformation  
                    effect.World = this.TransformationMatrix;
                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                }

                mesh.Draw();
            }
        }

        /// <summary>
        /// The barycentric interpolation.
        /// </summary>
        /// <param name="p1">
        /// The p 1.
        /// </param>
        /// <param name="p2">
        /// The p 2.
        /// </param>
        /// <param name="p3">
        /// The p 3.
        /// </param>
        /// <param name="pos">
        /// The pos.
        /// </param>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        private float BarycentricInterpolation(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 pos)
        {
            float det = (p2.Z - p3.Z) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Z - p3.Z);
            float l1 = ((p2.Z - p3.Z) * (pos.X - p3.X) + (p3.X - p2.X) * (pos.Y - p3.Z)) / det;
            float l2 = ((p3.Z - p1.Z) * (pos.X - p3.X) + (p1.X - p3.X) * (pos.Y - p3.Z)) / det;
            float l3 = 1.0f - l1 - l2;
            return l1 * p1.Y + l2 * p2.Y + l3 * p3.Y;
        }

        /// <summary>
        /// Initializes terrain. 
        /// Applies Noise function to map the height.
        /// Random by default but will produce same results if seed is provided.
        /// </summary>
        private void InitTerrain()
        {
            // I will seed it once the game gets playable. Maybe.
            HeightGenerator generator = new HeightGenerator();

            int vertexCount = 128;

            int count = vertexCount * vertexCount;

            this.heights = new float[vertexCount, vertexCount];

            float[] vertices = new float[count * 3];
            float[] normals = new float[count * 3];
            float[] textureCoords = new float[count * 2];
            this.indices = new int[6 * (vertexCount - 1) * (vertexCount - 1)];
            int vertexPointer = 0;
    
            for (int i = 0; i < vertexCount; i++)
            {
                for (int j = 0; j < vertexCount; j++)
                {
                    // Position
                    vertices[vertexPointer * 3] = (float)j / ((float)vertexCount - 1) * Size;
                    float height = this.GetHeight(j, i, generator);
                    vertices[(vertexPointer * 3) + 1] = height;
                    this.heights[j, i] = height;
                    vertices[(vertexPointer * 3) + 2] = (float)i / ((float)vertexCount - 1) * Size;

                    // Normals
                    Vector3 normal = this.CalculateNormal(j, i, generator);
                    normals[vertexPointer * 3] = normal.X;
                    normals[(vertexPointer * 3) + 1] = normal.Y;
                    normals[(vertexPointer * 3) + 2] = normal.Z;

                    // Texture Coords
                    textureCoords[vertexPointer * 2] = 
                        (float)j / ((float)vertexCount - 1) * Size;
                    textureCoords[(vertexPointer * 2) + 1] = 
                        (float)i / ((float)vertexCount - 1) * Size;


                    vertexPointer++;
                }
            }

            int pointer = 0;
            for (int gz = 0; gz < vertexCount - 1; gz++)
            {
                for (int gx = 0; gx < vertexCount - 1; gx++)
                {
                    int topLeft = (gz * vertexCount) + gx;
                    int topRight = topLeft + 1;
                    int bottomLeft = ((gz + 1) * vertexCount) + gx;
                    int bottomRight = bottomLeft + 1;
                    this.indices[pointer++] = topLeft;
                    this.indices[pointer++] = bottomLeft;
                    this.indices[pointer++] = topRight;
                    this.indices[pointer++] = topRight;
                    this.indices[pointer++] = bottomLeft;
                    this.indices[pointer++] = bottomRight;
                }
            }

            this.terrainVertices = new VertexPositionNormalTexture[count];
                             
            for (int i = 0; i < count; i++)
            {
                float posX = vertices[i * 3];
                float posY = vertices[i * 3 + 1];
                float posZ = vertices[i * 3 + 2];
                this.terrainVertices[i].Position = new Vector3(posX, posY, posZ);

                float normX = normals[i * 3];
                float normY = normals[i * 3 + 1];
                float normZ = normals[i * 3 + 2];
                this.terrainVertices[i].Normal = new Vector3(normX, normY, normZ);

                float textureX = textureCoords[i * 2];
                float textureY = textureCoords[i * 2 + 1];
                this.terrainVertices[i].TextureCoordinate = new Vector2(
                    Size / textureX, Size / textureY);
            }

            this.effect = new BasicEffect(this.device);
            this.effect.TextureEnabled = true;
            this.effect.Texture = this.texture;
            this.effect.EnableDefaultLighting();
            this.effect.FogEnabled = true;
            this.effect.FogStart = fogStart;
            this.effect.FogEnd = fogEnd;
            this.effect.FogColor = fogColor;
        }

        /// <summary>
        /// Gets the height at the specified position on the terrain
        /// </summary>
        /// <param name="x">
        /// The x coordinate
        /// </param>
        /// <param name="z">
        /// The z coordinate
        /// </param>
        /// <param name="generator">
        /// The perlin noise height generator
        /// </param>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        private float GetHeight(int x, int z, HeightGenerator generator)
        {
            return generator.GenerateHeight(x, z);
        }

        /// <summary>
        /// Calculates the normal vector for the specified position on the terrain
        /// </summary>
        /// <param name="x">
        /// The x coordinate
        /// </param>
        /// <param name="z">
        /// The z coordinate
        /// </param>
        /// <param name="generator">
        /// The generator perlin noise height generator
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        private Vector3 CalculateNormal(int x, int z, HeightGenerator generator)
        {
            float heightL = this.GetHeight(x - 1, z, generator);
            float heightR = this.GetHeight(x + 1, z, generator);
            float heightD = this.GetHeight(x, z - 1, generator);
            float heightU = this.GetHeight(x, z - 1, generator);
            Vector3 normal = new Vector3(heightL - heightR, 2f, heightD - heightU);
            normal.Normalize();
            return normal;
        }
    }
}