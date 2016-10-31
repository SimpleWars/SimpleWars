namespace SimpleWars.Entities.Environment
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Tree : Entity
    {
        public Tree(Model model, Vector3 position, float scale)
            : base(model, position, scale)
        {
        }

        public Tree(Model model, Vector3 position, Vector3 rotation, float scale)
            : base(model, position, rotation, scale)
        {
        }
    }
}