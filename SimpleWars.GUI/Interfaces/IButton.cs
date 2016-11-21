namespace SimpleWars.GUI.Interfaces
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.GUI.PrimitiveComponents;

    public interface IButton : IGui, IClickable
    {
        ITextBox AttachedTextBox { get; set; }

        Texture2D Background { get; set; }

        TextNode TextNode { get; set; }

        Vector2 TextOffset { get; set; }
    }
}