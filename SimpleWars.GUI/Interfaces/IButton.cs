namespace SimpleWars.GUI.Interfaces
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.GUI.PrimitiveComponents;

    public interface IButton : IGui, IClickable
    {
        Texture2D Background { get; set; }

        TextNode TextNode { get; set; }

        Color BorderColor { get; set; }

        int BorderWidth { get; set; }
    }
}