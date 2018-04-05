using Moq;
using NUnit.Framework;
using Shared.Components.Extensions;
using Shared.Components.Fields;
using Shared.Components.Players;
using Shared.Enums;
using System;

namespace SharedUnitTests.Components.Extensions
{
	[TestFixture]
	public class ExtensionIGoalFieldTest
	{
		#region Data
		private static readonly DateTime dateTimeExample;
		private static readonly IPlayer playerExample;
		private static object[] parametersWithIsDefault;
		private static object[] makeGoalFieldParameters;
		private static object[] parametersWithSetTimestampParameter;
		private static object[] parametersWithSetPlayerParameter;
		private static object[] parametersWithSetTypeParameter;
		static ExtensionIGoalFieldTest()
		{
			dateTimeExample = DateTime.Now;
			playerExample = Mock.Of<IPlayer>();
			parametersWithIsDefault = new object[]
			{
				new object[] { default( DateTime ), null, GoalFieldType.Unknown, true },
				new object[] { dateTimeExample, null, GoalFieldType.Unknown, false },
				new object[] { default( DateTime ), playerExample, GoalFieldType.Unknown, false },
				new object[] { default( DateTime ), null, GoalFieldType.Goal, false }
			};
			makeGoalFieldParameters = new object[]
			{
				new object[] { 0u, 2u, TeamColour.Red, default( DateTime ), null, GoalFieldType.Unknown },
				new object[] { 1u, 3u, TeamColour.Blue, dateTimeExample, playerExample, GoalFieldType.Goal }
			};
			parametersWithSetTimestampParameter = new object[]
			{
				new object[] { 0u, 2u, TeamColour.Red, default( DateTime ), null, GoalFieldType.Unknown, dateTimeExample },
				new object[] { 1u, 3u, TeamColour.Blue, dateTimeExample, playerExample, GoalFieldType.Goal, default( DateTime ) }
			};
			parametersWithSetPlayerParameter = new object[]
			{
				new object[] { 0u, 2u, TeamColour.Red, default( DateTime ), null, GoalFieldType.Unknown, playerExample },
				new object[] { 1u, 3u, TeamColour.Blue, dateTimeExample, playerExample, GoalFieldType.Goal, null }
			};
			parametersWithSetTypeParameter = new object[]
			{
				new object[] { 0u, 2u, TeamColour.Red, default( DateTime ), null, GoalFieldType.Unknown, GoalFieldType.NonGoal },
				new object[] { 1u, 3u, TeamColour.Blue, dateTimeExample, playerExample, GoalFieldType.Goal, GoalFieldType.Unknown }
			};
		}
		#endregion
		#region Test
		[TestCaseSource( nameof( parametersWithIsDefault ) )]
		public void IsDefaultReturnsTrueIfAndOnlyIfObjectWasPrepopulated( DateTime timestamp, IPlayer player, GoalFieldType type, bool expected )
		{
			var sut = Mock.Of<IGoalField>( field => field.Timestamp == timestamp && field.Player == player && field.Type == type );
			Assert.AreEqual( expected, sut.IsDefault() );
		}
		[TestCaseSource( nameof( makeGoalFieldParameters ) )]
		public void MakeGoalFieldCallsCreateGoalFieldWithProperParameters( uint x, uint y, TeamColour team, DateTime timestamp, IPlayer player, GoalFieldType type )
		{
			var mock = new Mock<IGoalField>();
			var sut = mock.Object;
			var result = sut.MakeGoalField( x, y, team, timestamp, player, type );
			mock.Verify( field => field.CreateGoalField( x, y, team, timestamp, player, type ) );
		}
		[TestCaseSource( nameof( parametersWithSetTimestampParameter ) )]
		public void SetTimestampCallsCreateGoalFieldWithProperParameters( uint x, uint y, TeamColour team, DateTime timestamp, IPlayer player, GoalFieldType type, DateTime value )
		{
			var mock = new Mock<IGoalField>();
			mock.SetupGet( field => field.X ).Returns( x );
			mock.SetupGet( field => field.Y ).Returns( y );
			mock.SetupGet( field => field.Timestamp ).Returns( timestamp );
			mock.SetupGet( field => field.Player ).Returns( player );
			mock.SetupGet( field => field.Type ).Returns( type );
			mock.SetupGet( field => field.Team ).Returns( team );
			var sut = mock.Object;
			var result = sut.SetTimestamp( value );
			mock.Verify( field => field.CreateGoalField( x, y, team, value, player, type ) );
		}
		[TestCaseSource( nameof( parametersWithSetPlayerParameter ) )]
		public void SetPlayerCallsCreateGoalFieldWithProperParameters( uint x, uint y, TeamColour team, DateTime timestamp, IPlayer player, GoalFieldType type, IPlayer value )
		{
			var mock = new Mock<IGoalField>();
			mock.SetupGet( field => field.X ).Returns( x );
			mock.SetupGet( field => field.Y ).Returns( y );
			mock.SetupGet( field => field.Timestamp ).Returns( timestamp );
			mock.SetupGet( field => field.Player ).Returns( player );
			mock.SetupGet( field => field.Type ).Returns( type );
			mock.SetupGet( field => field.Team ).Returns( team );
			var sut = mock.Object;
			var result = sut.SetPlayer( value );
			mock.Verify( field => field.CreateGoalField( x, y, team, timestamp, value, type ) );
		}
		[TestCaseSource( nameof( parametersWithSetTypeParameter ) )]
		public void SetTypeCallsCreateGoalFieldWithProperParameters( uint x, uint y, TeamColour team, DateTime timestamp, IPlayer player, GoalFieldType type, GoalFieldType value )
		{
			var mock = new Mock<IGoalField>();
			mock.SetupGet( field => field.X ).Returns( x );
			mock.SetupGet( field => field.Y ).Returns( y );
			mock.SetupGet( field => field.Timestamp ).Returns( timestamp );
			mock.SetupGet( field => field.Player ).Returns( player );
			mock.SetupGet( field => field.Type ).Returns( type );
			mock.SetupGet( field => field.Team ).Returns( team );
			var sut = mock.Object;
			var result = sut.SetType( value );
			mock.Verify( field => field.CreateGoalField( x, y, team, timestamp, player, value ) );
		}
		#endregion
	}
}
