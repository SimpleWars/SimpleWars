namespace SimpleWars.GUI.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface ITextBox : IGui, IClickable, IInputReader
    {
        Color BorderColor { get; set; }

        Color InnerColor { get; set; }

        int BorderWidth { get; set; }

        string TextContent { get; set; }
    }
}