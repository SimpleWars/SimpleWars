namespace SimpleWars.Displays.DisplaysHolder
{
    using System.Diagnostics;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Camera;
    using SimpleWars.Entities;
    using SimpleWars.Entities.Environment;
    using SimpleWars.Res.DisplayAssets;

    public class Test3Display : Display
    {
        private Test3Assets assets;

        private Tree testEntity;

        private Camera camera;

        public override void LoadContent()
        {
            this.assets = new Test3Assets();
            this.testEntity = new Tree(this.assets.Model, new Vector3(5, 5, 2), 1);
            this.camera = new Camera();
        }

        public override void UnloadContent()
        {
            this.assets.UnloadAssets();
        }

        public override void Update(GameTime gameTime)
        {
            this.testEntity.Move(new Vector3(-0.002f, 0, 0));
            this.testEntity.Rotate(new Vector3(0, 0, 2));
            this.camera.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.DrawModel(this.testEntity);
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