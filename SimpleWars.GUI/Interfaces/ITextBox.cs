namespace SimpleWars.GUI.Interfaces
{
    using Microsoft.Xna.Framework;

    using SimpleWars.GUI.PrimitiveComponents;

    public interface ITextBox : IGui, IClickable, IInputReader
    {
        Color BorderColor { get; set; }

        Color InnerColor { get; set; }

        int BorderWidth { get; set; }

        PartialTextNode TextNode { get; set; }
    }
}