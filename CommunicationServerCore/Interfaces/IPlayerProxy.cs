namespace CommunicationServerCore.Interfaces
{
    public interface IPlayerProxy
    {
		ICommunicationServer CommunicationServer { get; set; }
	}
}