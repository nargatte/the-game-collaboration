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
					yield return field.CloneField();
			}
		}
		public override IEnumerable<IPiece> Pieces
		{
			get
			{
				foreach( var piece in pieces )
					yield return piece.Value.ClonePiece();
			}
		}
		public override IEnumerable<IPlayer> Players
		{
			get
			{
				foreach( var player in players )
					yield return player.Value.ClonePlayer();
			}
		}
		public override IField GetField( uint x, uint y )// => x < Width ? ( y < Height ? fields[ x, y ].CloneField() : throw new ArgumentOutOfRangeException( nameof( y ) ) ) : throw new ArgumentOutOfRangeException( nameof( x ) );
        {
            Console.WriteLine($"{ x } { y } { Width } { Height }");
            return x < Width ? (y < Height ? fields[x, y].CloneField() : throw new ArgumentOutOfRangeException(nameof(y))) : throw new ArgumentOutOfRangeException(nameof(x));
        }
		public override IPiece GetPiece( ulong id ) => pieces.TryGetValue( id, out var piece ) ? piece.ClonePiece() : throw new ArgumentOutOfRangeException( nameof( id ) );
		public override IPlayer GetPlayer( ulong id ) => players.TryGetValue( id, out var player ) ? player.ClonePlayer() : throw new ArgumentOutOfRangeException( nameof( id ) );
		public override void SetField( IField value )
		{
			if( value is null )
				throw new ArgumentNullException( nameof( value ) );
			UpdateField( value.CloneField() );
		}
		public override void SetPiece( IPiece value )
		{
			if( value is null )
				throw new ArgumentNullException( nameof( value ) );
			UpdatePiece( value.ClonePiece() );
		}
		public override void SetPlayer( IPlayer value )
		{
			if( value is null )
				throw new ArgumentNullException( nameof( value ) );
			UpdatePlayer( value.ClonePlayer() );
		}
		#endregion
		#region Board
		private readonly IField[,] fields;
		private readonly IDictionary<ulong, IPiece> pieces;
		private readonly IDictionary<ulong, IPlayer> players;
		public Board( uint width, uint tasksHeight, uint goalsHeight, IBoardComponentFactory factory ) : base( width, tasksHeight, goalsHeight, factory )
		{
			fields = new IField[ Width, Height ];
			pieces = new Dictionary<ulong, IPiece>();
			players = new Dictionary<ulong, IPlayer>();
			InitializeFields();
		}
		protected void InitializeFields()
		{
			for( uint j = 0; j < GoalsHeight; ++j )
				for( uint i = 0; i < Width; ++i )
					fields[ i, j ] = Factory.MakeGoalField( i, j, TeamColour.Blue );
			for( uint j = GoalsHeight; j < Height - GoalsHeight; ++j )
				for( uint i = 0; i < Width; ++i )
					fields[ i, j ] = Factory.MakeTaskField( i, j );
			for( uint j = Height - GoalsHeight; j < Height; ++j )
				for( uint i = 0; i < Width; ++i )
					fields[ i, j ] = Factory.MakeGoalField( i, j, TeamColour.Red );
		}
		protected void UpdateField( IField value )
		{
			if( value is null )
				throw new ArgumentNullException( nameof( value ) );
			if( value.X >= Width || value.Y >= Height )
				throw new ArgumentOutOfRangeException( nameof( value ) );
			var field = fields[ value.X, value.Y ];
			if( field is ITaskField taskField && value is ITaskField aTaskField )
				UpdateTaskField( taskField, aTaskField );
			else if( field is IGoalField goalField && value is IGoalField aGoalField )
				UpdateGoalField( goalField, aGoalField );
			else
				throw new ArgumentException( nameof( value ) );
		}
		protected void UpdatePiece( IPiece value )
		{
			if( value is null )
				throw new ArgumentNullException( nameof( value ) );
			pieces.TryGetValue( value.Id, out var piece );
			UpdatePiece( piece, value );
		}
		protected void UpdatePlayer( IPlayer value )
		{
			if( value is null )
				throw new ArgumentNullException( nameof( value ) );
			players.TryGetValue( value.Id, out var player );
			UpdatePlayer( player, value );
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
						var player = field.Player;
						field.Player = null;
						OnFieldChanged( field.X, field.Y );
						OnPlayerChanged( player.Id );
					}
					if( field.Piece != null )
					{
						var piece = field.Piece;
						field.Piece = null;
						OnFieldChanged( field.X, field.Y );
						OnPieceChanged( piece.Id );
					}
				}
				fields[ value.X, value.Y ] = value;
				OnFieldChanged( value.X, value.Y );
			}
			if( value.Player != null )
			{
				var player = value.Player;
				value.Player = null;
				UpdatePlayer( player );
				value.Player = player;
			}
			if( value.Piece != null )
			{
				var piece = value.Piece;
				value.Piece = null;
				UpdatePiece( piece );
				value.Piece = piece;
			}
		}
		protected void UpdateGoalField( IGoalField field, IGoalField value )
		{
			if( value is null )
				throw new ArgumentNullException( nameof( value ) );
			if( field != value )
			{
				if( field != null && field.Player != null )
				{
					var player = field.Player;
					field.Player = null;
					OnFieldChanged( field.X, field.Y );
					OnPlayerChanged( player.Id );
				}
				fields[ value.X, value.Y ] = value;
				OnFieldChanged( value.X, value.Y );
			}
			if( value.Player != null )
			{
				var player = value.Player;
				value.Player = null;
				UpdatePlayer( player );
				value.Player = player;
			}
		}
		protected void UpdatePiece( IPiece piece, IPiece value )
		{
			if( value is null )
				throw new ArgumentNullException( nameof( value ) );
			if( piece != value )
			{
				if( piece != null )
					if( piece is IFieldPiece fieldPiece )
					{
						if( fieldPiece.Field != null )
						{
							var field = fieldPiece.Field;
							fieldPiece.Field = null;
							OnPieceChanged( fieldPiece.Id );
							OnFieldChanged( field.X, field.Y );
						}
					}
					else if( piece is IPlayerPiece playerPiece )
					{
						if( playerPiece.Player != null )
						{
							var player = playerPiece.Player;
							playerPiece.Player = null;
							OnPieceChanged( playerPiece.Id );
							OnPlayerChanged( player.Id );
						}
					}
					else
						throw new ArgumentException( nameof( piece ) );
				pieces[ value.Id ] = value;
				OnPieceChanged( value.Id );
			}
			if( value is IFieldPiece aFieldPiece )
			{
				if( aFieldPiece.Field != null )
				{
					var field = aFieldPiece.Field;
					aFieldPiece.Field = null;
					UpdateField( field );
					aFieldPiece.Field = field;
				}
			}
			else if( value is IPlayerPiece aPlayerPiece )
			{
				if( aPlayerPiece.Player != null )
				{
					var player = aPlayerPiece.Player;
					aPlayerPiece.Player = null;
					UpdatePlayer( player );
					aPlayerPiece.Player = player;
				}
			}
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
						var field = player.Field;
						player.Field = null;
						OnPlayerChanged( player.Id );
						OnFieldChanged( field.X, field.Y );
					}
					if( player.Piece != null )
					{
						var piece = player.Piece;
						player.Piece = null;
						OnPlayerChanged( player.Id );
						OnPieceChanged( piece.Id );
					}
				}
				players[ value.Id ] = value;
				OnPlayerChanged( value.Id );
			}
			if( value.Field != null )
			{
				var field = value.Field;
				value.Field = null;
				UpdateField( field );
				value.Field = field;
			}
			if( value.Piece != null )
			{
				var piece = value.Piece;
				value.Piece = null;
				UpdatePiece( piece );
				value.Piece = piece;
			}
		}
		#endregion
	}
}
