using Shared.Interfaces.Proxies;

namespace TheGame.ViewModels.Tuples
{
	enum Kind
	{
		Sent,
		Received,
		SentKeepAlive,
		ReceivedKeepAlive,
		Discarded,
		Disconnected
	}
	class CommunicationTuple
	{
		Kind Kind { get; }
		string Local { get; } = "lol";
		string Remote { get; }
		string Message { get; }
		public CommunicationTuple( Kind kind, IIdentity local, IIdentity remote, string message )
		{
			Kind = kind;
			Local = local.ToString();
			Remote = remote.ToString();
			Message = message;
		}
	}
}
