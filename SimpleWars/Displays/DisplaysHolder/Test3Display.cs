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
    using SimpleWars.InputManager;
    using SimpleWars.Res.DisplayAssets;
    using SimpleWars.Terrain;

    public class Test3Display : Display
    {
        private Test3Assets assets;

        private IList<Entity> entities;

        //private CameraOrthographic camera;

        private CameraPerspective camera;


        private HomeTerrain terrain;

        public override void LoadContent()
        {
            var aspectRatio = DisplayManager.Instance.Dimensions.X / DisplayManager.Instance.Dimensions.Y;
            this.camera = new CameraPerspective(aspectRatio, new Vector3(0, 30, 0));
            this.assets = new Test3Assets();
            this.entities = new List<Entity>();

            this.terrain = new HomeTerrain(
                                this.assets.Terra, 
                                this.assets.TerrainTexture, 
                                new Vector3(-100, 0, 0), 
                                new Vector3(1, 0, 0),
                                150);
            
            var random = new Random();
            var numberOfTrees = random.Next(30, 100);

            for (int i = 0; i < numberOfTrees; i++)
            {
                var x = random.Next(-20, 20);
                var z = random.Next(-20, 20);

                this.entities.Add(new Tree(this.assets.Model, new Vector3(x, 0, z), 1));
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
                entity.Rotate(new Vector3(0, 2, 0));
                //entity.Move(new Vector3(0, 0, 0));
            }

            this.camera.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.terrain.Draw(this.camera.ViewMatrix, this.camera.ProjectionMatrix);

            foreach (var entity in this.entities)
            {
                entity.Draw(this.camera.ViewMatrix, this.camera.ProjectionMatrix);
            }
        }
    }
}