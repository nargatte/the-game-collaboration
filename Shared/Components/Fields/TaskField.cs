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
		public TaskField( uint x, uint y, DateTime timestamp = default, IPlayer player = null, int distanceToPiece = -1, IFieldPiece piece = null ) : base( x, y, timestamp, player )
		{
			DistanceToPiece = distanceToPiece;
			Piece = piece;
		}

		public override IField CreateField( uint x, uint y, DateTime timestamp, IPlayer player ) => throw new NotImplementedException();
		#endregion
	}
}
