namespace SimpleWars.Terrain
{
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics;

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
            this.Model = model;
            this.Position = position;

            this.InitFlatTerrain();
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

            this.InitFlatTerrain();
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
        public HomeTerrain(Model model, Texture2D texture, Vector3 position, Vector3 rotation, float scale = 1)
            : base(model, position, rotation, scale)
        {
            this.device = DisplayManager.Instance.GraphicsDevice;
            this.texture = texture;

            this.InitFlatTerrain();
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
        public void DrawTerrainTexture(Matrix viewMatrix, Matrix projectionMatrix)
        {
            this.effect.View = viewMatrix;
            this.effect.Projection = projectionMatrix;
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
        /// The custom terrain draw.
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
        /// </summary>
        private void InitFlatTerrain()
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

            this.terrainVertices[3].TextureCoordinate = this.terrainVertices[1].TextureCoordinate;
            this.terrainVertices[4].TextureCoordinate = new Vector2(1, 1);
            this.terrainVertices[5].TextureCoordinate = this.terrainVertices[2].TextureCoordinate;


            this.effect = new BasicEffect(this.device);
        }
    }
}