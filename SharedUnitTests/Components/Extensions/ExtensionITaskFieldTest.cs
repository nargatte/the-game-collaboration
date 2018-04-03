using Moq;
using NUnit.Framework;
using Shared.Components.Extensions;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using System;

namespace SharedUnitTests.Components.Extensions
{
	[TestFixture]
	public class ExtensionITaskFieldTest
	{
		#region Data
		private static readonly DateTime dateTimeExample;
		private static readonly IPlayer playerExample;
		private static readonly IFieldPiece pieceExample;
		private static readonly object[] parametersWithIsDefault;
		private static readonly object[] makeTaskFieldParameters;
		private static readonly object[] parametersWithSetTimestampParameter;
		private static readonly object[] parametersWithSetPlayerParameter;
		private static readonly object[] parametersWithSetDistanceToPieceParameter;
		private static readonly object[] parametersWithSetPieceParameter;
		static ExtensionITaskFieldTest()
		{
			dateTimeExample = DateTime.Now;
			playerExample = new Mock<IPlayer>().Object;
			pieceExample = new Mock<IFieldPiece>().Object;
			parametersWithIsDefault = new object[]
			{
				new object[] { default( DateTime ), null, -1, null, true },
				new object[] { dateTimeExample, null, -1, null, false },
				new object[] { default( DateTime ), playerExample, -1, null, false },
				new object[] { dateTimeExample, playerExample, -1, null, false },
				new object[] { default( DateTime ), null, 0, null, false },
				new object[] { dateTimeExample, null, 1, null, false },
				new object[] { default( DateTime ), playerExample, 2, null, false },
				new object[] { dateTimeExample, playerExample, 3, null, false },
				new object[] { default( DateTime ), null, -1, pieceExample, false },
				new object[] { dateTimeExample, null, -1, pieceExample, false },
				new object[] { default( DateTime ), playerExample, -1, pieceExample, false },
				new object[] { dateTimeExample, playerExample, -1, pieceExample, false },
				new object[] { default( DateTime ), null, 0, pieceExample, false },
				new object[] { dateTimeExample, null, 1, pieceExample, false },
				new object[] { default( DateTime ), playerExample, 2, pieceExample, false },
				new object[] { dateTimeExample, playerExample, 3, pieceExample, false }
			};
			makeTaskFieldParameters = new object[]
			{
				new object[] { 0u, 2u, default( DateTime ), null, -1, null },
				new object[] { 1u, 3u, dateTimeExample, playerExample, 0, pieceExample }
			};
			parametersWithSetTimestampParameter = new object[]
			{
				new object[] { 0u, 2u, default( DateTime ), null, -1, null, dateTimeExample },
				new object[] { 1u, 3u, dateTimeExample, playerExample, 0, pieceExample, default( DateTime ) }
			};
			parametersWithSetPlayerParameter = new object[]
			{
				new object[] { 0u, 2u, default( DateTime ), null, -1, null, playerExample },
				new object[] { 1u, 3u, dateTimeExample, playerExample, 0, pieceExample, null }
			};
			parametersWithSetDistanceToPieceParameter = new object[]
			{
				new object[] { 0u, 2u, default( DateTime ), null, -1, null, 1 },
				new object[] { 1u, 3u, dateTimeExample, playerExample, 0, pieceExample, -1 }
			};
			parametersWithSetPieceParameter = new object[]
			{
				new object[] { 0u, 2u, default( DateTime ), null, -1, null, pieceExample },
				new object[] { 1u, 3u, dateTimeExample, playerExample, 0, pieceExample, null }
			};
		}
		#endregion
		#region Test
		[TestCaseSource( nameof( parametersWithIsDefault ) )]
		public void IsDefaultReturnsTrueIfAndOnlyIfObjectWasPrepopulated( DateTime timestamp, IPlayer player, int distanceToPiece, IFieldPiece piece, bool expected )
		{
			var sut = Mock.Of<ITaskField>( field => field.Timestamp == timestamp && field.Player == player && field.DistanceToPiece == distanceToPiece && field.Piece == piece );
			Assert.AreEqual( expected, sut.IsDefault() );
		}
		[TestCaseSource( nameof( makeTaskFieldParameters ) )]
		public void MakeTaskFieldCallsCreateTaskFieldWithProperParameters( uint x, uint y, DateTime timestamp, IPlayer player, int distanceToPiece, IFieldPiece piece )
		{
			var mock = new Mock<ITaskField>();
			var sut = mock.Object;
			var result = sut.MakeTaskField( x, y, timestamp, player, distanceToPiece, piece );
			mock.Verify( field => field.CreateTaskField( x, y, timestamp, player, distanceToPiece, piece ) );
		}
		[TestCaseSource( nameof( parametersWithSetTimestampParameter ) )]
		public void SetTimestampCallsCreateTaskFieldWithProperParameters( uint x, uint y, DateTime timestamp, IPlayer player, int distanceToPiece, IFieldPiece piece, DateTime value )
		{
			var mock = new Mock<ITaskField>();
			mock.SetupGet( field => field.X ).Returns( x );
			mock.SetupGet( field => field.Y ).Returns( y );
			mock.SetupGet( field => field.Timestamp ).Returns( timestamp );
			mock.SetupGet( field => field.Player ).Returns( player );
			mock.SetupGet( field => field.DistanceToPiece ).Returns( distanceToPiece );
			mock.SetupGet( field => field.Piece ).Returns( piece );
			var sut = mock.Object;
			var result = sut.SetTimestamp( value );
			mock.Verify( field => field.CreateTaskField( x, y, value, player, distanceToPiece, piece ) );
		}
		[TestCaseSource( nameof( parametersWithSetPlayerParameter ) )]
		public void SetPlayerCallsCreateTaskFieldWithProperParameters( uint x, uint y, DateTime timestamp, IPlayer player, int distanceToPiece, IFieldPiece piece, IPlayer value )
		{
			var mock = new Mock<ITaskField>();
			mock.SetupGet( field => field.X ).Returns( x );
			mock.SetupGet( field => field.Y ).Returns( y );
			mock.SetupGet( field => field.Timestamp ).Returns( timestamp );
			mock.SetupGet( field => field.Player ).Returns( player );
			mock.SetupGet( field => field.DistanceToPiece ).Returns( distanceToPiece );
			mock.SetupGet( field => field.Piece ).Returns( piece );
			var sut = mock.Object;
			var result = sut.SetPlayer( value );
			mock.Verify( field => field.CreateTaskField( x, y, timestamp, value, distanceToPiece, piece ) );
		}
		[TestCaseSource( nameof( parametersWithSetDistanceToPieceParameter ) )]
		public void SetDistanceToPieceCallsCreateTaskFieldWithProperParameters( uint x, uint y, DateTime timestamp, IPlayer player, int distanceToPiece, IFieldPiece piece, int value )
		{
			var mock = new Mock<ITaskField>();
			mock.SetupGet( field => field.X ).Returns( x );
			mock.SetupGet( field => field.Y ).Returns( y );
			mock.SetupGet( field => field.Timestamp ).Returns( timestamp );
			mock.SetupGet( field => field.Player ).Returns( player );
			mock.SetupGet( field => field.DistanceToPiece ).Returns( distanceToPiece );
			mock.SetupGet( field => field.Piece ).Returns( piece );
			var sut = mock.Object;
			var result = sut.SetDistanceToPiece( value );
			mock.Verify( field => field.CreateTaskField( x, y, timestamp, player, value, piece ) );
		}
		[TestCaseSource( nameof( parametersWithSetPieceParameter ) )]
		public void SetPieceCallsCreateTaskFieldWithProperParameters( uint x, uint y, DateTime timestamp, IPlayer player, int distanceToPiece, IFieldPiece piece, IFieldPiece value )
		{
			var mock = new Mock<ITaskField>();
			mock.SetupGet( field => field.X ).Returns( x );
			mock.SetupGet( field => field.Y ).Returns( y );
			mock.SetupGet( field => field.Timestamp ).Returns( timestamp );
			mock.SetupGet( field => field.Player ).Returns( player );
			mock.SetupGet( field => field.DistanceToPiece ).Returns( distanceToPiece );
			mock.SetupGet( field => field.Piece ).Returns( piece );
			var sut = mock.Object;
			var result = sut.SetPiece( value );
			mock.Verify( field => field.CreateTaskField( x, y, timestamp, player, distanceToPiece, value ) );
		}
		#endregion
	}
}
