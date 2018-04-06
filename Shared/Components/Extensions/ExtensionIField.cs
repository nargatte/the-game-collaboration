using Shared.Components.Fields;
using Shared.Components.Players;
using System;

namespace Shared.Components.Extensions
{
	public static class ExtensionIField
	{
		public static bool IsDefault( this IField field ) => field.Timestamp == default && field.Player is null;
		//public static IField MakeField( this IField field, uint x, uint y, DateTime timestamp = default, IPlayer player = null ) => field.CreateField( x, y, timestamp, player );
		//public static IField SetTimestamp( this IField field, DateTime timestamp = default ) => field.CreateField( field.X, field.Y, timestamp, field.Player );
		//public static IField SetPlayer( this IField field, IPlayer player = null ) => field.CreateField( field.X, field.Y, field.Timestamp, player );
	}
}
