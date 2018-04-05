using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.Enums;
using System;

namespace Shared.Components.Extensions
{
	public static class ExtensionIPlayer
	{
		public static bool IsDefault( this IPlayer player ) => player.Timestamp == default && player.Field is null && player.Piece is null;
		public static IPlayer MakePlayer( this IPlayer player, ulong id, TeamColour team, PlayerType type, DateTime timestamp = default, IField field = null, IPlayerPiece piece = null ) => player.CreatePlayer( id, team, type, timestamp, field, piece );
		public static IPlayer SetTimestamp( this IPlayer player, DateTime timestamp = default ) => player.CreatePlayer( player.Id, player.Team, player.Type, timestamp, player.Field, player.Piece );
		public static IPlayer SetField( this IPlayer player, IField field = null ) => player.CreatePlayer( player.Id, player.Team, player.Type, player.Timestamp, field, player.Piece );
		public static IPlayer SetPiece( this IPlayer player, IPlayerPiece piece = null ) => player.CreatePlayer( player.Id, player.Team, player.Type, player.Timestamp, player.Field, piece );
        public static uint? GetX(this IPlayer player) => player?.Field.X;
        public static uint? GetY(this IPlayer player) => player?.Field.Y;
    }
}