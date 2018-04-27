using CommunicationServerCore.Interfaces;
using Shared.DTOs.Configuration;

namespace CommunicationServerCore.Components.Modules
{
	public class CommunicationServerModule : ICommunicationServerModule
	{
		#region ICommunicationServerCore
		public virtual int Port { get; }
		public virtual CommunicationServerSettings Configuration { get; }
		public virtual void Start()
		{
			;
			System.Console.WriteLine( "CS module" );
		}
		#endregion
		#region CommunicationServerCore
		#endregion
	}
}
