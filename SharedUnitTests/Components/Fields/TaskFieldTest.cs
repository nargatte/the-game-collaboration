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
		private static readonly IPlayer playerExample2;
		private static readonly IFieldPiece fieldPieceExample;
		private static readonly object[] constructorParameters;
		private static readonly object[] parametersWithSetParameters;
		private static readonly object[] parametersWithSetPlayerParameter;
		static TaskFieldTest()
		{
			dateTimeExample = DateTime.Now;
			playerExample = Mock.Of<IPlayer>();
			var mockPlayer = new Mock<IPlayer>();
			mockPlayer.SetupProperty( player => player.Field );
			playerExample2 = mockPlayer.Object;
			fieldPieceExample = Mock.Of<IFieldPiece>();
			constructorParameters = new object[]
			{
				new object[] { 0u, 1u, default( DateTime ), null, -1, null },
				new object[] { 2u, 3u, dateTimeExample, playerExample, 4, fieldPieceExample }
			};
			parametersWithSetParameters = new object[]
			{
				new object[] { 0u, 1u, default( DateTime ), null, -1, null, 2u, 3u, dateTimeExample, 4 },
				new object[] { 5u, 6u, dateTimeExample, playerExample, 7, fieldPieceExample, 8u, 9u, default( DateTime ), 10 }
			};
			parametersWithSetPlayerParameter = new object[]
			{
				new object[] { 0u, 1u, default( DateTime ), null, -1, null, null },
				new object[] { 3u, 4u, dateTimeExample, playerExample, 5, fieldPieceExample, null },
				new object[] { 6u, 7u, default( DateTime ), null, -1, null, playerExample2 },
				new object[] { 8u, 9u, dateTimeExample, playerExample, 10, fieldPieceExample, playerExample2 }
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
		[TestCaseSource( nameof( parametersWithSetPlayerParameter ) )]
		public void SetPlayerTracksValueAndLinksObjects( uint x, uint y, DateTime timestamp, IPlayer player, int distanceToPiece, IFieldPiece piece, IPlayer value )
		{
			var sut = new TaskField( x, y, timestamp, player )
			{
				Player = value
			};
			Assert.Multiple( () =>
			{
				Assert.AreSame( value, sut.Player );
				if( player != null )
					Assert.IsNull( player.Field );
				if( value != null )
					Assert.AreSame( sut, sut.Player.Field );
			} );
		}
		#endregion
	}
}