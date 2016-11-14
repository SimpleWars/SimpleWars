namespace SimpleWars.GameData.Resources.ConcreteResources
{
    using Microsoft.Xna.Framework.Graphics;

    public class Gold : Resource
    {
        public Gold(int quantity)
            : base(ResourceType.Gold, quantity)
        {
        }
    }
}