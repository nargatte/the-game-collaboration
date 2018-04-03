using System;
using Moq;
using NUnit.Framework;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;

namespace SharedUnitTests.Components.Fields
{
	[TestFixture]
	public class TaskFieldTest
	{
		#region Data
		private static readonly DateTime dateTimeExample;
		private static readonly IPlayer playerExample;
		private static readonly IFieldPiece pieceExample;
		private static readonly object[] constructorParameters;
		private static readonly object[] parametersWithCreateFieldParameters;
		private static readonly object[] parametersWithCreateTaskFieldParameters;
		static TaskFieldTest()
		{
			dateTimeExample = DateTime.Now;
			playerExample = new Mock<IPlayer>().Object;
			pieceExample = new Mock<IFieldPiece>().Object;
			constructorParameters = new object[]
			{
				new object[] { 0u, 2u, default( DateTime ), null, -1, null },
				new object[] { 1u, 3u, dateTimeExample, playerExample, 4, pieceExample }
			};
			parametersWithCreateFieldParameters = new object[]
			{
				new object[] { 0u, 2u, 4u, 6u, default( DateTime ), null },
				new object[] { 1u, 3u, 5u, 7u, dateTimeExample, playerExample }
			};
			parametersWithCreateTaskFieldParameters = new object[]
			{
				new object[] { 0u, 2u, 4u, 6u, default( DateTime ), null, -1, null },
				new object[] { 1u, 3u, 5u, 7u, dateTimeExample, playerExample, 4, pieceExample }
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
				Assert.AreEqual( piece, sut.Piece );
			} );
		}
		[TestCaseSource( nameof( parametersWithCreateFieldParameters ) )]
		public void CreateFieldReturnsObjectWithFilledProperties( uint x, uint y, uint aX, uint aY, DateTime aTimestamp, IPlayer aPlayer )
		{
			var sut = new TaskField( x, y );
			var result = sut.CreateField( aX, aY, aTimestamp, aPlayer );
			Assert.Multiple( () =>
			{
				Assert.AreEqual( aX, result.X );
				Assert.AreEqual( aY, result.Y );
				Assert.AreEqual( aTimestamp, result.Timestamp );
				Assert.AreSame( aPlayer, result.Player );
			} );
		}
		[TestCaseSource( nameof( parametersWithCreateTaskFieldParameters ) )]
		public void CreateTaskFieldReturnsObjectWithFilledProperties( uint x, uint y, uint aX, uint aY, DateTime aTimestamp, IPlayer aPlayer, int aDistanceToPiece, IFieldPiece aPiece )
		{
			var sut = new TaskField( x, y );
			var result = sut.CreateTaskField( aX, aY, aTimestamp, aPlayer, aDistanceToPiece, aPiece );
			Assert.Multiple( () =>
			{
				Assert.AreEqual( aX, result.X );
				Assert.AreEqual( aY, result.Y );
				Assert.AreEqual( aTimestamp, result.Timestamp );
				Assert.AreSame( aPlayer, result.Player );
				Assert.AreEqual( aDistanceToPiece, result.DistanceToPiece );
				Assert.AreSame( aPiece, result.Piece );
			} );
		}
		#endregion
	}
}