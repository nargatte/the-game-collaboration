//using Moq;
//using NUnit.Framework;
//using Shared.Components.Fields;
//using Shared.Components.Players;
//using System;

//namespace SharedUnitTests.Components.Fields
//{
//	[TestFixture]
//	public class FieldTest
//	{
//		#region Data
//		private class FieldTestType : Field
//		{
//			#region Field
//			public override IField CreateField( uint x, uint y, DateTime timestamp, IPlayer player ) => throw new NotImplementedException();
//			#endregion
//			#region FieldTestType
//			public FieldTestType( uint x, uint y, DateTime timestamp = default, IPlayer player = null ) : base( x, y, timestamp, player )
//			{
//			}
//			#endregion
//		}
//		private static readonly DateTime dateTimeExample;
//		private static readonly IPlayer playerExample;
//		private static readonly object[] constructorParameters;
//		static FieldTest()
//		{
//			dateTimeExample = DateTime.Now;
//			playerExample = Mock.Of<IPlayer>();
//			constructorParameters = new object[]
//			{
//				new object[] { 0u, 2u, default( DateTime ), null },
//				new object[] { 1u, 3u, dateTimeExample, playerExample }
//			};
//		}
//		#endregion
//		#region Test
//		[TestCaseSource( nameof( constructorParameters ) )]
//		public void ConstructorFillsAllProperties( uint x, uint y, DateTime timestamp, IPlayer player )
//		{
//			var sut = new FieldTestType( x, y, timestamp, player );
//			Assert.Multiple( () =>
//			{
//				Assert.AreEqual( x, sut.X );
//				Assert.AreEqual( y, sut.Y );
//				Assert.AreEqual( timestamp, sut.Timestamp );
//				Assert.AreSame( player, sut.Player );
//			} );
//		}
//		#endregion
//	}
//}
