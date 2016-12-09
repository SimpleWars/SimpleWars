namespace SimpleWars.Comms
{
    using Microsoft.Xna.Framework;

    using SimpleWars.Assets;
    using SimpleWars.DisplayManagement;
    using SimpleWars.DisplayManagement.Displays;
    using SimpleWars.Factories;
    using SimpleWars.GUI.PrimitiveComponents;
    using SimpleWars.ModelDTOs;
    using SimpleWars.ModelDTOs.Enums;
    using SimpleWars.Models.Economy;
    using SimpleWars.Models.Users;
    using SimpleWars.Users;

    public class Parser
    {
        private readonly AsynchronousSocketClient client;

        public Parser(AsynchronousSocketClient client)
        {
            this.client = client;
        }

        public void StartParsing()
        {
            while (true)
            {
                Message message = this.client.MsgQueue.TryDequeue();
                if (message == null)
                    continue;

                switch (message.Service)
                {
                    case Service.Ping:
                        break;
                    case Service.Info:
                        this.HandleInfo(((Message<string>)message).Data);
                        break;
                    case Service.OwnPlayerData:
                        this.HandleLogin(((Message<PlayerDTO>)message).Data);
                        DisplayManager.Instance.ChangeDisplay(new HomeWorldDisplay());
                        break;
                }
            }
        }

        private void HandleInfo(string content)
        {
            DisplayManager.Instance.ResponseText = new TextNode(null, new Vector2(0, 0), new Vector2(1, 1), content, SpriteFontManager.Instance.GetFont("Arial_18"), Color.Red);
        }

        private void HandleLogin(PlayerDTO playerDto)
        {   
            UsersManager.CurrentPlayer = this.MapPlayerDto(playerDto);
        }

        private Player MapPlayerDto(PlayerDTO playerDto)
        {
            ResourceSet resSet = ResourceSetFactory.FromDto(playerDto.ResourceSet);

            var player = new Player(playerDto.Id, playerDto.Username, playerDto.WorldSeed, resSet);
            foreach (var unitDto in playerDto.Units)
            {
                var unit = UnitFactory.FromDto(unitDto);
                if (unit == null) continue;

                player.Units.Add(unit);
            }

            foreach (var resProvDto in playerDto.ResourceProviders)
            {
                var resProv = ResProvFactory.FromDto(resProvDto);
                if (resProv == null) continue;

                player.ResourceProviders.Add(resProv);
            }

            player.MapEntities();
            return player;
        }
    }
}