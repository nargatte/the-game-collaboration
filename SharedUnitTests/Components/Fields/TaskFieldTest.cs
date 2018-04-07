using Moq;
using NUnit.Framework;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using System;

namespace SharedUnitTests.Components.Fields
{
	[TestFixture]
	public class TaskFieldTest
	{
		#region Data
		private static readonly DateTime dateTimeExample;
		private static readonly IPlayer playerExample;
		private static readonly IFieldPiece fieldPieceExample;
		private static readonly object[] constructorParameters;
		private static readonly object[] parametersWithSetParameters;
		static TaskFieldTest()
		{
			dateTimeExample = DateTime.Now;
			playerExample = Mock.Of<IPlayer>();
			fieldPieceExample = Mock.Of<IFieldPiece>();
			constructorParameters = new object[]
			{
				new object[] { 0u, 2u, default( DateTime ), null, -1, null },
				new object[] { 1u, 3u, dateTimeExample, playerExample, 4, fieldPieceExample }
			};
			parametersWithSetParameters = new object[]
			{
				new object[] { 0u, 2u, default( DateTime ), null, -1, null, 5u, 7u, dateTimeExample, 9 },
				new object[] { 1u, 3u, dateTimeExample, playerExample, 4, fieldPieceExample, 6u, 8u, default( DateTime ), 10 }
			};
		}
		#endregion
		#region Test
		[TestCaseSource( nameof( constructorParameters ) )]
		public void ConstructorFillsAllProperties( uint x, uint y, DateTime timestamp, IPlayer player, int distanceToPiece, IFieldPiece piece )
		{
			var sut = new TaskField( x, y, timestamp, player, distanceToPiece, piece );
			Assert.Multiple( () =>
			{
				Assert.AreEqual( x, sut.X );
				Assert.AreEqual( y, sut.Y );
				Assert.AreEqual( timestamp, sut.Timestamp );
				Assert.AreSame( player, sut.Player );
				Assert.AreEqual( distanceToPiece, sut.DistanceToPiece );
				Assert.AreSame( piece, sut.Piece );
			} );
		}
		[TestCaseSource( nameof( parametersWithSetParameters ) )]
		public void SetForNonNavigationalPropertyTracksValue( uint x, uint y, DateTime timestamp, IPlayer player, int distanceToPiece, IFieldPiece piece, uint aX, uint aY, DateTime aTimestamp, int aDistanceToPiece )
		{
			var sut = new TaskField( x, y, timestamp, player, distanceToPiece, piece )
			{
				X = aX,
				Y = aY,
				Timestamp = aTimestamp,
				DistanceToPiece = aDistanceToPiece
			};
			Assert.Multiple( () =>
			{
				Assert.AreEqual( aX, sut.X );
				Assert.AreEqual( aY, sut.Y );
				Assert.AreEqual( aTimestamp, sut.Timestamp );
				Assert.AreEqual( aDistanceToPiece, sut.DistanceToPiece );
			} );
		}
		#endregion
	}
}