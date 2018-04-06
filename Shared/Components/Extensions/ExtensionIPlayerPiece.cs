using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.Enums;
using System;

namespace Shared.Components.Extensions
{
	public static class ExtensionIPlayerPiece
	{
		public static void Clone( this IPlayerPiece piece, IPlayerPiece aPiece )
		{
			( piece as IPiece ).Clone( aPiece );
			var player = piece.Player;
			piece.Player = null;
			var aPlayer = player.ClonePlayer();
			piece.Player = player;
			aPiece.Player = aPlayer;
		}
		public static bool IsDefault( this IPlayerPiece piece ) => ( piece as IPiece ).IsDefault() && piece.Player is null;
	}
}