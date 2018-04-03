using Moq;
using NUnit.Framework;
using Shared.Components.Extensions;
using Shared.Components.Fields;
using Shared.Components.Players;
using System;

namespace SharedUnitTests.Components.Extensions
{
	[TestFixture]
	public class ExtensionIFieldTest
	{
		#region Data
		private static readonly DateTime dateTimeExample;
		private static readonly IPlayer playerExample;
		private static readonly object[] parametersWithIsDefault;
		private static readonly object[] makeFieldParameters;
		private static readonly object[] parametersWithSetTimestampParameter;
		private static readonly object[] parametersWithSetPlayerParameter;
		static ExtensionIFieldTest()
		{
			dateTimeExample = DateTime.Now;
			playerExample = new Mock<IPlayer>().Object;
			parametersWithIsDefault = new object[]
			{
				new object[] { default( DateTime ), null, true },
				new object[] { dateTimeExample, null, false },
				new object[] { default( DateTime ), playerExample, false },
				new object[] { dateTimeExample, playerExample, false }
			};
			makeFieldParameters = new object[]
			{
				new object[] { 0u, 2u, default( DateTime ), null },
				new object[] { 1u, 3u, dateTimeExample, playerExample }
			};
			parametersWithSetTimestampParameter = new object[]
			{
				new object[] { 0u, 2u, default( DateTime ), null, dateTimeExample },
				new object[] { 1u, 3u, dateTimeExample, playerExample, default( DateTime ) }
			};
			parametersWithSetPlayerParameter = new object[]
			{
				new object[] { 0u, 2u, default( DateTime ), null, playerExample },
				new object[] { 1u, 3u, dateTimeExample, playerExample, null }
			};
		}
		#endregion
		#region Test
		[TestCaseSource( nameof( parametersWithIsDefault ) )]
		public void IsDefaultReturnsTrueIfAndOnlyIfObjectWasPrepopulated( DateTime timestamp, IPlayer player, bool expected )
		{
			var sut = Mock.Of<IField>( field => field.Timestamp == timestamp && field.Player == player );
			Assert.AreEqual( expected, sut.IsDefault() );
		}
		[TestCaseSource( nameof( makeFieldParameters ) )]
		public void MakeFieldCallsCreateFieldWithProperParameters( uint x, uint y, DateTime timestamp, IPlayer player )
		{
			var mock = new Mock<IField>();
			var sut = mock.Object;
			var result = sut.MakeField( x, y, timestamp, player );
			mock.Verify( field => field.CreateField( x, y, timestamp, player ) );
		}
		[TestCaseSource( nameof( parametersWithSetTimestampParameter ) )]
		public void SetTimestampCallsCreateFieldWithProperParameters( uint x, uint y, DateTime timestamp, IPlayer player, DateTime value )
		{
			var mock = new Mock<IField>();
			mock.SetupGet( field => field.X ).Returns( x );
			mock.SetupGet( field => field.Y ).Returns( y );
			mock.SetupGet( field => field.Timestamp ).Returns( timestamp );
			mock.SetupGet( field => field.Player ).Returns( player );
			var sut = mock.Object;
			var result = sut.SetTimestamp( value );
			mock.Verify( field => field.CreateField( x, y, value, player ) );
		}
		[TestCaseSource( nameof( parametersWithSetPlayerParameter ) )]
		public void SetPlayerCallsCreateFieldWithProperParameters( uint x, uint y, DateTime timestamp, IPlayer player, IPlayer value )
		{
			var mock = new Mock<IField>();
			mock.SetupGet( field => field.X ).Returns( x );
			mock.SetupGet( field => field.Y ).Returns( y );
			mock.SetupGet( field => field.Timestamp ).Returns( timestamp );
			mock.SetupGet( field => field.Player ).Returns( player );
			var sut = mock.Object;
			var result = sut.SetPlayer( value );
			mock.Verify( field => field.CreateField( x, y, timestamp, value ) );
		}
		#endregion
	}
}
