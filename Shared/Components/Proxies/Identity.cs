using Shared.Const;
using Shared.Enums;
using Shared.Interfaces.Proxies;
using System;

namespace Shared.Components.Proxies
{
	public class Identity : IIdentity
	{
		#region IIdentity
		public virtual HostType Type { get; }
		public virtual ulong Id { get; }
		#endregion
		#region Identity
		public Identity( HostType type = HostType.Unknown, ulong aId = Constants.AnonymousId )
		{
			Type = type;
			if( ( Type is HostType.CommunicationServer || Type is HostType.Unknown ) && aId != Constants.AnonymousId )
				throw new ArgumentOutOfRangeException( nameof( aId ) );
			Id = aId;
		}
		public override string ToString() => ( Type is HostType.CommunicationServer || Type is HostType.Unknown ) ? $"[{ Type }]" : ( Id == Constants.AnonymousId ? $"[{ Type }:Anonymous]" : $"[{ Type }:{ Id }]" );
		#endregion
	}
}
