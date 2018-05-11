using System.Text;

namespace Shared.Components
{
	public static class ConstHelper
	{
		public static readonly string EndOfMessage = ( ( char )23 ).ToString();
		public static readonly Encoding Encoding = Encoding.UTF8;
		public static readonly double KeepAliveFrequency = 5.0;
	}
}
