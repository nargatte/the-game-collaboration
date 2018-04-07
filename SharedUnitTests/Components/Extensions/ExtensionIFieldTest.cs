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
		private static readonly IPlayer playerExample2;
		private static readonly object[] parametersWithIsDefault;
		static ExtensionIFieldTest()
		{
			dateTimeExample = DateTime.Now;
			playerExample = Mock.Of<IPlayer>();
			playerExample2 = Mock.Of<IPlayer>( p => p.ClonePlayer() == playerExample );
			parametersWithIsDefault = new object[]
			{
				new object[] { default( DateTime ), null, true },
				new object[] { dateTimeExample, null, false },
				new object[] { default( DateTime ), playerExample, false },
			};
		}
		#endregion
		#region Test
		[Test]
		public void CloneDeepCopiesPlayerAndAssigns()
		{
			var player = Mock.Of<IPlayer>( p => p.ClonePlayer() == playerExample );
			var mockField = new Mock<IField>();
			mockField.SetupProperty( f => f.Player, player );
			var field = mockField.Object;
			var mockField2 = new Mock<IField>();
			mockField2.SetupProperty( f => f.Player );
			var aField = mockField2.Object;
			field.Clone( aField );
			Assert.Multiple( () =>
			{
				Assert.AreSame( field.Player, player );
				Assert.AreSame( aField.Player, playerExample );
			} );
		}
		[TestCaseSource( nameof( parametersWithIsDefault ) )]
		public void IsDefaultReturnsTrueIfAndOnlyIfObjectWasPrepopulated( DateTime timestamp, IPlayer player, bool expected )
		{
			var sut = Mock.Of<IField>( field => field.Timestamp == timestamp && field.Player == player );
			Assert.AreEqual( expected, sut.IsDefault() );
		}
		#endregion
	}
}
