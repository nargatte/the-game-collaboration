using Shared.Components.Players;
using System;

namespace Shared.Components.Fields
{
	public interface IField
    {
		uint X { get; set; }
		uint Y { get; set; }
        DateTime Timestamp { get; set; }
        IPlayer Player { get; set; }
		IField CloneField();
    }
}
