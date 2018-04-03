using Shared.Components.Players;
using System;

namespace Shared.Components.Fields
{
    public interface IField
    {
		uint X { get; }
		uint Y { get; }
        DateTime Timestamp { get; }
        IPlayer Player { get; }
		IField CreateField( uint x, uint y, DateTime timestamp, IPlayer player );
    }
}
