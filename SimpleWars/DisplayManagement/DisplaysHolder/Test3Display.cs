namespace SimpleWars.DisplayManagement.DisplaysHolder
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.AssetsManagement;
    using SimpleWars.Camera;
    using SimpleWars.Entities;
    using SimpleWars.Entities.DynamicEntities;
    using SimpleWars.Entities.DynamicEntities.AnimatedEnvironment;
    using SimpleWars.Entities.StaticEntities;
    using SimpleWars.Entities.StaticEntities.Environment;
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
                var treeType = random.Next(0, 2);
                Entity tree;
                if (treeType == 1)
                {
                    tree = new AnimatedTree(new Vector3(x, y, z), new Vector3(90, 0, 0), weight, 0.0001f);
                    (tree as AnimatedEntity).ChangeClip("Armature|ArmatureAction");
                }
                else
                {
                    tree = new Tree(new Vector3(x, y, z), new Vector3(-90, 0, 0), weight, 1);
                }

                this.entities.Add(tree);
            }
        }

        public override void UnloadContent()
        {
            ModelsManager.Instance.DisposeAll();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in this.entities)
            {
                entity.GravityAffect(gameTime, this.terrain);

                (entity as AnimatedEntity)?.UpdateAnimation(gameTime);
            }

            if (Input.LeftMouseClick())
            {
                if (EntityPicker.HasPicked())
                {
                    EntityPicker.PlaceEntity();
                }
                else
                {
                    EntityPicker.PickEntity(this.camera.ProjectionMatrix, this.camera.ViewMatrix, this.entities);
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

            foreach (var entity in this.entities)
            {
                if ((entity as AnimatedEntity) == null)
                {
                    entity.Draw(this.camera.ViewMatrix, this.camera.ProjectionMatrix);
                }
                else
                {
                    (entity as AnimatedEntity).Draw(this.camera.ViewMatrix, this.camera.ProjectionMatrix);
                }                
            }
        }
    }
}