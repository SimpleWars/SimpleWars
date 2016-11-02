namespace SimpleWars.Terrain
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Camera;
    using SimpleWars.Displays;

    /// <summary>
    /// The terrain.
    /// </summary>

    public class Terrain
    {
        /// <summary>
        /// The camera.
        /// </summary>
        //private readonly CameraOrthographic camera;

        private readonly CameraPerspective camera;

        /// <summary>
        /// The device.
        /// </summary>
        private readonly GraphicsDevice device;

        /// <summary>
        /// The texture.
        /// </summary>
        private readonly Texture2D texture;

        /// <summary>
        /// The terrain vertices.
        /// </summary>
        private VertexPositionNormalTexture[] terrainVertices;

        /// <summary>
        /// The effect.
        /// </summary>
        private BasicEffect effect;

        /// <summary>
        /// Initializes a new instance of the <see cref="Terrain"/> class.
        /// </summary>
        /// <param name="camera">
        /// The camera.
        /// </param>
        /// <param name="texture">
        /// The texture.
        /// </param>
        public Terrain(CameraPerspective camera, Texture2D texture)
        {
            this.device = DisplayManager.Instance.GraphicsDevice;
            this.camera = camera;
            this.texture = texture;

            this.Init();
        }

        /// <summary>
        /// The draw terrain.
        /// </summary>
        public void DrawTerrain()
        {
            this.effect.View = this.camera.ViewMatrix;
            this.effect.Projection = this.camera.ProjectionMatrix;
            this.effect.World = Matrix.Identity;
            this.effect.TextureEnabled = true;
            this.effect.Texture = this.texture;
            foreach (var pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                this.device.DrawUserPrimitives(PrimitiveType.TriangleList, this.terrainVertices, 0, 2);
            }
        }

        /// <summary>
        /// The init.
        /// </summary>
        private void Init()
        {
            var minus = -80;
            var plus = 80;
            this.terrainVertices = new VertexPositionNormalTexture[6];
            this.terrainVertices[0].Position = new Vector3(minus, minus, 0);
            this.terrainVertices[1].Position = new Vector3(minus, plus, 0);
            this.terrainVertices[2].Position = new Vector3(plus, minus, 0);
            this.terrainVertices[3].Position = this.terrainVertices[1].Position;
            this.terrainVertices[4].Position = new Vector3(plus, plus, 0);
            this.terrainVertices[5].Position = this.terrainVertices[2].Position;

            this.terrainVertices[0].TextureCoordinate = new Vector2(0, 0);
            this.terrainVertices[1].TextureCoordinate = new Vector2(0, 1);
            this.terrainVertices[2].TextureCoordinate = new Vector2(1, 0);

            this.terrainVertices[3].TextureCoordinate = terrainVertices[1].TextureCoordinate;
            this.terrainVertices[4].TextureCoordinate = new Vector2(1, 1);
            this.terrainVertices[5].TextureCoordinate = terrainVertices[2].TextureCoordinate;


            this.effect = new BasicEffect(this.device);
        }
    }
}