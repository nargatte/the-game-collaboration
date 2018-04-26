namespace PlayerCore.Interfaces
{
    public interface IPlayer
    {
        ICommunicationServerProxy Proxy { get; set; }
    }
}