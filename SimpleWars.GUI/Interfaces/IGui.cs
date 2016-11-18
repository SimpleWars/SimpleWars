namespace SimpleWars.GUI.Interfaces
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public interface IGui
    {
        Vector2 Position { get; set; }

        Vector2 Dimensions { get; set; }

        void Draw(SpriteBatch spriteBatch);
    }
}