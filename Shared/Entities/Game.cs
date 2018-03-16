using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class Game
    {
        public int PlayerId { get; set; }

        public List<Player> Players { get; set; }

        public Board Board { get; set; }

        public Location PlayerLocation { get; set; }
    }
}
