using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ConfirmJoiningGame
    {
        public int GameId { get; set; }

        public int PlayerId { get; set; }

        public string PrivateGuid { get; set; }

        public PlayerDefinition PlayerDefinition { get; set; }
    }
}
