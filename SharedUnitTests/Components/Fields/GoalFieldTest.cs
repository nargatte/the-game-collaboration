using System;
using Moq;
using NUnit.Framework;
using Shared.Components.Fields;
using Shared.Components.Players;
using Shared.Enums;

namespace SharedUnitTests.Components.Fields
{
	[TestFixture]
	public class GoalFieldTest
	{
		#region Data
		private static readonly DateTime dateTimeExample;
		private static readonly IPlayer playerExample;
		private static readonly object[] constructorParameters;
		private static readonly object[] parametersWithCreateFieldParameters;
		private static readonly object[] parametersWithCreateGoalFieldParameters;
		static GoalFieldTest()
		{
			dateTimeExample = DateTime.Now;
			playerExample = new Mock<IPlayer>().Object;
			constructorParameters = new object[]
			{
				new object[] { 0u, 2u, TeamColour.Blue, default( DateTime ), null, GoalFieldType.Unknown },
				new object[] { 1u, 3u, TeamColour.Red, dateTimeExample, playerExample, GoalFieldType.Goal }
			};
			parametersWithCreateFieldParameters = new object[]
			{
				new object[] { 0u, 2u, TeamColour.Blue, 4u, 6u, default( DateTime ), null },
				new object[] { 1u, 3u, TeamColour.Red, 5u, 7u, dateTimeExample, playerExample }
			};
			parametersWithCreateGoalFieldParameters = new object[]
			{
				new object[] { 0u, 2u, TeamColour.Blue, 4u, 6u, TeamColour.Red, default( DateTime ), null, GoalFieldType.Unknown },
				new object[] { 1u, 3u, TeamColour.Red, 5u, 7u, TeamColour.Blue, dateTimeExample, playerExample, GoalFieldType.Goal }
			};
		}
		#endregion
		#region Test
		[TestCaseSource( nameof( constructorParameters ) )]
		public void ConstructorFillsAllProperties( uint x, uint y, TeamColour team, DateTime timestamp, IPlayer player, GoalFieldType type )
		{
			var sut = new GoalField( x, y, team, timestamp, player, type );
			Assert.Multiple( () =>
			{
				Assert.AreEqual( x, sut.X );
				Assert.AreEqual( y, sut.Y );
				Assert.AreEqual( timestamp, sut.Timestamp );
				Assert.AreSame( player, sut.Player );
				Assert.AreEqual( type, sut.Type );
				Assert.AreEqual( team, sut.Team );
			} );
		}
		[TestCaseSource( nameof( parametersWithCreateFieldParameters ) )]
		public void CreateFieldThrowsNotSupportedException( uint x, uint y, TeamColour team, uint ax, uint ay, DateTime atimestamp, IPlayer aplayer )
		{
			var sut = new GoalField( x, y, team );
			Assert.That( () => sut.CreateField( ax, ay, atimestamp, aplayer ), Throws.InstanceOf<NotSupportedException>() );
		}
		[TestCaseSource( nameof( parametersWithCreateGoalFieldParameters ) )]
		public void CreateGoalFieldReturnsObjectWithFilledProperties( uint x, uint y, TeamColour team, uint ax, uint ay, TeamColour ateam, DateTime atimestamp, IPlayer aplayer, GoalFieldType atype )
		{
			var sut = new GoalField( x, y, team );
			var result = sut.CreateGoalField( ax, ay, ateam, atimestamp, aplayer, atype );
			Assert.Multiple( () =>
			{
				Assert.AreEqual( ax, result.X );
				Assert.AreEqual( ay, result.Y );
				Assert.AreEqual( atimestamp, result.Timestamp );
				Assert.AreSame( aplayer, result.Player );
				Assert.AreEqual( atype, result.Type );
				Assert.AreEqual( ateam, result.Team );
			} );
		}
		#endregion
	}
}

