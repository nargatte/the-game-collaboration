using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Interfaces;
using Shared.Messages.Communication;
using Shared.Components.Boards;
using Shared.Components.Pieces;
using Shared.Components.Players;

namespace PlayerCore
{
    public class State
    {
        public Game Game { get; }

        public event EventHandler EndGame; 

        public IBoard Board { get; }

        public State(Game game, IBoard board, int id)
        {
            Game = game;
            Board = board;

        }

        public void ReceiveData(Data data)
        {

        }
    }
}
