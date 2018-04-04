using Moq;
using NUnit.Framework;
using Shared.Components.Extensions;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Enums;
using System;

namespace SharedUnitTests.Components.Extensions
{
	[TestFixture]
	public class ExtensionIFieldPieceTest
	{
		#region Data
		private static readonly DateTime dateTimeExample;
		private static readonly ITaskField taskFieldExample;
		private static readonly object[] parametersWithIsDefault;
		private static readonly object[] makeFieldPieceParameters;
		private static readonly object[] parametersWithSetTypeParameter;
		private static readonly object[] parametersWithSetTimestampParameter;
		private static readonly object[] parametersWithSetFieldParameter;
		static ExtensionIFieldPieceTest()
		{
			dateTimeExample = DateTime.Now;
			taskFieldExample = Mock.Of<ITaskField>();
			parametersWithIsDefault = new object[]
			{
				new object[] { PieceType.Unknown, default( DateTime ), null, true },
				new object[] { PieceType.Sham, default( DateTime ), null, false },
				new object[] { PieceType.Unknown, dateTimeExample, null, false },
				new object[] { PieceType.Unknown, default( DateTime ), taskFieldExample, false }
			};
			makeFieldPieceParameters = new object[]
			{
				new object[] { 0ul, PieceType.Unknown, default( DateTime ), null },
				new object[] { 1ul, PieceType.Sham, dateTimeExample, taskFieldExample }
			};
			parametersWithSetTypeParameter = new object[]
			{
				new object[] { 0ul, PieceType.Unknown, default( DateTime ), null, PieceType.Normal },
				new object[] { 1ul, PieceType.Sham, dateTimeExample, taskFieldExample, PieceType.Unknown }
			};
			parametersWithSetTimestampParameter = new object[]
			{
				new object[] { 0ul, PieceType.Unknown, default( DateTime ), null, dateTimeExample },
				new object[] { 1ul, PieceType.Sham, dateTimeExample, taskFieldExample, default( DateTime ) }
			};
			parametersWithSetFieldParameter = new object[]
			{
				new object[] { 0ul, PieceType.Unknown, default( DateTime ), null, taskFieldExample },
				new object[] { 1ul, PieceType.Sham, dateTimeExample, taskFieldExample, null }
			};
		}
		#endregion
		#region Test
		[TestCaseSource( nameof( parametersWithIsDefault ) )]
		public void IsDefaultReturnsTrueIfAndOnlyIfObjectWasPrepopulated( PieceType type, DateTime timestamp, ITaskField field, bool expected )
		{
			var sut = Mock.Of<IFieldPiece>( piece => piece.Type == type && piece.Timestamp == timestamp && piece.Field == field );
			Assert.AreEqual( expected, sut.IsDefault() );
		}
		[TestCaseSource( nameof( makeFieldPieceParameters ) )]
		public void MakeFieldPieceCallsCreateFieldPieceWithProperParameters( ulong id, PieceType type, DateTime timestamp, ITaskField field )
		{
			var mock = new Mock<IFieldPiece>();
			var sut = mock.Object;
			var result = sut.MakeFieldPiece( id, type, timestamp, field );
			mock.Verify( piece => piece.CreateFieldPiece( id, type, timestamp, field ) );
		}
		[TestCaseSource( nameof( parametersWithSetTypeParameter ) )]
		public void SetPlayerCallsCreateFieldPieceWithProperParameters( ulong id, PieceType type, DateTime timestamp, ITaskField field, PieceType value )
		{
			var mock = new Mock<IFieldPiece>();
			mock.SetupGet( piece => piece.Id ).Returns( id );
			mock.SetupGet( piece => piece.Type ).Returns( type );
			mock.SetupGet( piece => piece.Timestamp ).Returns( timestamp );
			mock.SetupGet( piece => piece.Field ).Returns( field );
			var sut = mock.Object;
			var result = sut.SetType( value );
			mock.Verify( piece => piece.CreateFieldPiece( id, value, timestamp, field ) );
		}
		[TestCaseSource( nameof( parametersWithSetTimestampParameter ) )]
		public void SetTimestampCallsCreateFieldPieceWithProperParameters( ulong id, PieceType type, DateTime timestamp, ITaskField field, DateTime value )
		{
			var mock = new Mock<IFieldPiece>();
			mock.SetupGet( piece => piece.Id ).Returns( id );
			mock.SetupGet( piece => piece.Type ).Returns( type );
			mock.SetupGet( piece => piece.Timestamp ).Returns( timestamp );
			mock.SetupGet( piece => piece.Field ).Returns( field );
			var sut = mock.Object;
			var result = sut.SetTimestamp( value );
			mock.Verify( piece => piece.CreateFieldPiece( id, type, value, field ) );
		}
		[TestCaseSource( nameof( parametersWithSetFieldParameter ) )]
		public void SetTimestampCallsCreateFieldPieceWithProperParameters( ulong id, PieceType type, DateTime timestamp, ITaskField field, ITaskField value )
		{
			var mock = new Mock<IFieldPiece>();
			mock.SetupGet( piece => piece.Id ).Returns( id );
			mock.SetupGet( piece => piece.Type ).Returns( type );
			mock.SetupGet( piece => piece.Timestamp ).Returns( timestamp );
			mock.SetupGet( piece => piece.Field ).Returns( field );
			var sut = mock.Object;
			var result = sut.SetField( value );
			mock.Verify( piece => piece.CreateFieldPiece( id, type, timestamp, value ) );
		}
		#endregion
	}
}
