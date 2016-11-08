﻿namespace SimpleWars.Displays.DisplaysHolder
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

        private Skybox skybox;

        public override void LoadContent()
        {
            var aspectRatio = DisplayManager.Instance.Dimensions.X / DisplayManager.Instance.Dimensions.Y;
            this.camera = new CameraPerspective(
                aspectRatio,
                new Vector3(-50, -30, 0));
            //this.camera = new CameraOrthographic();
            this.assets = new Test3Assets();
            this.entities = new List<Entity>();

            this.terrain = new HomeTerrain(
                                this.assets.Terra, 
                                this.assets.TerrainTexture, 
                                new Vector3(400, 0, 400));

            this.skybox = new Skybox(DisplayManager.Instance.GraphicsDevice, this.assets.SkyboxTexture);
            
            var random = new Random();
            var numberOfTrees = random.Next(30, 100);

            for (int i = 0; i < numberOfTrees; i++)
            {
                var x = random.Next(-20, 20);
                var y = random.Next(-20, 20);

                this.entities.Add(new Tree(this.assets.Model, new Vector3(x, 0, y), new Vector3(-90, 0, 0), 1));
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

            this.skybox.Update(gameTime);
            this.camera.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //this.terrain.Draw(this.camera.ViewMatrix, this.camera.ProjectionMatrix);
            this.terrain.DrawProceduralTerrain(this.camera.ViewMatrix, this.camera.ProjectionMatrix);
            this.skybox.Draw(this.camera.ProjectionMatrix, this.camera.ViewMatrix);

            foreach (var entity in this.entities)
            {
                entity.Draw(this.camera.ViewMatrix, this.camera.ProjectionMatrix);
            }
        }
    }
}