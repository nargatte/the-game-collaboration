using Shared.Const;
using Shared.Enums;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Proxies;

namespace Shared.Components.Extensions
{
	public static class ExtensionIProxyComponentFactory
	{
		public static IIdentity MakeIdentity( this IProxyComponentFactory factory, HostType type = HostType.Unknown, ulong id = ConstHelper.AnonymousId ) => factory.CreateIdentity( type, id );
	}
}
