using Shared.Components.Pieces;
using Shared.Components.Players;
using System;

namespace Shared.Components.Fields
{
	public class TaskField : Field, ITaskField
	{
		#region Field
		public override IField CloneField() => CloneTaskField();
		#endregion
		#region ITaskField
		public virtual int DistanceToPiece { get; set; }
		private IFieldPiece piece;
		public virtual IFieldPiece Piece
		{
			get => piece;
			set
			{
				if( piece != value )
				{
					if( piece != null )
						piece.Field = null;
					piece = value;
					if( piece != null )
						piece.Field = this;
				}
			}
		}
		public virtual ITaskField CloneTaskField()
		{
			var field = new TaskField( X, Y, Timestamp, null, DistanceToPiece, null );
			var player = Player;
			var piece = Piece;
			Player = null;
			Piece = null;
			var aPlayer = player.ClonePlayer();
			var aPiece = piece.CloneFieldPiece();
			Player = player;
			Piece = piece;
			field.Player = aPlayer;
			field.Piece = aPiece;
			return field;
		}
		#endregion
		#region TaskField
		public TaskField( uint x, uint y, DateTime timestamp = default, IPlayer player = null, int distanceToPiece = -1, IFieldPiece piece = null ) : base( x, y, timestamp, player )
		{
			DistanceToPiece = distanceToPiece;
			Piece = piece;
		}
		#endregion
	}
}
