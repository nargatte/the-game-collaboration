using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shared.Components.Fields;
using Shared.Components.Players;
using Shared.Messages.Communication;
using Shared.Messages.Serialization;

namespace SharedUnitTests.Serialization
{

    [TestClass]
    public class SerializationTestClass
    {
        [TestMethod]
        public void SerializeTest()
        {
            var dataToSerialize = new Shared.Messages.Communication.ConfirmJoiningGame()
            {
                gameId = 1234,
                playerId = 567,
                privateGuid = "some random GUID"
            };
            var resultingXML = XMLSerialization.Serialize(dataToSerialize, new System.Xml.Serialization.XmlSerializerNamespaces());
            var resultingObject = XMLSerialization.Deserialize<ConfirmJoiningGame>(resultingXML);
            Assert.AreEqual(dataToSerialize.gameId, resultingObject.gameId);
            Assert.AreEqual(dataToSerialize.playerId, resultingObject.playerId);
            Assert.AreEqual(dataToSerialize.privateGuid, resultingObject.privateGuid);
            Assert.IsNull(resultingObject.PlayerDefinition);
        }
    }
}
