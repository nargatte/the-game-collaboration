namespace PlayerCore.Interfaces
{
    public interface ICommunicationServerProxy
    {
        IPlayer Player { get; set; }

        void Send<T>(T );
    }
}