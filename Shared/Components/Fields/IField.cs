using System;

namespace Shared.Components.Fields
{
    public interface IField
    {
        uint X { get; }
        uint Y { get; }
        DateTime Timestamp { get; }
        ulong? PlayerId { get; }
    }
}
