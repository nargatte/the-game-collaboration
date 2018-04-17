using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Messages.Communication;

namespace Shared.Interfaces
{
    public interface IStrategy
    {
        Data PerformAction();
        IGameMaster GameMaster { get; set; } 
        
    }
}
