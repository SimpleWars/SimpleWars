namespace SimpleWars.GUI.Interfaces
{
    public interface IPartialTextNode : ITextNode
    {
        int CharsDisplayed{ get; set; }

        int CursorIndex { get; }

        int Limit { get; }

        string DisplayText { get; }

        void MoveCursor(int amount);
    }
}