using System;

namespace Shared.Components.Fields
{
    public interface IField
    {
        DateTime Timestamp { get; }
        ulong? PlayerId { get; }
    }
}
