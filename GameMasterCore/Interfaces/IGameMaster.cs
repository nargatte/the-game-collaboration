namespace GameMasterCore.Interfaces
{
    public interface IGameMaster
    {
        ICommunicationServerProxy Proxy { get; set; }
    }
}