namespace SimpleWars.GUI.Interfaces
{
    using System;

    public interface IClickable
    {
        bool IsClicked { get; set; }

        Action ClickLogic { get; set; }

        void DetectClick(float mouseX, float mouseY);
    }
}