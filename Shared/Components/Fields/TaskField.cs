using System;
using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.Enums;

namespace Shared.Components.Fields
{
	/// <summary>
	/// immutable
	/// </summary>
	public class TaskField : Field, ITaskField
	{
		#region Field
		public override IField CreateField( uint x, uint y, DateTime timestamp, IPlayer player ) => new TaskField( x, y, timestamp, player );
		#endregion
		#region ITaskField
		public virtual int DistanceToPiece { get; }
		public virtual IFieldPiece Piece { get; }
		public virtual ITaskField CreateTaskField( uint x, uint y, DateTime timestamp, IPlayer player, int distanceToPiece, IFieldPiece piece ) => new TaskField( x, y, timestamp, player, distanceToPiece, piece );
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
