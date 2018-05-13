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
		public Identity( HostType type = HostType.Unknown, ulong aId = 0uL )
		{
			Type = type;
			if( ( Type is HostType.CommunicationServer || Type is HostType.Unknown ) && aId != 0uL )
				throw new ArgumentOutOfRangeException( nameof( aId ) );
			Id = aId;
		}
		public override string ToString() => ( Type is HostType.CommunicationServer || Type is HostType.Unknown ) ? $"[{ Type }]" : ( Id == 0uL ? $"[{ Type }:Anonymous]" : $"[{ Type }:{ Id }]" );
		#endregion
	}
}
