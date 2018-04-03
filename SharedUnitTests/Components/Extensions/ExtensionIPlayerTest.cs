using Moq;
using NUnit.Framework;
using Shared.Components.Extensions;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.Enums;
using System;

namespace SharedUnitTests.Components.Extensions
{
	[TestFixture]
	public class ExtensionIPlayerTest
	{
		#region Data
		private static readonly DateTime dateTimeExample;
		private static readonly IField fieldExample;
		private static readonly IPlayerPiece playerPieceExample;
		private static readonly object[] parametersWithIsDefault;
		private static readonly object[] makePlayerParameters;
		private static readonly object[] parametersWithSetTimestampParameter;
		private static readonly object[] parametersWithSetFieldParameter;
		private static readonly object[] parametersWithSetPieceParameter;
		static ExtensionIPlayerTest()
		{
			dateTimeExample = DateTime.Now;
			fieldExample = Mock.Of<IField>();
			playerPieceExample = Mock.Of<IPlayerPiece>();
			parametersWithIsDefault = new object[]
			{
				new object[] { default( DateTime ), null, null, true },
				new object[] { dateTimeExample, null, null, false },
				new object[] { default( DateTime ), fieldExample, null, false },
				new object[] { dateTimeExample, fieldExample, null, false },
				new object[] { default( DateTime ), null, playerPieceExample, false },
				new object[] { dateTimeExample, null, playerPieceExample, false },
				new object[] { default( DateTime ), fieldExample, playerPieceExample, false },
				new object[] { dateTimeExample, fieldExample, playerPieceExample, false }
			};
			makePlayerParameters = new object[]
			{
				new object[] { 0ul, TeamColour.Red, PlayerType.Leader, default( DateTime ), null, null },
				new object[] { 1ul, TeamColour.Blue, PlayerType.Member, dateTimeExample, fieldExample, playerPieceExample }
			};
			parametersWithSetTimestampParameter = new object[]
			{
				new object[] { 0ul, TeamColour.Red, PlayerType.Leader, default( DateTime ), null, null, dateTimeExample },
				new object[] { 1ul, TeamColour.Blue, PlayerType.Member, dateTimeExample, fieldExample, playerPieceExample, default( DateTime ) }
			};
			parametersWithSetFieldParameter = new object[]
			{
				new object[] { 0ul, TeamColour.Red, PlayerType.Leader, default( DateTime ), null, null, fieldExample },
				new object[] { 1ul, TeamColour.Blue, PlayerType.Member, dateTimeExample, fieldExample, playerPieceExample, null }
			};
			parametersWithSetPieceParameter = new object[]
			{
				new object[] { 0ul, TeamColour.Red, PlayerType.Leader, default( DateTime ), null, null, playerPieceExample },
				new object[] { 1ul, TeamColour.Blue, PlayerType.Member, dateTimeExample, fieldExample, playerPieceExample, null }
			};
		}
		#endregion
		#region Test
		[TestCaseSource( nameof( parametersWithIsDefault ) )]
		public void IsDefaultReturnsTrueIfAndOnlyIfObjectWasPrepopulated( DateTime timestamp, IField field, IPlayerPiece piece, bool expected )
		{
			var sut = Mock.Of<IPlayer>( player => player.Timestamp == timestamp && player.Field == field && player.Piece == piece );
			Assert.AreEqual( expected, sut.IsDefault() );
		}
		[TestCaseSource( nameof( makePlayerParameters ) )]
		public void MakePlayerCallsCreatePlayerWithProperParameters( ulong id, TeamColour team, PlayerType type, DateTime timestamp, IField field, IPlayerPiece piece )
		{
			var mock = new Mock<IPlayer>();
			var sut = mock.Object;
			var result = sut.MakePlayer( id, team, type, timestamp, field, piece );
			mock.Verify( player => player.CreatePlayer( id, team, type, timestamp, field, piece ) );
		}
		[TestCaseSource( nameof( parametersWithSetTimestampParameter ) )]
		public void SetTimestampCallsCreatePlayerWithProperParameters( ulong id, TeamColour team, PlayerType type, DateTime timestamp, IField field, IPlayerPiece piece, DateTime value )
		{
			var mock = new Mock<IPlayer>();
			mock.SetupGet( player => player.Id ).Returns( id );
			mock.SetupGet( player => player.Team ).Returns( team );
			mock.SetupGet( player => player.Type ).Returns( type );
			mock.SetupGet( player => player.Timestamp ).Returns( timestamp );
			mock.SetupGet( player => player.Field ).Returns( field );
			mock.SetupGet( player => player.Piece ).Returns( piece );
			var sut = mock.Object;
			var result = sut.SetTimestamp( value );
			mock.Verify( player => player.CreatePlayer( id, team, type, value, field, piece ) );
		}
		[TestCaseSource( nameof( parametersWithSetFieldParameter ) )]
		public void SetFieldCallsCreatePlayerWithProperParameters( ulong id, TeamColour team, PlayerType type, DateTime timestamp, IField field, IPlayerPiece piece, IField value )
		{
			var mock = new Mock<IPlayer>();
			mock.SetupGet( player => player.Id ).Returns( id );
			mock.SetupGet( player => player.Team ).Returns( team );
			mock.SetupGet( player => player.Type ).Returns( type );
			mock.SetupGet( player => player.Timestamp ).Returns( timestamp );
			mock.SetupGet( player => player.Field ).Returns( field );
			mock.SetupGet( player => player.Piece ).Returns( piece );
			var sut = mock.Object;
			var result = sut.SetField( value );
			mock.Verify( player => player.CreatePlayer( id, team, type, timestamp, value, piece ) );
		}
		[TestCaseSource( nameof( parametersWithSetPieceParameter ) )]
		public void SetPieceCallsCreatePlayerWithProperParameters( ulong id, TeamColour team, PlayerType type, DateTime timestamp, IField field, IPlayerPiece piece, IPlayerPiece value )
		{
			var mock = new Mock<IPlayer>();
			mock.SetupGet( player => player.Id ).Returns( id );
			mock.SetupGet( player => player.Team ).Returns( team );
			mock.SetupGet( player => player.Type ).Returns( type );
			mock.SetupGet( player => player.Timestamp ).Returns( timestamp );
			mock.SetupGet( player => player.Field ).Returns( field );
			mock.SetupGet( player => player.Piece ).Returns( piece );
			var sut = mock.Object;
			var result = sut.SetPiece( value );
			mock.Verify( player => player.CreatePlayer( id, team, type, timestamp, field, value ) );
		}
		#endregion
	}
}
