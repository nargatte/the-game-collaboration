using System.IO;
using System.Text;

namespace Shared.Components.Serialization
{
	class StringWriterWithEncoding : StringWriter
	{
		#region StringWriter
		public override Encoding Encoding { get; }
		#endregion
		#region StringWriterWithEncoding
		public StringWriterWithEncoding( Encoding encoding ) : base() => Encoding = encoding;
		#endregion
	}
}
