using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class PlayerDefinition
    {
        public int Id { get; set; }

        public TeamColour Team { get; set; }

        public PlayerType Type { get; set; }
    }
}
