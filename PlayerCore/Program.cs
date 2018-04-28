using Shared.Components.Communication;
using Shared.Enums;
using Shared.Messages.Communication;

namespace PlayerCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var networkClient = new NetworkClient())
            {
                networkClient.Connect("localhost", 21);
                Game g = new Game()
                {
                    Board = new GameBoard()
                    {
                        goalsHeight = 10,
                        tasksHeight = 20,
                        width = 30
                    },
                    PlayerLocation = new Location()
                    {
                        x = 12,
                        y = 13
                    },
                    Players = new Player[]
                    {
                        new Player() {id = 1, team = TeamColour.Blue, type = PlayerType.Leader},
                        new Player() {id = 2, team = TeamColour.Blue, type = PlayerType.Member},
                        new Player() {id = 3, team = TeamColour.Red, type = PlayerType.Leader},
                        new Player() {id = 4, team = TeamColour.Red, type = PlayerType.Member}
                    },
                    playerId = 2
                };
                networkClient.Send(g);

                if (networkClient.TryReceive(out Game g2))
                {
                    var q = g2.ToString();
                }
            }

        }
    }
}
