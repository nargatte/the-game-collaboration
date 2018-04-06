//using Moq;
//using NUnit.Framework;
//using Shared.Components.Extensions;
//using Shared.Components.Pieces;
//using Shared.Components.Players;
//using Shared.Enums;
//using System;

//namespace SharedUnitTests.Components.Extensions
//{
//	[TestFixture]
//	public class ExtensionIPlayerPieceTest
//	{
//		#region Data
//		private static readonly DateTime dateTimeExample;
//		private static readonly IPlayer playerExample;
//		private static readonly object[] parametersWithIsDefault;
//		private static readonly object[] makePlayerPieceParameters;
//		private static readonly object[] parametersWithSetTypeParameter;
//		private static readonly object[] parametersWithSetTimestampParameter;
//		private static readonly object[] parametersWithSetPlayerParameter;
//		static ExtensionIPlayerPieceTest()
//		{
//			dateTimeExample = DateTime.Now;
//			playerExample = Mock.Of<IPlayer>();
//			parametersWithIsDefault = new object[]
//			{
//				new object[] { PieceType.Unknown, default( DateTime ), null, true },
//				new object[] { PieceType.Sham, default( DateTime ), null, false },
//				new object[] { PieceType.Unknown, dateTimeExample, null, false },
//				new object[] { PieceType.Unknown, default( DateTime ), playerExample, false }
//			};
//			makePlayerPieceParameters = new object[]
//			{
//				new object[] { 0ul, PieceType.Unknown, default( DateTime ), null },
//				new object[] { 1ul, PieceType.Sham, dateTimeExample, playerExample }
//			};
//			parametersWithSetTypeParameter = new object[]
//			{
//				new object[] { 0ul, PieceType.Unknown, default( DateTime ), null, PieceType.Normal },
//				new object[] { 1ul, PieceType.Sham, dateTimeExample, playerExample, PieceType.Unknown }
//			};
//			parametersWithSetTimestampParameter = new object[]
//			{
//				new object[] { 0ul, PieceType.Unknown, default( DateTime ), null, dateTimeExample },
//				new object[] { 1ul, PieceType.Sham, dateTimeExample, playerExample, default( DateTime ) }
//			};
//			parametersWithSetPlayerParameter = new object[]
//			{
//				new object[] { 0ul, PieceType.Unknown, default( DateTime ), null, playerExample },
//				new object[] { 1ul, PieceType.Sham, dateTimeExample, playerExample, null }
//			};
//		}
//		#endregion
//		#region Test
//		[TestCaseSource( nameof( parametersWithIsDefault ) )]
//		public void IsDefaultReturnsTrueIfAndOnlyIfObjectWasPrepopulated( PieceType type, DateTime timestamp, IPlayer player, bool expected )
//		{
//			var sut = Mock.Of<IPlayerPiece>( piece => piece.Type == type && piece.Timestamp == timestamp && piece.Player == player );
//			Assert.AreEqual( expected, sut.IsDefault() );
//		}
//		[TestCaseSource( nameof( makePlayerPieceParameters ) )]
//		public void MakeFieldPieceCallsCreatePlayerPieceWithProperParameters( ulong id, PieceType type, DateTime timestamp, IPlayer player )
//		{
//			var mock = new Mock<IPlayerPiece>();
//			var sut = mock.Object;
//			var result = sut.MakePlayerPiece( id, type, timestamp, player );
//			mock.Verify( piece => piece.CreatePlayerPiece( id, type, timestamp, player ) );
//		}
//		[TestCaseSource( nameof( parametersWithSetTypeParameter ) )]
//		public void SetPlayerCallsCreatePlayerPieceWithProperParameters( ulong id, PieceType type, DateTime timestamp, IPlayer player, PieceType value )
//		{
//			var mock = new Mock<IPlayerPiece>();
//			mock.SetupGet( piece => piece.Id ).Returns( id );
//			mock.SetupGet( piece => piece.Type ).Returns( type );
//			mock.SetupGet( piece => piece.Timestamp ).Returns( timestamp );
//			mock.SetupGet( piece => piece.Player ).Returns( player );
//			var sut = mock.Object;
//			var result = sut.SetType( value );
//			mock.Verify( piece => piece.CreatePlayerPiece( id, value, timestamp, player ) );
//		}
//		[TestCaseSource( nameof( parametersWithSetTimestampParameter ) )]
//		public void SetTimestampCallsCreatePlayerPieceWithProperParameters( ulong id, PieceType type, DateTime timestamp, IPlayer player, DateTime value )
//		{
//			var mock = new Mock<IPlayerPiece>();
//			mock.SetupGet( piece => piece.Id ).Returns( id );
//			mock.SetupGet( piece => piece.Type ).Returns( type );
//			mock.SetupGet( piece => piece.Timestamp ).Returns( timestamp );
//			mock.SetupGet( piece => piece.Player ).Returns( player );
//			var sut = mock.Object;
//			var result = sut.SetTimestamp( value );
//			mock.Verify( piece => piece.CreatePlayerPiece( id, type, value, player ) );
//		}
//		[TestCaseSource( nameof( parametersWithSetPlayerParameter ) )]
//		public void SetTimestampCallsCreateFieldPieceWithProperParameters( ulong id, PieceType type, DateTime timestamp, IPlayer player, IPlayer value )
//		{
//			var mock = new Mock<IPlayerPiece>();
//			mock.SetupGet( piece => piece.Id ).Returns( id );
//			mock.SetupGet( piece => piece.Type ).Returns( type );
//			mock.SetupGet( piece => piece.Timestamp ).Returns( timestamp );
//			mock.SetupGet( piece => piece.Player ).Returns( player );
//			var sut = mock.Object;
//			var result = sut.SetPlayer( value );
//			mock.Verify( piece => piece.CreatePlayerPiece( id, type, timestamp, value ) );
//		}
//		#endregion
//	}
//}
