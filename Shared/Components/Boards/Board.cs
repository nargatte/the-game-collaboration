using Shared.Components.Extensions;
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
		public override IField GetField( uint x, uint y ) => x < Width ? ( y < Height ? fields[ x, y ] : throw new ArgumentOutOfRangeException( nameof( y ) ) ) : throw new ArgumentOutOfRangeException( nameof( x ) );
		public override IPiece GetPiece( ulong id ) => pieces.TryGetValue( id, out var piece ) ? piece : throw new ArgumentOutOfRangeException( nameof( id ) );
		public override IPlayer GetPlayer( ulong id ) => players.TryGetValue( id, out var player ) ? player : throw new ArgumentOutOfRangeException( nameof( id ) );
		public override void SetField( IField value )
		{
			if( value is null )
				throw new ArgumentNullException( nameof( value ) );
			var field = GetField( value.X, value.Y );
			if( field is ITaskField taskField && value is ITaskField aTaskField )
				UpdateTaskField( taskField, aTaskField );
			else if( field is IGoalField goalField && value is IGoalField aGoalField )
				UpdateGoalField( goalField, aGoalField );
			else
				throw new ArgumentException( nameof( value ) );
		}
		public override void SetPiece( IPiece value )
		{
			if( value is null )
				throw new ArgumentNullException( nameof( value ) );
			pieces.TryGetValue( value.Id, out var piece );
			UpdatePiece( piece, value );
		}
		public override void SetPlayer( IPlayer value )
		{
			if( value is null )
				throw new ArgumentNullException( nameof( value ) );
			players.TryGetValue( value.Id, out var player );
			UpdatePlayer( player, value );
		}
		#endregion
		#region Board
		private readonly IField[,] fields;
		private readonly IDictionary<ulong, IPlayer> players;
		private readonly IDictionary<ulong, IPiece> pieces;
		public Board( uint width, uint tasksHeight, uint goalsHeight, IBoardComponentFactory factory ) : base( width, tasksHeight, goalsHeight, factory )
		{
			fields = new IField[ Width, Height ];
			players = new Dictionary<ulong, IPlayer>();
			pieces = new Dictionary<ulong, IPiece>();
			InitializeFields();
		}
		protected void InitializeFields()
		{
			//for( uint j = 0; j < GoalsHeight; ++j )
			//	for( uint i = 0; i < Width; ++i )
			//		fields[ i, j ] = Factory.GoalField.MakeGoalField( i, j, TeamColour.Blue );
			//for( uint j = GoalsHeight; j < Height - GoalsHeight; ++j )
			//	for( uint i = 0; i < Width; ++i )
			//		fields[ i, j ] = Factory.TaskField.MakeTaskField( i, j );
			//for( uint j = Height - GoalsHeight; j < Height; ++j )
			//	for( uint i = 0; i < Width; ++i )
			//		fields[ i, j ] = Factory.GoalField.MakeGoalField( i, j, TeamColour.Red );
		}
		protected void UpdateTaskField( ITaskField field, ITaskField value )
		{
			if( value is null )
				throw new ArgumentNullException( nameof( value ) );
			if( field != value )
			{
				if( field != null )
				{
					if( field.Player != null )
					{
						//fields[ field.X, field.Y ] = field.SetPlayer();
						//players[ field.Player.Id ] = field.Player.SetField();
						//OnFieldChanged( field.X, field.Y );
						//OnPlayerChanged( field.Player.Id );
					}
					if( field.Piece != null )
					{
						//fields[ field.X, field.Y ] = field.SetPiece();
						//pieces[ field.Piece.Id ] = field.Piece.SetField();
						//OnFieldChanged( field.X, field.Y );
						//OnPieceChanged( field.Piece.Id );
					}
				}
				fields[ value.X, value.Y ] = value;
				OnFieldChanged( value.X, value.Y );
			}
			if( value.Player != null )
				SetPlayer( value.Player );
			if( value.Piece != null )
				SetPiece( value.Piece );
		}
		protected void UpdateGoalField( IGoalField field, IGoalField value )
		{
			if( value is null )
				throw new ArgumentNullException( nameof( value ) );
			if( field != value )
			{
				if( field != null && field.Player != null )
				{
					//fields[ field.X, field.Y ] = field.SetPlayer();
					//players[ field.Player.Id ] = field.Player.SetField();
					//OnFieldChanged( field.X, field.Y );
					//OnPlayerChanged( field.Player.Id );
				}
				fields[ value.X, value.Y ] = value;
				OnFieldChanged( value.X, value.Y );
			}
			if( value.Player != null )
				SetPlayer( value.Player );
		}
		protected void UpdatePiece( IPiece piece, IPiece value )
		{
			if( value is null )
				throw new ArgumentNullException( nameof( value ) );
			if( piece != value )
			{
				if( piece != null )
					if( piece is IFieldPiece fieldPiece && fieldPiece.Field != null )
					{
						//pieces[ fieldPiece.Id ] = fieldPiece.SetField();
						//fields[ fieldPiece.Field.X, fieldPiece.Field.Y ] = fieldPiece.Field.SetPiece();
						//OnPieceChanged( fieldPiece.Id );
						//OnFieldChanged( fieldPiece.Field.X, fieldPiece.Field.Y );
					}
					else if( piece is IPlayerPiece playerPiece && playerPiece.Player != null )
					{
						//pieces[ playerPiece.Id ] = playerPiece.SetPlayer();
						//players[ playerPiece.Player.Id ] = playerPiece.Player.SetPiece();
						//OnPieceChanged( playerPiece.Id );
						//OnPlayerChanged( playerPiece.Player.Id );
					}
					else
						throw new ArgumentException( nameof( piece ) );
				pieces[ value.Id ] = value;
				OnPieceChanged( value.Id );
			}
			if( value is IFieldPiece aFieldPiece && aFieldPiece.Field != null )
				SetField( aFieldPiece.Field );
			else if( value is IPlayerPiece aPlayerPiece && aPlayerPiece.Player != null )
				SetPlayer( aPlayerPiece.Player );
			else
				throw new ArgumentException( nameof( value ) );
		}
		protected void UpdatePlayer( IPlayer player, IPlayer value )
		{
			if( value is null )
				throw new ArgumentNullException( nameof( value ) );
			if( player != value )
			{
				if( player != null )
				{
					if( player.Field != null )
					{
						//players[ player.Id ] = player.SetField();
						//fields[ player.Field.X, player.Field.Y ] = player.Field.SetPlayer();
						//OnPlayerChanged( player.Id );
						//OnFieldChanged( player.Field.X, player.Field.Y );
					}
					if( player.Piece != null )
					{
						//players[ player.Id ] = player.SetPiece();
						//pieces[ player.Piece.Id ] = player.Piece.SetPlayer();
						//OnPlayerChanged( player.Id );
						//OnPieceChanged( player.Piece.Id );
					}
				}
				players[ value.Id ] = value;
				OnPlayerChanged( value.Id );
			}
			if( value.Field != null )
				SetField( value.Field );
			if( value.Piece != null )
				SetPiece( value.Piece );
		}
		#endregion
	}
}
