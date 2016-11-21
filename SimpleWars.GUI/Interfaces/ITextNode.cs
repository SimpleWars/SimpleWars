namespace SimpleWars.GUI.Interfaces
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public interface ITextNode : IGui
    {
        string TextContent { get; set; }

        string DisplayText { get; set; }

        SpriteFont SpriteFont { get; set; }

        Color TextColor { get; set; }
    }
}