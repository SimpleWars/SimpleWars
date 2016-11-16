namespace SimpleWars.GUI.Interfaces
{
    using System.Dynamic;

    using Microsoft.Xna.Framework;

    public interface ITextBox : IGui, IClickable, IInputReader
    {
        Vector2 Position { get; set; }

        Vector2 Dimensions { get; set; }

        string TextContent { get; set; }

        Color BorderColor { get; set; }

        Color InnerColor { get; set; }

        int BorderWidth { get; set; }
    }
}