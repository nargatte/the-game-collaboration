using Shared.Enums;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Proxies;
using System;
using System.Net;
using System.Net.Sockets;

namespace Shared.Components.Extensions
{
	public static class ExtensionIProxyComponentFactory
	{
		public static IIdentity MakeIdentity( this IProxyComponentFactory factory, HostType type = HostType.Unknown, ulong id = 0uL ) => factory.CreateIdentity( type, id );
	}
}
