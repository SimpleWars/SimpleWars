namespace SimpleWars.Displays.DisplaysHolder
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Res.DisplayAssets;

    public class Test3Display : Display
    {
        private Test3Assets assets;

        private Matrix world = Matrix.Identity;
        private Matrix view = Matrix.CreateLookAt(new Vector3(5, 8, 5), new Vector3(0, 0, 0), Vector3.UnitZ);
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 1280f / 720f, 1, 200);

        public override void LoadContent()
        {
            this.assets = new Test3Assets();
        }

        public override void UnloadContent()
        {
            this.assets.UnloadAssets();
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.DrawModel(this.assets.Model, this.world, this.view, this.projection);
        }

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
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