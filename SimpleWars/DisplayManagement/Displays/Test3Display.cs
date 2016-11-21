namespace SimpleWars.DisplayManagement.Displays
{
    using System;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using SimpleWars.Assets;
    using SimpleWars.Camera;
    using SimpleWars.Data.Contexts;
    using SimpleWars.Environment.Skybox;
    using SimpleWars.Environment.Terrain;
    using SimpleWars.Environment.Terrain.Terrains;
    using SimpleWars.Input;
    using SimpleWars.Models.Entities.DynamicEntities.BattleUnits;
    using SimpleWars.Models.Entities.Interfaces;
    using SimpleWars.Models.Entities.StaticEntities.ResourceProviders;
    using SimpleWars.Users;

    public class Test3Display : Display
    {
        private CameraPerspective camera;

        private Terrain terrain;

        private Skybox skybox;

        public override void LoadContent()
        {
            var aspectRatio = DisplayManager.Instance.Dimensions.X / DisplayManager.Instance.Dimensions.Y;
            this.camera = new CameraPerspective(
                aspectRatio,
                new Vector3(50, 30, 0));

            this.terrain = new HomeTerrain(DisplayManager.Instance.GraphicsDevice, UsersManager.CurrentPlayer.HomeSeed, new Vector3(-400, 0, -400));

            this.skybox = new Skybox(DisplayManager.Instance.GraphicsDevice);

            if (!UsersManager.CurrentPlayer.ResourceProviders.Concat<IEntity>(UsersManager.CurrentPlayer.Units).Any())
            {
                var random = new Random();
                var numberOfTrees = random.Next(300, 400);

                for (int i = 0; i < numberOfTrees; i++)
                {
                    var x = random.Next(-200, 200);
                    var z = random.Next(-200, 200);
                    var weight = random.Next(5, 10);
                    var y = 100;

                    var tree = new Tree(new Vector3(x, y, z), Vector3.Zero, weight, 1);
                    UsersManager.CurrentPlayer.ResourceProviders.Add(tree);
                }
            }
            else
            {
                foreach (var entity in
                    UsersManager.CurrentPlayer.ResourceProviders
                    .Concat<IEntity>(UsersManager.CurrentPlayer.Units))
                {
                    entity.LoadModel();
                }
            }            
        }

        public override void UnloadContent()
        {
            ModelsManager.Instance.DisposeAll();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in UsersManager.CurrentPlayer.ResourceProviders)
            {
                entity.GravityAffect(gameTime, this.terrain);
            }

            if (Input.KeyPressed(Keys.D1))
            {
                if (EntityPicker.HasPicked())
                {
                    EntityPicker.PlaceEntity();
                }
                else
                {
                    var unit = new Swordsman(Vector3.Zero, 1);
                    UsersManager.CurrentPlayer.Units.Add(unit);
                    EntityPicker.EntityPicked = unit;
                }
            }

            if (Input.LeftMouseClick())
            {
                if (EntityPicker.HasPicked())
                {
                    EntityPicker.PlaceEntity();
                }
                else
                {
                    EntityPicker.PickEntity(
                        DisplayManager.Instance.GraphicsDevice,
                        this.camera.ProjectionMatrix,
                        this.camera.ViewMatrix,
                        UsersManager.CurrentPlayer.ResourceProviders);
                }
            }

            EntityPicker.DragEntity(
                DisplayManager.Instance.GraphicsDevice,
                this.camera.ProjectionMatrix,
                this.camera.ViewMatrix,
                this.terrain);

            this.skybox.Update(gameTime);
            this.camera.Update(gameTime, this.terrain);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.skybox.Draw(this.camera.ProjectionMatrix, this.camera.ViewMatrix);
            this.terrain.Draw(this.camera.ViewMatrix, this.camera.ProjectionMatrix);

            foreach (var entity in UsersManager.CurrentPlayer.ResourceProviders)
            {
                entity.Draw(this.camera.ViewMatrix, this.camera.ProjectionMatrix);          
            }

            foreach (var unit in UsersManager.CurrentPlayer.Units)
            {
                unit.Draw(this.camera.ViewMatrix, this.camera.ProjectionMatrix);
            }
        }
    }
}