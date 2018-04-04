using Shared.Components.Factories;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.Enums;
using System;
using System.Collections.Generic;

namespace Shared.Components.Boards
{
	public class Board : BoardBase
	{
		#region IBoard
		public override IEnumerable<IField> Fields
		{
			get
			{
				foreach( var field in fields )
					yield return field;
			}
		}
		public override IEnumerable<IPiece> Pieces
		{
			get
			{
				foreach( var piece in pieces )
					yield return piece.Value;
			}
		}
		public override IEnumerable<IPlayer> Players
		{
			get
			{
				foreach( var player in players )
					yield return player.Value;
			}
		}
		public override IField GetField( uint x, uint y ) => x < Width && y < Height ? fields[ x, y ] : null;
		public override IPiece GetPiece( ulong id ) => pieces.TryGetValue( id, out var piece ) ? piece : null;
		public override IPlayer GetPlayer( ulong id ) => players.TryGetValue( id, out var player ) ? player : null;
		public override void SetField( IField value )
		{
			if( value is null )
				return;
			var field = GetField( value.X, value.Y );
			if( field is null || field == value )
				return;
			if( field is ITaskField oldTask && value is ITaskField freshTask )
				UpdateTaskField( oldTask, freshTask );
			if( field is IGoalField oldGoal && value is IGoalField freshGoal )
				UpdateGoalField( oldGoal, freshGoal );
		}
		public override void SetPiece( IPiece value ) => throw new NotImplementedException();
		public override void SetPlayer( IPlayer value ) => throw new NotImplementedException();
		#endregion
		#region Board
		private readonly IField[,] fields;
		private readonly IDictionary<ulong, IPlayer> players;
		private readonly IDictionary<ulong, IPiece> pieces;
		public Board( uint width, uint tasksHeight, uint goalsHeight, IBoardPrototypeFactory factory ) : base( width, tasksHeight, goalsHeight, factory )
		{
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
		protected void UpdateTaskField( ITaskField old, ITaskField fresh ) => throw new NotImplementedException();
		protected void UpdateGoalField( IGoalField old, IGoalField fresh ) => throw new NotImplementedException();
		#endregion
	}
}
