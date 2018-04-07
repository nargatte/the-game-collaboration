using Moq;
using NUnit.Framework;
using Shared.Components.Fields;
using Shared.Components.Players;
using System;

namespace SharedUnitTests.Components.Fields
{
	[TestFixture]
	public class FieldTest
	{
		#region Data
		private class FieldTestType : Field
		{
			#region Field
			public override IField CloneField() => throw new NotImplementedException();
			#endregion
			#region FieldTestType
			public FieldTestType( uint x, uint y, DateTime timestamp = default, IPlayer player = null ) : base( x, y, timestamp, player )
			{
			}
			#endregion
		}
		private static readonly DateTime dateTimeExample;
		private static readonly IPlayer playerExample;
		private static readonly IPlayer playerExample2;
		private static readonly object[] constructorParameters;
		private static readonly object[] parametersWithSetParameters;
		private static readonly object[] parametersWithSetPlayerParameter;
		static FieldTest()
		{
			dateTimeExample = DateTime.Now;
			playerExample = Mock.Of<IPlayer>();
			var mockPlayer = new Mock<IPlayer>();
			mockPlayer.SetupProperty( player => player.Field );
			playerExample2 = mockPlayer.Object;
			constructorParameters = new object[]
			{
				new object[] { 0u, 2u, default( DateTime ), null },
				new object[] { 1u, 3u, dateTimeExample, playerExample }
			};
			parametersWithSetParameters = new object[]
			{
				new object[] { 0u, 2u, default( DateTime ), null, 4u, 6u, dateTimeExample },
				new object[] { 1u, 3u, dateTimeExample, playerExample, 5u, 7u, default( DateTime ) }
			};
			parametersWithSetPlayerParameter = new object[]
			{
				new object[] { 0u, 2u, default( DateTime ), null, null },
				new object[] { 1u, 3u, dateTimeExample, playerExample, null },
				new object[] { 0u, 2u, default( DateTime ), null, playerExample2 },
				new object[] { 1u, 3u, dateTimeExample, playerExample, playerExample2 }
			};
		}
		#endregion
		#region Test
		[TestCaseSource( nameof( constructorParameters ) )]
		public void ConstructorFillsAllProperties( uint x, uint y, DateTime timestamp, IPlayer player )
		{
			var sut = new FieldTestType( x, y, timestamp, player );
			Assert.Multiple( () =>
			{
				Assert.AreEqual( x, sut.X );
				Assert.AreEqual( y, sut.Y );
				Assert.AreEqual( timestamp, sut.Timestamp );
				Assert.AreSame( player, sut.Player );
			} );
		}
		[TestCaseSource( nameof( parametersWithSetParameters ) )]
		public void SetForNonNavigationalPropertyTracksValue( uint x, uint y, DateTime timestamp, IPlayer player, uint aX, uint aY, DateTime aTimestamp )
		{
			var sut = new FieldTestType( x, y, timestamp, player )
			{
				X = aX,
				Y = aY,
				Timestamp = aTimestamp
			};
			Assert.Multiple( () =>
			{
				Assert.AreEqual( aX, sut.X );
				Assert.AreEqual( aY, sut.Y );
				Assert.AreEqual( aTimestamp, sut.Timestamp );
			} );
		}
		[TestCaseSource( nameof( parametersWithSetPlayerParameter ) )]
		public void SetPlayerTracksValueAndLinksObjects( uint x, uint y, DateTime timestamp, IPlayer player, IPlayer value )
		{
			var sut = new FieldTestType( x, y, timestamp, player )
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
