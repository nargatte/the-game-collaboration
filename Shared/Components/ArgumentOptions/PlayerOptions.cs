using System.Collections.Generic;
using Shared.Enums;

namespace Shared.Components.ArgumentOptions
{
    public class PlayerOptions : GameMasterOptions
    {
        public PlayerOptions(Dictionary<string, string> dictionary) : base(dictionary)
        {
            Game = dictionary["game"];
            Team = dictionary["team"] == "red" ? TeamColour.Red : TeamColour.Blue;
            Role = dictionary["role"] == "leader" ? PlayerRole.Leader : PlayerRole.Member;
        }

        public string Game { get; }
        public TeamColour Team { get; }
        public PlayerRole Role { get; }
    }
}