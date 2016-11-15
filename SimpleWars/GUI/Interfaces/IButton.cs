namespace SimpleWars.GUI.Interfaces
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public interface IButton : IGui, IClickable
    {
        Vector2 Position { get; set; }

        Vector2 Scale { get; set; }

        Vector2 TextOffset { get; set; }

        Texture2D Background { get; set; }

        string TextContent { get; set; }
    }
}