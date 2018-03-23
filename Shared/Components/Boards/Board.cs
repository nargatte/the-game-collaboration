using System.Collections.Generic;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.Enums;

namespace Shared.Components.Boards
{
	public class Board : IBoard
	{
		#region IBoard
		public virtual uint Width { get; }
		public virtual uint TasksHeight { get; }
		public virtual uint GoalsHeight { get; }
		public virtual uint Height { get; }
		public virtual IEnumerable<IField> Fields
		{
			get
			{
				foreach( var field in fields )
					yield return field;
			}
		}
		public virtual IEnumerable<IPiece> Pieces
		{
			get
			{
				foreach( var piece in pieces )
					yield return piece.Value;
			}
		}
		public virtual IEnumerable<IPlayer> Players
		{
			get
			{
				foreach( var player in players )
					yield return player.Value;
			}
		}
		public virtual IField GetField( uint x, uint y ) => x >= 0 && x < Width && y >= 0 && y < Height ? fields[ x, y ] : null;
		public virtual IPiece GetPiece( ulong id ) => pieces.TryGetValue( id, out var piece ) ? piece : null;
		public virtual IPlayer GetPlayer( ulong id ) => players.TryGetValue( id, out var player ) ? player : null;
		public virtual void SetField( IField value ) => throw new System.NotImplementedException();
		public virtual void SetPiece( IPiece value ) => throw new System.NotImplementedException();
		public virtual void SetPlayer( IPlayer value ) => throw new System.NotImplementedException();
		#endregion
		#region Board
		private readonly IField[,] fields;
		private readonly IDictionary<ulong, IPlayer> players;
		private readonly IDictionary<ulong, IPiece> pieces;
		public Board( uint width, uint tasksHeight, uint goalsHeight )
		{
			Width = width;
			TasksHeight = tasksHeight;
			GoalsHeight = goalsHeight;
			Height = TasksHeight + 2 * GoalsHeight;
			fields = new IField[ Width, Height ];
			players = new Dictionary<ulong, IPlayer>();
			pieces = new Dictionary<ulong, IPiece>();
			InitializeFields();
		}
		protected void InitializeFields()
		{
			for( uint i = 0; i < GoalsHeight; ++i )
				for( uint j = 0; j < Width; ++j )
					fields[ i, j ] = new GoalField( i, j, TeamColour.Blue );
			for( uint i = GoalsHeight; i < Height - GoalsHeight; ++i )
				for( uint j = 0; j < Width; ++j )
					fields[ i, j ] = new TaskField( i, j );
			for( uint i = Height - GoalsHeight; i < Height; ++i )
				for( uint j = 0; j < Width; ++j )
					fields[ i, j ] = new GoalField( i, j, TeamColour.Red );
		}
		#endregion
	}
}
