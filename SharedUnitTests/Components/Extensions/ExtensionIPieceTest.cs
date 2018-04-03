using Moq;
using NUnit.Framework;
using Shared.Components.Extensions;
using Shared.Components.Pieces;
using Shared.Enums;
using System;

namespace SharedUnitTests.Components.Extensions
{
	[TestFixture]
	public class ExtensionIPieceTest
	{
		#region Data
		private static readonly DateTime dateTimeExample;
		private static readonly object[] parametersWithIsDefault;
		private static readonly object[] makePieceParameters;
		private static readonly object[] parametersWithSetTypeParameter;
		private static readonly object[] parametersWithSetTimestampParameter;
		static ExtensionIPieceTest()
		{
			dateTimeExample = DateTime.Now;
			parametersWithIsDefault = new object[]
			{
				new object[] { PieceType.Unknown, default( DateTime ), true },
				new object[] { PieceType.Sham, default( DateTime ), false },
				new object[] { PieceType.Unknown, dateTimeExample, false },
				new object[] { PieceType.Sham, dateTimeExample, false }
			};
			makePieceParameters = new object[]
			{
				new object[] { 0ul, PieceType.Unknown, default( DateTime ) },
				new object[] { 1ul, PieceType.Sham, dateTimeExample }
			};
			parametersWithSetTypeParameter = new object[]
			{
				new object[] { 0ul, PieceType.Unknown, default( DateTime ), PieceType.Normal },
				new object[] { 1ul, PieceType.Sham, dateTimeExample, PieceType.Unknown }
			};
			parametersWithSetTimestampParameter = new object[]
			{
				new object[] { 0ul, PieceType.Unknown, default( DateTime ), dateTimeExample },
				new object[] { 1ul, PieceType.Sham, dateTimeExample, default( DateTime ) }
			};
		}
		#endregion
		#region Test
		[TestCaseSource( nameof( parametersWithIsDefault ) )]
		public void IsDefaultReturnsTrueIfAndOnlyIfObjectWasPrepopulated( PieceType type, DateTime timestamp, bool expected )
		{
			var sut = Mock.Of<IPiece>( piece => piece.Type == type && piece.Timestamp == timestamp );
			Assert.AreEqual( expected, sut.IsDefault() );
		}
		[TestCaseSource( nameof( makePieceParameters ) )]
		public void MakePieceCallsCreatePieceWithProperParameters( ulong id, PieceType type, DateTime timestamp )
		{
			var mock = new Mock<IPiece>();
			var sut = mock.Object;
			var result = sut.MakePiece( id, type, timestamp );
			mock.Verify( piece => piece.CreatePiece( id, type, timestamp ) );
		}
		[TestCaseSource( nameof( parametersWithSetTypeParameter ) )]
		public void SetPlayerCallsCreateFieldWithProperParameters( ulong id, PieceType type, DateTime timestamp, PieceType value )
		{
			var mock = new Mock<IPiece>();
			mock.SetupGet( piece => piece.Id ).Returns( id );
			mock.SetupGet( piece => piece.Type ).Returns( type );
			mock.SetupGet( piece => piece.Timestamp ).Returns( timestamp );
			var sut = mock.Object;
			var result = sut.SetType( value );
			mock.Verify( field => field.CreatePiece( id, value, timestamp ) );
		}
		[TestCaseSource( nameof( parametersWithSetTimestampParameter ) )]
		public void SetTimestampCallsCreateFieldWithProperParameters( ulong id, PieceType type, DateTime timestamp, DateTime value )
		{
			var mock = new Mock<IPiece>();
			mock.SetupGet( piece => piece.Id ).Returns( id );
			mock.SetupGet( piece => piece.Type ).Returns( type );
			mock.SetupGet( piece => piece.Timestamp ).Returns( timestamp );
			var sut = mock.Object;
			var result = sut.SetTimestamp( value );
			mock.Verify( field => field.CreatePiece( id, type, value ) );
		}
		#endregion
	}
}
