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


        private Terrain terrain;

        public override void LoadContent()
        {
            var aspectRatio = DisplayManager.Instance.Dimensions.X / DisplayManager.Instance.Dimensions.Y;
            this.camera = new CameraPerspective(aspectRatio, new Vector3(0, 35, 40));
            //this.camera = new CameraOrthographic();
            //this.camera.Pitch = 0.5f;
            //this.camera.Yaw = 0.7f;
            this.assets = new Test3Assets();
            this.entities = new List<Entity>();
            this.terrain = new Terrain(this.camera, this.assets.TerrainTexture);
            var random = new Random();
            var numberOfTrees = random.Next(30, 100);

            for (int i = 0; i < numberOfTrees; i++)
            {
                var x = random.Next(-20, 20);
                var y = random.Next(-20, 20);

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
                //entity.Move(new Vector3(0.02f, 0, 0));
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
            var world = entity.GetTransformationMatrix();
            var view = this.camera.ViewMatrix;
            var projection = this.camera.ProjectionMatrix;

            foreach (ModelMesh mesh in entity.Model.Meshes)
            {              
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }
    }
}