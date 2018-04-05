using Moq;
using NUnit.Framework;
using Shared.Components.Boards;
using Shared.Components.Factories;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.Enums;
using System;

namespace SharedUnitTests.Components.Boards
{
	[TestFixture]
	public class BoardTest
	{
		#region Data
		private static readonly IBoardPrototypeFactory exampleBoardPrototypeFactory;
		private static readonly object[] constructorParameters;
		static BoardTest()
		{
			var mock = new Mock<IBoardPrototypeFactory>();
			mock.Setup( factory => factory.GoalField.CreateGoalField( It.IsAny<uint>(), It.IsAny<uint>(), It.IsAny<TeamColour>(), It.IsAny<DateTime>(), It.IsAny<IPlayer>(), It.IsAny<GoalFieldType>() ) ).Returns( ( uint x, uint y, TeamColour team, DateTime timestamp, IPlayer player, GoalFieldType type ) => Mock.Of<IGoalField>( field => field.X == x && field.Y == y && field.Team == team && field.Timestamp == timestamp && field.Player == player && field.Type == type ) );
			mock.Setup( factory => factory.TaskField.CreateTaskField( It.IsAny<uint>(), It.IsAny<uint>(), It.IsAny<DateTime>(), It.IsAny<IPlayer>(), It.IsAny<int>(), It.IsAny<IFieldPiece>() ) ).Returns( ( uint x, uint y, DateTime timestamp, IPlayer player, int distanceToPiece, IFieldPiece piece ) => Mock.Of<ITaskField>( field => field.X == x && field.Y == y && field.Timestamp == timestamp && field.Player == player && field.DistanceToPiece == distanceToPiece && field.Piece == piece ) );
			exampleBoardPrototypeFactory = mock.Object;
			constructorParameters = new object[]
			{
				new object[] { 1u, 3u, 5u, exampleBoardPrototypeFactory },
				new object[] { 2u, 4u, 6u, exampleBoardPrototypeFactory }
			};
		}
		#endregion
		#region Test
		[TestCaseSource( nameof( constructorParameters ) )]
		public void ConstructorFillsAllProperties( uint width, uint tasksHeight, uint goalsHeight, IBoardPrototypeFactory factory )
		{
			var sut = new Board( width, tasksHeight, goalsHeight, factory );
			uint height = tasksHeight + 2u * goalsHeight;
			Assert.Multiple( () =>
			{
				Assert.AreEqual( width, sut.Width );
				Assert.AreEqual( tasksHeight, sut.TasksHeight );
				Assert.AreEqual( goalsHeight, sut.GoalsHeight );
				Assert.AreSame( factory, sut.Factory );
				Assert.AreEqual( height, sut.Height );
			} );
		}
		#endregion
	}
}