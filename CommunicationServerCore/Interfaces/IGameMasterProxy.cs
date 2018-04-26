namespace CommunicationServerCore.Interfaces
{
    public interface IGameMasterProxy
    {
        ICommunicationServer CommunicationServer { get; set; }
    }
}