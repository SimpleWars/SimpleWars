namespace SimpleWars.GUI.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface ITextBox : IGui, IClickable, IInputReader
    {
        Vector2 TextOffset { get; set; }

        Color BorderColor { get; set; }

        Color InnerColor { get; set; }

        int BorderWidth { get; set; }

        string TextContent { get; set; }

        int CharsDisplayed { get; set; }

        int CurrentIndexToDisplay { get; set; }
    }
}