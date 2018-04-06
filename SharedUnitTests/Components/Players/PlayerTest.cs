//using Moq;
//using NUnit.Framework;
//using Shared.Components.Fields;
//using Shared.Components.Pieces;
//using Shared.Components.Players;
//using Shared.Enums;
//using System;

//namespace SharedUnitTests.Components.Players
//{
//	[TestFixture]
//	public class PlayerTest
//	{
//		#region Data
//		private static readonly DateTime dateTimeExample;
//		private static readonly IField fieldExample;
//		private static readonly IPlayerPiece playerPieceExample;
//		private static readonly object[] constructorParameters;
//		private static readonly object[] parametersWithCreatePlayerParameters;
//		static PlayerTest()
//		{
//			dateTimeExample = DateTime.Now;
//			fieldExample = Mock.Of<IField>();
//			playerPieceExample = Mock.Of<IPlayerPiece>();
//			constructorParameters = new object[]
//			{
//				new object[] { 0ul, TeamColour.Red, PlayerType.Leader, default( DateTime ), null, null },
//				new object[] { 1ul, TeamColour.Blue, PlayerType.Member, dateTimeExample, fieldExample, playerPieceExample }
//			};
//			parametersWithCreatePlayerParameters = new object[]
//			{
//				new object[] { 0ul, TeamColour.Red, PlayerType.Leader, 2ul, TeamColour.Blue, PlayerType.Member, default( DateTime ), null, null },
//				new object[] { 1ul, TeamColour.Blue, PlayerType.Member, 3ul, TeamColour.Red, PlayerType.Leader, dateTimeExample, fieldExample, playerPieceExample }
//			};
//		}
//		#endregion
//		#region Test
//		[TestCaseSource( nameof( constructorParameters ) )]
//		public void ConstructorFillsAllProperties( ulong id, TeamColour team, PlayerType type, DateTime timestamp, IField field, IPlayerPiece piece )
//		{
//			var sut = new Player( id, team, type, timestamp, field, piece );
//			Assert.Multiple( () =>
//			{
//				Assert.AreEqual( id, sut.Id );
//				Assert.AreEqual( team, sut.Team );
//				Assert.AreEqual( type, sut.Type );
//				Assert.AreEqual( timestamp, sut.Timestamp );
//				Assert.AreSame( field, sut.Field );
//				Assert.AreSame( piece, sut.Piece );
//			} );
//		}
//		[TestCaseSource( nameof( parametersWithCreatePlayerParameters ) )]
//		public void CreatePlayerReturnsObjectWithFilledProperties( ulong id, TeamColour team, PlayerType type, ulong aId, TeamColour aTeam, PlayerType aType, DateTime aTimestamp, IField aField, IPlayerPiece aPiece )
//		{
//			var sut = new Player( id, team, type );
//			var result = sut.CreatePlayer( aId, aTeam, aType, aTimestamp, aField, aPiece );
//			Assert.Multiple( () =>
//			{
//				Assert.AreEqual( aId, result.Id );
//				Assert.AreEqual( aTeam, result.Team );
//				Assert.AreEqual( aType, result.Type );
//				Assert.AreEqual( aTimestamp, result.Timestamp );
//				Assert.AreSame( aField, result.Field );
//				Assert.AreSame( aPiece, result.Piece );
//			} );
//		}
//		#endregion
//	}
//}