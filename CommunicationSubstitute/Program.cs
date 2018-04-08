using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameMasterCore;
using Shared.Components.Players;
using Shared.Messages.Configuration;
using PlayerCore;
using Shared.Enums;
using Shared.Messages.Communication;
using Shared.Components.Events;

namespace CommunicationSubstitute
{
    class Program
    {

        static void Main(string[] args)
        {
            Game g = new Game();
            g.Initialize();
            g.RegisterPlayers();
            g.CreatePlayers();
            // all playeyers have bords
            // g.BluePlayers[0].State.Board
            // g.GameInfo.blueTeamPlayers
            // g.BluePlayers[0].State.Id
            // g.GameMaster.Board
            // g.GameMaster.Log
            g.StartPlayers();
            g.JoinPlayers();
        }
    }
}
