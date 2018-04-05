//using Moq;
using NUnit.Framework;
using Shared.Components.Boards;
using Shared.Components.Factories;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
//using Shared.Enums;
using System;
using System.Collections.Generic;

namespace SharedUnitTests.Components.Boards
{
	[TestFixture]
	public class BoardBaseTest
	{
		#region Data
		private class BoardBaseTestType : BoardBase
		{
			#region BoardBase
			public override IEnumerable<IField> Fields => throw new NotImplementedException();
			public override IEnumerable<IPiece> Pieces => throw new NotImplementedException();
			public override IEnumerable<IPlayer> Players => throw new NotImplementedException();
			public override IField GetField( uint x, uint y ) => throw new NotImplementedException();
			public override IPiece GetPiece( ulong id ) => throw new NotImplementedException();
			public override IPlayer GetPlayer( ulong id ) => throw new NotImplementedException();
			public override void SetField( IField value ) => throw new NotImplementedException();
			public override void SetPiece( IPiece value ) => throw new NotImplementedException();
			public override void SetPlayer( IPlayer value ) => throw new NotImplementedException();
			#endregion
			#region FieldTestType
			protected BoardBaseTestType( uint width, uint tasksHeight, uint goalsHeight, IBoardPrototypeFactory factory ) : base( width, tasksHeight, goalsHeight, factory )
			{
			}
			#endregion
		}
		private static readonly object[] constructorParameters;
		static BoardBaseTest()
		{
			/*constructorParameters = new object[]
			{
				new object[] { 1u, 3u, 5u,  }
			};*/
		}
		#endregion
		#region Test
		/*[TestCaseSource( nameof( constructorParameters ) )]
		public void ConstructorFillsAllProperties( ulong id, TeamColour team, PlayerType type, DateTime timestamp, IField field, IPlayerPiece piece )
		{
			var sut = new Player( id, team, type, timestamp, field, piece );
			Assert.Multiple( () =>
			{
				Assert.AreEqual( id, sut.Id );
				Assert.AreEqual( team, sut.Team );
				Assert.AreEqual( type, sut.Type );
				Assert.AreEqual( timestamp, sut.Timestamp );
				Assert.AreSame( field, sut.Field );
				Assert.AreSame( piece, sut.Piece );
			} );
		}*/
		#endregion
	}
}