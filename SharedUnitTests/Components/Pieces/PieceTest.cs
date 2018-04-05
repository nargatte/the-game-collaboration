using NUnit.Framework;
using Shared.Components.Pieces;
using Shared.Enums;
using System;

namespace SharedUnitTests.Components.Pieces
{
	[TestFixture]
	public class PieceTest
	{
		#region Data
		private class PieceTestType : Piece
		{
			#region Piece
			public override IPiece CreatePiece( ulong id, PieceType type, DateTime timestamp ) => throw new NotImplementedException();
			#endregion
			#region PieceTestType
			public PieceTestType( ulong id, PieceType type = PieceType.Unknown, DateTime timestamp = default ) : base( id, type, timestamp )
			{
			}
			#endregion
		}
		private static readonly DateTime dateTimeExample;
		private static readonly object[] constructorParameters;
		static PieceTest()
		{
			dateTimeExample = DateTime.Now;
			constructorParameters = new object[]
			{
				new object[] { 0ul, PieceType.Unknown, default( DateTime ) },
				new object[] { 1ul, PieceType.Sham, dateTimeExample }
			};
		}
		#endregion
		#region Test
		[TestCaseSource( nameof( constructorParameters ) )]
		public void ConstructorFillsAllProperties( ulong id, PieceType type, DateTime timestamp )
		{
			var sut = new PieceTestType( id, type, timestamp );
			Assert.Multiple( () =>
			{
				Assert.AreEqual( id, sut.Id );
				Assert.AreEqual( type, sut.Type );
				Assert.AreEqual( timestamp, sut.Timestamp );
			} );
		}
		#endregion
	}
}
