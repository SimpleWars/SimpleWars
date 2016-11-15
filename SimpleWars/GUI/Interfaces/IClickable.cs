namespace SimpleWars.GUI.Interfaces
{
    using System;

    public interface IClickable
    {
        Action ClickLogic { get; set; }

        void DetectClick(float mouseX, float mouseY);
    }
}