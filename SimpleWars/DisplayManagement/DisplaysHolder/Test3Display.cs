namespace SimpleWars.DisplayManagement.DisplaysHolder
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.AssetsManagement;
    using SimpleWars.Camera;
    using SimpleWars.DBContexts;
    using SimpleWars.GameData.Entities;
    using SimpleWars.GameData.Entities.DynamicEntities;
    using SimpleWars.GameData.Entities.StaticEntities.Environment;
    using SimpleWars.GameData.Environment;
    using SimpleWars.GameData.Terrain;
    using SimpleWars.GameData.Terrain.Terrains;
    using SimpleWars.InputManagement;
    using SimpleWars.User;

    public class Test3Display : Display
    {
        private CameraPerspective camera;

        private Terrain terrain;

        private Skybox skybox;

        public override void LoadContent(GameContext context)
        {
            var aspectRatio = DisplayManager.Instance.Dimensions.X / DisplayManager.Instance.Dimensions.Y;
            this.camera = new CameraPerspective(
                aspectRatio,
                new Vector3(50, 30, 0));

            this.terrain = new HomeTerrain(new Vector3(-400, 0, -400));

            this.skybox = new Skybox();

            if (PlayerManager.CurrentPlayer.ResourceProviders.Count == 0)
            {
                PlayerManager.CurrentPlayer.ResourceSet.Gold.Quantity = 5000;
                PlayerManager.CurrentPlayer.ResourceSet.Wood.Quantity = 2000;
                var random = new Random();
                var numberOfTrees = random.Next(300, 400);

                for (int i = 0; i < numberOfTrees; i++)
                {
                    var x = random.Next(-200, 200);
                    var z = random.Next(-200, 200);
                    //var y = this.terrain.GetWorldHeight(x, z);
                    var weight = random.Next(5, 10);
                    var y = 100;

                    var tree = new Tree(new Vector3(x, y, z), Vector3.Zero, weight, 1);
                    PlayerManager.CurrentPlayer.ResourceProviders.Add(tree);
                }

                context.SaveChanges();
            }
            else
            {
                foreach (var entity in PlayerManager.CurrentPlayer.ResourceProviders)
                {
                    entity.LoadModel();
                }
            }
        }

        public override void UnloadContent()
        {
            ModelsManager.Instance.DisposeAll();
        }

        public override void Update(GameTime gameTime, GameContext context)
        {
            foreach (var entity in PlayerManager.CurrentPlayer.ResourceProviders)
            {
                entity.GravityAffect(gameTime, this.terrain);
            }

            if (Input.LeftMouseClick())
            {
                if (EntityPicker.HasPicked())
                {
                    EntityPicker.PlaceEntity();
                }
                else
                {
                    EntityPicker.PickEntity(this.camera.ProjectionMatrix, this.camera.ViewMatrix, PlayerManager.CurrentPlayer.ResourceProviders);
                }              
            }

            EntityPicker.DragEntity(this.camera.ProjectionMatrix, this.camera.ViewMatrix, this.terrain);

            this.skybox.Update(gameTime);
            this.camera.Update(gameTime, this.terrain);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.skybox.Draw(this.camera.ProjectionMatrix, this.camera.ViewMatrix);
            this.terrain.Draw(this.camera.ViewMatrix, this.camera.ProjectionMatrix);

            foreach (var entity in PlayerManager.CurrentPlayer.ResourceProviders)
            {
                entity.Draw(this.camera.ViewMatrix, this.camera.ProjectionMatrix);

                //if (!(entity is AnimatedEntity))
                //{
                //}
                //else
                //{
                //    (entity as AnimatedEntity).Draw(this.camera.ViewMatrix, this.camera.ProjectionMatrix);
                //}                
            }
        }
    }
}