using Moq;
using NUnit.Framework;
using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.Enums;
using System;

namespace SharedUnitTests.Components.Pieces
{
	[TestFixture]
	public class PlayerPieceTest
	{
		#region Data
		private static readonly DateTime dateTimeExample;
		private static readonly IPlayer playerExample;
		private static readonly object[] constructorParameters;
		private static readonly object[] parametersWithCreatePieceParameters;
		private static readonly object[] parametersWithCreatePlayerPieceParameters;

		static PlayerPieceTest()
		{
			dateTimeExample = DateTime.Now;
			playerExample = Mock.Of<IPlayer>();
			constructorParameters = new object[]
			{
				new object[] { 0ul, PieceType.Unknown, default( DateTime ), null },
				new object[] { 1ul, PieceType.Sham, dateTimeExample, playerExample }
			};
			parametersWithCreatePieceParameters = new object[]
			{
				new object[] { 0ul, 2ul, PieceType.Unknown, default( DateTime ) },
				new object[] { 1ul, 3ul, PieceType.Sham, dateTimeExample }
			};
			parametersWithCreatePlayerPieceParameters = new object[]
			{
				new object[] { 0ul, 2ul, PieceType.Unknown, default( DateTime ), null },
				new object[] { 1ul, 3ul, PieceType.Sham, dateTimeExample, playerExample }
			};
		}
		#endregion
		#region Test
		[TestCaseSource( nameof( constructorParameters ) )]
		public void ConstructorFillsAllProperties( ulong id, PieceType type, DateTime timestamp, IPlayer player )
		{
			var sut = new PlayerPiece( id, type, timestamp, player );
			Assert.Multiple( () =>
			{
				Assert.AreEqual( id, sut.Id );
				Assert.AreEqual( type, sut.Type );
				Assert.AreEqual( timestamp, sut.Timestamp );
				Assert.AreSame( player, sut.Player );
			} );
		}
		[TestCaseSource( nameof( parametersWithCreatePieceParameters ) )]
		public void CreatePieceReturnsObjectWithFilledProperties( ulong id, ulong aId, PieceType aType, DateTime aTimestamp )
		{
			var sut = new PlayerPiece( id );
			var result = sut.CreatePiece( aId, aType, aTimestamp );
			Assert.Multiple( () =>
			{
				Assert.AreEqual( aId, result.Id );
				Assert.AreEqual( aType, result.Type );
				Assert.AreEqual( aTimestamp, result.Timestamp );
			} );
		}
		[TestCaseSource( nameof( parametersWithCreatePlayerPieceParameters ) )]
		public void CreateFieldPieceReturnsObjectWithFilledProperties( ulong id, ulong aId, PieceType aType, DateTime aTimestamp, IPlayer aPlayer )
		{
			var sut = new PlayerPiece( id );
			var result = sut.CreatePlayerPiece( aId, aType, aTimestamp, aPlayer );
			Assert.Multiple( () =>
			{
				Assert.AreEqual( aId, result.Id );
				Assert.AreEqual( aType, result.Type );
				Assert.AreEqual( aTimestamp, result.Timestamp );
				Assert.AreEqual( aPlayer, result.Player );
			} );
		}
		#endregion
	}
}
