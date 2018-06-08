using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMasterCore.Components.PlayerStatistics
{
    public class PlayerStats
    {
        public string playerFriendlyName;
        public bool hasWon;
        public List<TimeSpan> responseTimes;
    }
}
