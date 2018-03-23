using System;
using Shared.Components.Pieces;
using Shared.Components.Players;

namespace Shared.Components.Fields
{
	/// <summary>
	/// immutable
	/// </summary>
	public class TaskField : Field, ITaskField
	{
		#region ITaskField
		public virtual int DistanceToPiece { get; }
		public virtual IFieldPiece Piece { get; }
		#endregion
		#region TaskField
		public TaskField( uint x, uint y, DateTime timestamp = default( DateTime ), IPlayer player = null, int distanceToPiece = -1, IFieldPiece piece = null ) : base( x, y, timestamp, player )
		{
			DistanceToPiece = distanceToPiece;
			Piece = piece;
		}
		public TaskField( ITaskField field ) : base( field )
		{
			DistanceToPiece = field.DistanceToPiece;
			Piece = field.Piece;
		}
		#endregion
	}
}
