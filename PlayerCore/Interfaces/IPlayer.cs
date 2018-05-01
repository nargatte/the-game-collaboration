namespace PlayerCore.Interfaces
{
    public interface IPlayer
    {
		uint KeepAliveInterval { get; }
		ICommunicationServerProxy Proxy { get; set; }
    }
}