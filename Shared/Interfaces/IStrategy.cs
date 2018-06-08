using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTO.Communication;

namespace Shared.Interfaces
{
    public interface IStrategy
    {
        Data PerformAction();
        IGameMaster GameMaster { get; set; } 
        
    }
}
