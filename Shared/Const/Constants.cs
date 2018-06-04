using System.Text;

namespace Shared.Const
{
	public static class Constants
	{
		public static readonly string EndOfMessage = ( ( char )23 ).ToString();
		public static readonly Encoding Encoding = Encoding.UTF8;
		public static readonly double KeepAliveFrequency = 2.0;
		public const ulong AnonymousId = 0uL;
		public const bool Cooperation = true;
	}
}
