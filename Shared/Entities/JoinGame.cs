using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class JoinGame
    {
        public string GameName { get; set; }

        public PlayerType PreferedRole { get; set; }

        public TeamColour PreferedTeam { get; set; }
    }
}
