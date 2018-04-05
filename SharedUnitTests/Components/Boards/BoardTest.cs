﻿using Moq;
using NUnit.Framework;
using Shared.Components.Boards;
using Shared.Components.Factories;

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
			//mock.Se
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