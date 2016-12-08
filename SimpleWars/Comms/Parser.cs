namespace SimpleWars.Comms
{
    using Microsoft.Xna.Framework;

    using SimpleWars.Assets;
    using SimpleWars.DisplayManagement;
    using SimpleWars.Factories;
    using SimpleWars.GUI.PrimitiveComponents;
    using SimpleWars.ModelDTOs;
    using SimpleWars.ModelDTOs.Enums;
    using SimpleWars.Models.Users;
    using SimpleWars.Users;

    public class Parser
    {
        private readonly AsynchronousSocketClient client;

        public Parser(AsynchronousSocketClient client)
        {
            this.client = client;
        }

        public void Parse()
        {
            Message message = this.client.MsgQueue.TryDequeue();

            switch (message.Service)
            {
                case Service.Info:
                    this.HandleInfo(((Message<string>)message).Data);
                break;
                case Service.OwnPlayerData:
                    this.HandleLogin(((Message<PlayerDTO>)message).Data);
                    break;

            }
        }

        private void HandleInfo(string content)
        {
            DisplayManager.Instance.CurrentDisplay.Guis.Add(new TextNode(null, new Vector2(20, 20), new Vector2(100, 1000), content, SpriteFontManager.Instance.GetFont("Ariel_18"), Color.Red));
        }

        private void HandleLogin(PlayerDTO playerDto)
        {
            
            UsersManager.CurrentPlayer = this.MapPlayerDto(playerDto);
        }

        private Player MapPlayerDto(PlayerDTO playerDto)
        {
            var player = new Player(playerDto.Username, playerDto.WorldSeed);
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

            return player;
        }
    }
}