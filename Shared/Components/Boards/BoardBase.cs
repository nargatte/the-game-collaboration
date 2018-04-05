using Shared.Components.Events;
using Shared.Components.Factories;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using System;
using System.Collections.Generic;

namespace Shared.Components.Boards
{
	public abstract class BoardBase : IBoard
	{
		#region IBoard
		public virtual uint Width { get; }
		public virtual uint TasksHeight { get; }
		public virtual uint GoalsHeight { get; }
		public virtual uint Height { get; }
		public abstract IEnumerable<IField> Fields { get; }
		public abstract IEnumerable<IPiece> Pieces { get; }
		public abstract IEnumerable<IPlayer> Players { get; }
		public virtual IBoardPrototypeFactory Factory { get; }
		public abstract IField GetField( uint x, uint y );
		public abstract IPiece GetPiece( ulong id );
		public abstract IPlayer GetPlayer( ulong id );
		public abstract void SetField( IField value );
		public abstract void SetPlayer( IPlayer value );
		public abstract void SetPiece( IPiece value );
		public virtual event EventHandler<FieldChangedArgs> FieldChanged = delegate { };
		public virtual event EventHandler<PieceChangedArgs> PieceChanged = delegate { };
		public virtual event EventHandler<PlayerChangedArgs> PlayerChanged = delegate { };
		#endregion
		#region BoardBase
		protected BoardBase( uint width, uint tasksHeight, uint goalsHeight, IBoardPrototypeFactory factory )
		{
			if( width == 0u )
				throw new ArgumentOutOfRangeException( nameof( width ) );
			if( tasksHeight == 0u )
				throw new ArgumentOutOfRangeException( nameof( tasksHeight ) );
			if( goalsHeight == 0u )
				throw new ArgumentOutOfRangeException( nameof( goalsHeight ) );
			if( factory is null )
				throw new ArgumentNullException( nameof( factory ) );
			Width = width;
			TasksHeight = tasksHeight;
			GoalsHeight = goalsHeight;
			Height = TasksHeight + 2u * GoalsHeight;
			Factory = factory;
		}
		protected void OnFieldChanged( uint x, uint y ) => EventHelper.OnEvent( this, FieldChanged, new FieldChangedArgs( x, y ) );
		protected void OnPieceChanged( ulong id ) => EventHelper.OnEvent( this, PieceChanged, new PieceChangedArgs( id ) );
		protected void OnPlayerChanged( ulong id ) => EventHelper.OnEvent( this, PlayerChanged, new PlayerChangedArgs( id ) );
		#endregion
	}
}
