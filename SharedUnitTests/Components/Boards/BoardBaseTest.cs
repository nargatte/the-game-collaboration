//using Moq;
//using NUnit.Framework;
//using Shared.Components.Boards;
//using Shared.Components.Factories;
//using Shared.Components.Fields;
//using Shared.Components.Pieces;
//using Shared.Components.Players;
//using System;
//using System.Collections.Generic;

//namespace SharedUnitTests.Components.Boards
//{
//	[TestFixture]
//	public class BoardBaseTest
//	{
//		#region Data
//		private class BoardBaseTestType : BoardBase
//		{
//			#region BoardBase
//			public override IEnumerable<IField> Fields => throw new NotImplementedException();
//			public override IEnumerable<IPiece> Pieces => throw new NotImplementedException();
//			public override IEnumerable<IPlayer> Players => throw new NotImplementedException();
//			public override IField GetField( uint x, uint y ) => throw new NotImplementedException();
//			public override IPiece GetPiece( ulong id ) => throw new NotImplementedException();
//			public override IPlayer GetPlayer( ulong id ) => throw new NotImplementedException();
//			public override void SetField( IField value ) => throw new NotImplementedException();
//			public override void SetPiece( IPiece value ) => throw new NotImplementedException();
//			public override void SetPlayer( IPlayer value ) => throw new NotImplementedException();
//			#endregion
//			#region FieldTestType
//			public BoardBaseTestType( uint width, uint tasksHeight, uint goalsHeight, IBoardPrototypeFactory factory ) : base( width, tasksHeight, goalsHeight, factory )
//			{
//			}
//			public void OnFieldChanged() => OnFieldChanged( default, default );
//			public void OnPieceChanged() => OnPieceChanged( default );
//			public void OnPlayerChanged() => OnPlayerChanged( default );
//			#endregion
//		}
//		private static readonly IBoardPrototypeFactory exampleBoardPrototypeFactory;
//		private static readonly object[] constructorParameters;
//		private static readonly object[] constructorParametersWithWidthError;
//		private static readonly object[] constructorParametersWithTasksHeightError;
//		private static readonly object[] constructorParametersWithGoalsHeightError;
//		static BoardBaseTest()
//		{
//			exampleBoardPrototypeFactory = Mock.Of<IBoardPrototypeFactory>();
//			constructorParameters = new object[]
//			{
//				new object[] { 1u, 3u, 5u, exampleBoardPrototypeFactory },
//				new object[] { 2u, 4u, 6u, exampleBoardPrototypeFactory }
//			};
//			constructorParametersWithWidthError = new object[]
//			{
//				new object[] { 0u, 1u, 2u, exampleBoardPrototypeFactory },
//			};
//			constructorParametersWithTasksHeightError = new object[]
//			{
//				new object[] { 1u, 0u, 2u, exampleBoardPrototypeFactory },
//			};
//			constructorParametersWithGoalsHeightError = new object[]
//			{
//				new object[] { 1u, 2u, 0u, exampleBoardPrototypeFactory },
//			};
//		}
//		#endregion
//		#region Test
//		[TestCaseSource( nameof( constructorParametersWithWidthError ) )]
//		public void ConstructorThrowsArgumentOutOfRangeExceptionWhenWidthIs0( uint width, uint tasksHeight, uint goalsHeight, IBoardPrototypeFactory factory ) => Assert.That( () =>
//		{
//			var sut = new BoardBaseTestType( width, tasksHeight, goalsHeight, factory );
//		}, Throws.InstanceOf<ArgumentOutOfRangeException>() );
//		[TestCaseSource( nameof( constructorParametersWithTasksHeightError ) )]
//		public void ConstructorThrowsArgumentOutOfRangeExceptionWhenTasksHeightIs0( uint width, uint tasksHeight, uint goalsHeight, IBoardPrototypeFactory factory ) => Assert.That( () =>
//		{
//			var sut = new BoardBaseTestType( width, tasksHeight, goalsHeight, factory );
//		}, Throws.InstanceOf<ArgumentOutOfRangeException>() );
//		[TestCaseSource( nameof( constructorParametersWithGoalsHeightError ) )]
//		public void ConstructorThrowsArgumentOutOfRangeExceptionWhenGoalsHeightIs0( uint width, uint tasksHeight, uint goalsHeight, IBoardPrototypeFactory factory ) => Assert.That( () =>
//		{
//			var sut = new BoardBaseTestType( width, tasksHeight, goalsHeight, factory );
//		}, Throws.InstanceOf<ArgumentOutOfRangeException>() );
//		[TestCase( 1u, 2u, 3u, null )]
//		public void ConstructorThrowsArgumentNullExceptionWhenFactoryIsNull( uint width, uint tasksHeight, uint goalsHeight, IBoardPrototypeFactory factory ) => Assert.That( () =>
//		{
//			var sut = new BoardBaseTestType( width, tasksHeight, goalsHeight, factory );
//		}, Throws.ArgumentNullException );
//		[TestCaseSource( nameof( constructorParameters ) )]
//		public void ConstructorFillsAllProperties( uint width, uint tasksHeight, uint goalsHeight, IBoardPrototypeFactory factory )
//		{
//			var sut = new BoardBaseTestType( width, tasksHeight, goalsHeight, factory );
//			uint height = tasksHeight + 2u * goalsHeight;
//			Assert.Multiple( () =>
//			{
//				Assert.AreEqual( width, sut.Width );
//				Assert.AreEqual( tasksHeight, sut.TasksHeight );
//				Assert.AreEqual( goalsHeight, sut.GoalsHeight );
//				Assert.AreSame( factory, sut.Factory );
//				Assert.AreEqual( height, sut.Height );
//			} );
//		}
//		[TestCaseSource( nameof( constructorParameters ) )]
//		public void EventsAreNotNullByDefault( uint width, uint tasksHeight, uint goalsHeight, IBoardPrototypeFactory factory )
//		{
//			var sut = new BoardBaseTestType( width, tasksHeight, goalsHeight, factory );
//			sut.OnFieldChanged();
//			sut.OnPieceChanged();
//			sut.OnPlayerChanged();
//		}
//		#endregion
//	}
//}