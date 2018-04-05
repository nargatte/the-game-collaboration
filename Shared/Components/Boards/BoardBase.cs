using Shared.Components.Events;
using Shared.Components.Factories;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
			Width = width;
			TasksHeight = tasksHeight;
			GoalsHeight = goalsHeight;
			Height = TasksHeight + 2 * GoalsHeight;
			if( factory is null )
				throw new ArgumentNullException( nameof( factory ) );
			Factory = factory;
		}
		protected void OnEvent<T>( EventHandler<T> eventHandler, T eventArgs, [CallerMemberName] string eventName = null )
		{
			var exceptionList = new List<Exception>();
			foreach( EventHandler<T> handler in eventHandler.GetInvocationList() )
				try
				{
					handler( this, eventArgs );
				}
				catch( Exception exception )
				{
					exceptionList.Add( exception );
				}
			if( exceptionList.Count > 0 )
				throw new AggregateException( $"Exceptions thrown by event subscribers called in { eventName }.", exceptionList );
		}
		protected void OnFieldChanged( uint x, uint y ) => OnEvent( FieldChanged, new FieldChangedArgs( x, y ) );
		protected void OnPieceChanged( ulong id ) => OnEvent( PieceChanged, new PieceChangedArgs( id ) );
		protected void OnPlayerChanged( ulong id ) => OnEvent( PlayerChanged, new PlayerChangedArgs( id ) );
		#endregion
	}
}
