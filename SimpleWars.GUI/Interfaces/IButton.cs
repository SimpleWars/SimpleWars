namespace SimpleWars.GUI.Interfaces
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public interface IButton : IGui, IClickable
    {
        ITextBox AttachedTextBox { get; set; }

        Texture2D Background { get; set; }

        string TextContent { get; set; }

        Vector2 TextOffset { get; set; }
    }
}