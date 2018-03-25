using Moq;
using NUnit.Framework;
using Shared.Components.Extensions;
using Shared.Components.Fields;
using Shared.Components.Players;
using System;

namespace SharedUnitTests.Components.Fields
{
	[TestFixture]
	public class ExtensionIFieldTest
	{
		#region Data
		private static readonly DateTime dateTimeExample;
		private static readonly IPlayer playerExample;
		private static object[] parametersWithIsDefault;
		private static object[] makeFieldParameters;
		private static object[] parametersWithSetTimestampParameter;
		private static object[] parametersWithSetPlayerParameter;
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
				new object[] { 0u, 1u, default( DateTime ), null },
				new object[] { 2u, 3u, dateTimeExample, null },
				new object[] { 4u, 5u, default( DateTime ), playerExample },
				new object[] { 6u, 7u, dateTimeExample, playerExample }
			};
			parametersWithSetTimestampParameter = new object[]
			{
				new object[] { 0u, 1u, default( DateTime ), null, dateTimeExample },
				new object[] { 2u, 3u, dateTimeExample, null, default( DateTime ) },
				new object[] { 4u, 5u, default( DateTime ), playerExample, dateTimeExample },
				new object[] { 6u, 7u, dateTimeExample, playerExample, default( DateTime ) }
			};
			parametersWithSetPlayerParameter = new object[]
			{
				new object[] { 0u, 1u, default( DateTime ), null, playerExample },
				new object[] { 2u, 3u, dateTimeExample, null, playerExample },
				new object[] { 4u, 5u, default( DateTime ), playerExample, null },
				new object[] { 6u, 7u, dateTimeExample, playerExample, null }
			};
		}
		#endregion
		#region Test
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
		[TestCase( 0u, 1u )]
		[TestCase( 2u, 3u )]
		public void MakeFieldWithDefaultParametersCallsCreateFieldWithProperParameters( uint x, uint y )
		{
			var mock = new Mock<IField>();
			var sut = mock.Object;
			var result = sut.MakeField( x, y );
			mock.Verify( field => field.CreateField( x, y, default( DateTime ), null ) );
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
		[TestCaseSource( nameof( makeFieldParameters ) )]
		public void SetTimestampWithDefaultParameterCallsCreateFieldWithProperParameters( uint x, uint y, DateTime timestamp, IPlayer player )
		{
			var mock = new Mock<IField>();
			mock.SetupGet( field => field.X ).Returns( x );
			mock.SetupGet( field => field.Y ).Returns( y );
			mock.SetupGet( field => field.Timestamp ).Returns( timestamp );
			mock.SetupGet( field => field.Player ).Returns( player );
			var sut = mock.Object;
			var result = sut.SetTimestamp();
			mock.Verify( field => field.CreateField( x, y, default( DateTime ), player ) );
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
		[TestCaseSource( nameof( makeFieldParameters ) )]
		public void SetPlayerWithDefaultParameterCallsCreateFieldWithProperParameters( uint x, uint y, DateTime timestamp, IPlayer player )
		{
			var mock = new Mock<IField>();
			mock.SetupGet( field => field.X ).Returns( x );
			mock.SetupGet( field => field.Y ).Returns( y );
			mock.SetupGet( field => field.Timestamp ).Returns( timestamp );
			mock.SetupGet( field => field.Player ).Returns( player );
			var sut = mock.Object;
			var result = sut.SetPlayer();
			mock.Verify( field => field.CreateField( x, y, timestamp, null ) );
		}
		#endregion
	}
}
