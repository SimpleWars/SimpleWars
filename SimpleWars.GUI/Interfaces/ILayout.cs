namespace SimpleWars.GUI.Interfaces
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SimpleWars.Data.Contexts;

    public interface ILayout : IGui
    {
        ICollection<IButton> Buttons { get; }

        ICollection<ITextBox> TextBoxes { get; }

        ICollection<ITextNode> TextNodes { get; }

        Texture2D Background { get; set; }

        void Update(GameTime gameTime);
    }
}