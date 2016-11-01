namespace SimpleWars.Displays.DisplaysHolder
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Camera;
    using SimpleWars.Entities;
    using SimpleWars.Entities.Environment;
    using SimpleWars.Res.DisplayAssets;
    using SimpleWars.Terrain;

    public class Test3Display : Display
    {
        private Test3Assets assets;

        private IList<Entity> entities;

        private CameraPerspective camera;

        private Terrain terrain;

        public override void LoadContent()
        {
            var aspectRatio = DisplayManager.Instance.Dimensions.X / DisplayManager.Instance.Dimensions.Y;
            this.camera = new CameraPerspective(aspectRatio, new Vector3(-20, -20, 50));
            this.camera.Pitch = 1f;
            this.assets = new Test3Assets();
            this.entities = new List<Entity>();
            this.terrain = new Terrain(this.camera, this.assets.TerrainTexture);
            var random = new Random();
            var numberOfTrees = random.Next(30, 100);

            for (int i = 0; i < numberOfTrees; i++)
            {
                var x = random.Next(1, 50);
                var y = random.Next(1, 50);

                this.entities.Add(new Tree(this.assets.Model, new Vector3(x, y, 2), 1));
            }
        }

        public override void UnloadContent()
        {
            this.assets.UnloadAssets();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in this.entities)
            {
                entity.Rotate(new Vector3(0, 0, 2));
            }
            
            this.camera.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.terrain.DrawTerrain();

            foreach (var entity in this.entities)
            {
                this.DrawModel(entity);
            }
        }

        private void DrawModel(Entity entity)
        {
            foreach (ModelMesh mesh in entity.Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    
                    effect.World = entity.GetTransformationMatrix();
                    effect.View = this.camera.ViewMatrix;
                    effect.Projection = this.camera.ProjectionMatrix;
                }

                mesh.Draw();
            }
        }
    }
}