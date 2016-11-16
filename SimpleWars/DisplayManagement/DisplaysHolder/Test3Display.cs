namespace SimpleWars.DisplayManagement.DisplaysHolder
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using SimpleWars.AssetsManagement;
    using SimpleWars.Camera;
    using SimpleWars.DBContexts;
    using SimpleWars.GameData.Entities;
    using SimpleWars.GameData.Entities.DynamicEntities;
    using SimpleWars.GameData.Entities.DynamicEntities.BasicUnits;
    using SimpleWars.GameData.Entities.Interfaces;
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

            foreach (var entity 
                    in PlayerManager.CurrentPlayer.ResourceProviders
                    .Concat<IEntity>(PlayerManager.CurrentPlayer.Units))
            {
                entity.LoadModel();
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

            if (Input.KeyPressed(Keys.D1))
            {
                if (EntityPicker.HasPicked())
                {
                    EntityPicker.PlaceEntity();
                }
                else
                {
                    var unit = new Swordsman(Vector3.Zero, 1);
                    PlayerManager.CurrentPlayer.Units.Add(unit);
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
                        this.camera.ProjectionMatrix,
                        this.camera.ViewMatrix,
                        PlayerManager.CurrentPlayer.ResourceProviders);
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
            }

            foreach (var unit in PlayerManager.CurrentPlayer.Units)
            {
                unit.Draw(this.camera.ViewMatrix, this.camera.ProjectionMatrix);
            }
        }
    }
}