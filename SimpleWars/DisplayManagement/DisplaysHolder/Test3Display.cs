namespace SimpleWars.DisplayManagement.DisplaysHolder
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.AssetsManagement;
    using SimpleWars.Camera;
    using SimpleWars.Entities;
    using SimpleWars.Entities.StaticEntities;
    using SimpleWars.Environment;
    using SimpleWars.InputManagement;
    using SimpleWars.Terrain;
    using SimpleWars.Terrain.Terrains;

    public class Test3Display : Display
    {
        private IList<Entity> entities;

        private CameraPerspective camera;

        private Terrain terrain;

        private Skybox skybox;

        private Tree testtree;

        public override void LoadContent()
        {
            var aspectRatio = DisplayManager.Instance.Dimensions.X / DisplayManager.Instance.Dimensions.Y;
            this.camera = new CameraPerspective(
                aspectRatio,
                new Vector3(50, 30, 0));

            this.entities = new List<Entity>();

            this.terrain = new HomeTerrain(new Vector3(-400, 0, -400));

            this.skybox = new Skybox();
           
            var random = new Random();
            var numberOfTrees = random.Next(300, 400);

            for (int i = 0; i < numberOfTrees; i++)
            {
                var x = random.Next(-200, 200);
                var z = random.Next(-200, 200);
                //var y = this.terrain.GetWorldHeight(x, z);
                var weight = random.Next(5, 10);
                var y = 100;

                this.entities.Add(new Tree(new Vector3(x, y, z), new Vector3(-90, 0, 0), weight, 1));
            }

            this.testtree = new Tree(new Vector3(0, 50, 0), new Vector3(-90, 0, 0), 0, 1);
        }

        public override void UnloadContent()
        {
            Assets3Manager.Instance.DisposeAll();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in this.entities)
            {
                entity.Rotate(new Vector3(0, 2, 0));
                entity.Move(gameTime, Vector3.Zero, this.terrain);
            }

            this.skybox.Update(gameTime);
            this.camera.Update(gameTime, this.terrain);

            this.testtree.Position = RayCaster.GetTerrainPoint(
                    this.camera.ProjectionMatrix,
                    this.camera.ViewMatrix,
                    this.terrain);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.terrain.Draw(this.camera.ViewMatrix, this.camera.ProjectionMatrix);
            this.skybox.Draw(this.camera.ProjectionMatrix, this.camera.ViewMatrix);

            foreach (var entity in this.entities)
            {
                entity.Draw(this.camera.ViewMatrix, this.camera.ProjectionMatrix);
            }

            this.testtree.Draw(this.camera.ViewMatrix, this.camera.ProjectionMatrix);
        }
    }
}