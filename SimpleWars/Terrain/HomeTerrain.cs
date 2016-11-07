namespace SimpleWars.Terrain
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Camera;
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
        /// Gets the height at the specific point of the terrain.
        /// </summary>
        public float[,] Heights { get; private set; }
        
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
            this.effect.EnableDefaultLighting();
            foreach (var pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                //this.device.DrawUserPrimitives(PrimitiveType.TriangleList, this.terrainVertices, 0, 3);
                this.device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, this.terrainVertices, 0, this.terrainVertices.Length, this.indices, 0, this.indices.Length / 3);
            }
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
        /// Initializes terrain of flat square 2d terrain in 3d space and maps a texture to it.
        /// Prepared for Perlin Noise height mapping
        /// </summary>
        private void InitTerrain()
        {
            HeightGenerator generator = new HeightGenerator();

            int size = 800;

            int vertexCount = 128;

            int count = vertexCount * vertexCount;

            this.Heights = new float[vertexCount, vertexCount];

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
                    vertices[vertexPointer * 3] = -(float)j / ((float)vertexCount - 1) * size;
                    float height = this.GetHeight(j, i, generator);
                    vertices[(vertexPointer * 3) + 1] = height;
                    this.Heights[j, i] = height;
                    vertices[(vertexPointer * 3) + 2] = -(float)i / ((float)vertexCount - 1) * size;

                    // Normals
                    Vector3 normal = this.CalculateNormal(j, i, generator);
                    normals[vertexPointer * 3] = normal.X;
                    normals[(vertexPointer * 3) + 1] = normal.Y;
                    normals[(vertexPointer * 3) + 2] = normal.Z;

                    // Texture Coords
                    textureCoords[vertexPointer * 2] = 
                        (float)j / ((float)vertexCount - 1) * size;
                    textureCoords[(vertexPointer * 2) + 1] = 
                        (float)i / ((float)vertexCount - 1) * size;


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
                    size / textureX, size / textureY);
            }

            this.effect = new BasicEffect(this.device);
            this.effect.TextureEnabled = true;
            this.effect.Texture = this.texture;
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