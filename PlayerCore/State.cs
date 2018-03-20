using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Interfaces;
using Shared.Messages.Communication;

namespace PlayerCore
{
    public class State
    {
        public Game Game { get; set; } = new Game();

        public event EventHandler EndGame; 

        public State()
        {
            Game.Board = new GameBoard();
            Game.PlayerLocation = new Location();
            Game.Players = new Player[0];
        }

        public void ReceiveData(Data data)
        {

        } 
    }
}
