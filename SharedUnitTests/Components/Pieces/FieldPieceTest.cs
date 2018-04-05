using Moq;
using NUnit.Framework;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Enums;
using System;

namespace SharedUnitTests.Components.Pieces
{
	[TestFixture]
	public class FieldPieceTest
	{
		#region Data
		private static readonly DateTime dateTimeExample;
		private static readonly ITaskField taskFieldExample;
		private static readonly object[] constructorParameters;
		private static readonly object[] parametersWithCreatePieceParameters;
		private static readonly object[] parametersWithCreateFieldPieceParameters;

		static FieldPieceTest()
		{
			dateTimeExample = DateTime.Now;
			taskFieldExample = Mock.Of<ITaskField>();
			constructorParameters = new object[]
			{
				new object[] { 0ul, PieceType.Unknown, default( DateTime ), null },
				new object[] { 1ul, PieceType.Sham, dateTimeExample, taskFieldExample }
			};
			parametersWithCreatePieceParameters = new object[]
			{
				new object[] { 0ul, 2ul, PieceType.Unknown, default( DateTime ) },
				new object[] { 1ul, 3ul, PieceType.Sham, dateTimeExample }
			};
			parametersWithCreateFieldPieceParameters = new object[]
			{
				new object[] { 0ul, 2ul, PieceType.Unknown, default( DateTime ), null },
				new object[] { 1ul, 3ul, PieceType.Sham, dateTimeExample, taskFieldExample }
			};
		}
		#endregion
		#region Test
		[TestCaseSource( nameof( constructorParameters ) )]
		public void ConstructorFillsAllProperties( ulong id, PieceType type, DateTime timestamp, ITaskField field )
		{
			var sut = new FieldPiece( id, type, timestamp, field );
			Assert.Multiple( () =>
			{
				Assert.AreEqual( id, sut.Id );
				Assert.AreEqual( type, sut.Type );
				Assert.AreEqual( timestamp, sut.Timestamp );
				Assert.AreSame( field, sut.Field );
			} );
		}
		[TestCaseSource( nameof( parametersWithCreatePieceParameters ) )]
		public void CreatePieceReturnsObjectWithFilledProperties( ulong id, ulong aId, PieceType aType, DateTime aTimestamp )
		{
			var sut = new FieldPiece( id );
			var result = sut.CreatePiece( aId, aType, aTimestamp );
			Assert.Multiple( () =>
			{
				Assert.AreEqual( aId, result.Id );
				Assert.AreEqual( aType, result.Type );
				Assert.AreEqual( aTimestamp, result.Timestamp );
			} );
		}
		[TestCaseSource( nameof( parametersWithCreateFieldPieceParameters ) )]
		public void CreateFieldPieceReturnsObjectWithFilledProperties( ulong id, ulong aId, PieceType aType, DateTime aTimestamp, ITaskField aField )
		{
			var sut = new FieldPiece( id );
			var result = sut.CreateFieldPiece( aId, aType, aTimestamp, aField );
			Assert.Multiple( () =>
			{
				Assert.AreEqual( aId, result.Id );
				Assert.AreEqual( aType, result.Type );
				Assert.AreEqual( aTimestamp, result.Timestamp );
				Assert.AreEqual( aField, result.Field );
			} );
		}
		#endregion
	}
}
