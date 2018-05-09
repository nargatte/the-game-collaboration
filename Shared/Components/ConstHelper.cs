using System.Text;

namespace Shared.Components
{
	public static class ConstHelper
	{
		public static readonly string KeepAliveByte = ( ( char )23 ).ToString();
		public static readonly string EndOfMessage = KeepAliveByte;
		public static readonly Encoding Encoding = Encoding.UTF8;
	}
}
