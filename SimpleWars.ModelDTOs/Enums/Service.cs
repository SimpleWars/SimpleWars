namespace SimpleWars.ModelDTOs.Enums
{
    public enum Service
    {
        None = 0,
        Info = 1,
        Login = 2,
        Logout = 3,
        Registration = 4,
        OwnPlayerData = 5,
        OtherPlayerData = 6,
        UpdateEntities = 7,
        UpdateResourceSet = 8,
        AddResProv = 9,
        AddUnit = 10,
        FetchOtherPlayers = 11,
        OtherPlayers = 12,
        StartBattle = 13,
        BattleEnd = 14,
        BattleStarted = 15,
        Ping = 16,
    }
}