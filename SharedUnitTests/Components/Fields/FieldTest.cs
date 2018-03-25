using System;
using Moq;
using NUnit.Framework;
using Shared.Components.Fields;
using Shared.Components.Players;

namespace SharedUnitTests.Components.Fields
{
	[TestFixture]
	public class FieldTest
	{
		#region Data
		private class FieldTestType : Field
		{
			#region Field
			public override IField CreateField( uint x, uint y, DateTime timestamp, IPlayer player ) => throw new NotImplementedException();
			#endregion
			#region FieldTestType
			public FieldTestType( uint x, uint y, DateTime timestamp = default( DateTime ), IPlayer player = null ) : base( x, y, timestamp, player )
			{
			}
			#endregion
		}
		private static readonly DateTime dateTimeExample;
		private static readonly IPlayer playerExample;
		private static object[] constructorParameters;
		static FieldTest()
		{
			dateTimeExample = DateTime.Now;
			playerExample = new Mock<IPlayer>().Object;
			constructorParameters = new object[]
			{
				new object[] { 0u, 1u, default( DateTime ), null },
				new object[] { 2u, 3u, dateTimeExample, null },
				new object[] { 4u, 5u, default( DateTime ), playerExample },
				new object[] { 6u, 7u, dateTimeExample, playerExample }
			};
		}
		#endregion
		#region Test
		[TestCaseSource( nameof( constructorParameters ) )]
		public void ConstructorFillsAllProperties( uint x, uint y, DateTime timestamp, IPlayer player )
		{
			var sut = new FieldTestType( x, y, timestamp, player );
			Assert.AreEqual( x, sut.X );
			Assert.AreEqual( y, sut.Y );
			Assert.AreEqual( timestamp, sut.Timestamp );
			Assert.AreSame( player, sut.Player );
		}
		[TestCase( 0u, 1u )]
		[TestCase( 2u, 3u )]
		public void ConstructorWithDefaultParametersFillsAllProperties( uint x, uint y )
		{
			var sut = new FieldTestType( x, y );
			Assert.AreEqual( x, sut.X );
			Assert.AreEqual( y, sut.Y );
			Assert.AreEqual( default( DateTime ), sut.Timestamp );
			Assert.IsNull( sut.Player );
		}
		#endregion
	}
}
