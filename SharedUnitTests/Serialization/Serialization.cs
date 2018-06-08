using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shared.Components.Fields;
using Shared.Components.Players;
using Shared.Components.Serialization;
using Shared.DTO.Communication;

namespace SharedUnitTests.Serialization
{

    [TestClass]
    public class SerializationTestClass
    {
        [TestMethod]
        public void SerializeTest()
        {
            var dataToSerialize = new Shared.DTO.Communication.ConfirmJoiningGame()
            {
                GameId = 1234,
                PlayerId = 567,
                PrivateGuid = "some random GUID"
            };
            //var resultingXML = XMLSerialization.Serialize(dataToSerialize, new System.Xml.Serialization.XmlSerializerNamespaces());
            //var resultingObject = XMLSerialization.Deserialize<ConfirmJoiningGame>(resultingXML);
            //Assert.AreEqual(dataToSerialize.GameId, resultingObject.gameId);
            //Assert.AreEqual(dataToSerialize.PlayerId, resultingObject.playerId);
            //Assert.AreEqual(dataToSerialize.PrivateGuid, resultingObject.privateGuid);
            //Assert.IsNull(resultingObject.PlayerDefinition);
        }
    }
}
