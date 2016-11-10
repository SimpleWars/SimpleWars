namespace SimpleWars.Environment
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.AssetsManagement;
    using SimpleWars.DisplayManagement;
    using SimpleWars.Utils;

    /// <summary>
    /// The skybox.
    /// </summary>
    public class Skybox
    {
        /// <summary>
        /// The device.
        /// </summary>
        private readonly GraphicsDevice device;

        /// <summary>
        /// The texture.
        /// </summary>
        private readonly Texture2D texture;

        /// <summary>
        /// The cube vertices.
        /// </summary>
        private VertexPositionNormalTexture[] cubeVertices;

        /// <summary>
        /// The effect.
        /// </summary>
        private BasicEffect effect;

        /// <summary>
        /// The rotation.
        /// </summary>
        private Matrix rotation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Skybox"/> class.
        /// </summary>
        public Skybox()
        {
            this.device = DisplayManager.Instance.GraphicsDevice;

            this.texture = TexturesManager.Instance.GetTexture("Skybox", "skybox");

            this.rotation = Matrix.CreateRotationX(MathHelper.ToRadians(180));

            this.InitTexturedCube();
        }

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="projectionMatrix">
        /// The projection matrix.
        /// </param>
        /// <param name="viewMatrix">
        /// The view matrix.
        /// </param>
        public void Draw(Matrix projectionMatrix, Matrix viewMatrix)
        {
            this.effect.View = Matrix.CreateFromQuaternion(viewMatrix.Rotation);
            this.effect.Projection = projectionMatrix;
            this.effect.World = this.rotation;
            
            this.device.RasterizerState = RasterizerState.CullClockwise;

            foreach (var pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();
      
                this.device.DrawUserPrimitives(PrimitiveType.TriangleList, this.cubeVertices, 0, 12);
            }

            this.device.RasterizerState = RasterizerState.CullNone;
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public void Update(GameTime gameTime)
        {
            this.rotation *= Matrix.CreateRotationY(
                MathHelper.ToRadians(-(float)gameTime.ElapsedGameTime.TotalSeconds) * 0.4f);
        }

        /// <summary>
        /// Initializes the textured cube (skybox)
        /// </summary>
        private void InitTexturedCube()
        {
            int size = 500;
            float col = 0.25f;
            float row = 1/3f;

            this.cubeVertices = new VertexPositionNormalTexture[36];

            // Left
            this.cubeVertices[0].Position = new Vector3(-size, size, -size);
            this.cubeVertices[1].Position = new Vector3(-size, -size, -size);
            this.cubeVertices[2].Position = new Vector3(size, -size, -size);
            this.cubeVertices[3].Position = this.cubeVertices[2].Position;
            this.cubeVertices[4].Position = new Vector3(size, size, -size);
            this.cubeVertices[5].Position = this.cubeVertices[0].Position;

            this.cubeVertices[0].TextureCoordinate = new Vector2(col * 2, row * 2);
            this.cubeVertices[1].TextureCoordinate = new Vector2(col * 2, row * 1);
            this.cubeVertices[2].TextureCoordinate = new Vector2(col * 1, row * 1);
            this.cubeVertices[3].TextureCoordinate = this.cubeVertices[2].TextureCoordinate;
            this.cubeVertices[4].TextureCoordinate = new Vector2(col * 1, row * 2);
            this.cubeVertices[5].TextureCoordinate = this.cubeVertices[0].TextureCoordinate;

            // Front
            this.cubeVertices[6].Position = new Vector3(-size, -size, size);
            this.cubeVertices[7].Position = new Vector3(-size, -size, -size);
            this.cubeVertices[8].Position = new Vector3(-size, size, -size);
            this.cubeVertices[9].Position = this.cubeVertices[8].Position;
            this.cubeVertices[10].Position = new Vector3(-size, size, size);
            this.cubeVertices[11].Position = this.cubeVertices[6].Position;

            this.cubeVertices[6].TextureCoordinate = new Vector2(col * 3, row * 1);
            this.cubeVertices[7].TextureCoordinate = new Vector2(col * 2, row * 1);
            this.cubeVertices[8].TextureCoordinate = new Vector2(col * 2, row * 2);
            this.cubeVertices[9].TextureCoordinate = this.cubeVertices[8].TextureCoordinate;
            this.cubeVertices[10].TextureCoordinate = new Vector2(col * 3, row * 2);
            this.cubeVertices[11].TextureCoordinate = this.cubeVertices[6].TextureCoordinate;

            //// Back
            this.cubeVertices[12].Position = new Vector3(size, -size, -size);
            this.cubeVertices[13].Position = new Vector3(size, -size, size);
            this.cubeVertices[14].Position = new Vector3(size, size, size);
            this.cubeVertices[15].Position = this.cubeVertices[14].Position;
            this.cubeVertices[16].Position = new Vector3(size, size, -size);
            this.cubeVertices[17].Position = this.cubeVertices[12].Position;

            this.cubeVertices[12].TextureCoordinate = new Vector2(col * 1, row * 1);
            this.cubeVertices[13].TextureCoordinate = new Vector2(col * 0, row * 1);
            this.cubeVertices[14].TextureCoordinate = new Vector2(col * 0, row * 2);
            this.cubeVertices[15].TextureCoordinate = this.cubeVertices[14].TextureCoordinate;
            this.cubeVertices[16].TextureCoordinate = new Vector2(col * 1, row * 2);
            this.cubeVertices[17].TextureCoordinate = this.cubeVertices[12].TextureCoordinate;

            //// Right
            this.cubeVertices[18].Position = new Vector3(-size, -size, size);
            this.cubeVertices[19].Position = new Vector3(-size, size, size);
            this.cubeVertices[20].Position = new Vector3(size, size, size);
            this.cubeVertices[21].Position = this.cubeVertices[20].Position;
            this.cubeVertices[22].Position = new Vector3(size, -size, size);
            this.cubeVertices[23].Position = this.cubeVertices[18].Position;

            this.cubeVertices[18].TextureCoordinate = new Vector2(col * 3, row * 1);
            this.cubeVertices[19].TextureCoordinate = new Vector2(col * 3, row * 2);
            this.cubeVertices[20].TextureCoordinate = new Vector2(col * 4, row * 2);
            this.cubeVertices[21].TextureCoordinate = this.cubeVertices[20].TextureCoordinate;
            this.cubeVertices[22].TextureCoordinate = new Vector2(col * 4, row * 1);
            this.cubeVertices[23].TextureCoordinate = this.cubeVertices[18].TextureCoordinate;

            // Down
            this.cubeVertices[24].Position = new Vector3(-size, size, -size);
            this.cubeVertices[25].Position = new Vector3(size, size, -size);
            this.cubeVertices[26].Position = new Vector3(size, size, size);
            this.cubeVertices[27].Position = this.cubeVertices[26].Position;
            this.cubeVertices[28].Position = new Vector3(-size, size, size);
            this.cubeVertices[29].Position = this.cubeVertices[24].Position;

            this.cubeVertices[24].TextureCoordinate = new Vector2(col * 2, row * 2);
            this.cubeVertices[25].TextureCoordinate = new Vector2(col * 1, row * 2);
            this.cubeVertices[26].TextureCoordinate = new Vector2(col * 1, row * 3);
            this.cubeVertices[27].TextureCoordinate = this.cubeVertices[26].TextureCoordinate;
            this.cubeVertices[28].TextureCoordinate = new Vector2(col * 2, row * 3);
            this.cubeVertices[29].TextureCoordinate = this.cubeVertices[24].TextureCoordinate;

            // Up
            this.cubeVertices[30].Position = new Vector3(-size, -size, -size);
            this.cubeVertices[31].Position = new Vector3(-size, -size, size);
            this.cubeVertices[32].Position = new Vector3(size, -size, size);
            this.cubeVertices[33].Position = this.cubeVertices[32].Position;
            this.cubeVertices[34].Position = new Vector3(size, -size, -size);
            this.cubeVertices[35].Position = this.cubeVertices[30].Position;

            this.cubeVertices[30].TextureCoordinate = new Vector2(col * 2, row * 1);
            this.cubeVertices[31].TextureCoordinate = new Vector2(col * 2, row * 0);
            this.cubeVertices[32].TextureCoordinate = new Vector2(col * 1, row * 0);
            this.cubeVertices[33].TextureCoordinate = this.cubeVertices[32].TextureCoordinate;
            this.cubeVertices[34].TextureCoordinate = new Vector2(col * 1, row * 1);
            this.cubeVertices[35].TextureCoordinate = this.cubeVertices[30].TextureCoordinate;

            this.effect = new BasicEffect(this.device);
            this.effect.TextureEnabled = true;
            this.effect.Texture = this.texture;
            this.effect.PreferPerPixelLighting = true;
        }
    }
}