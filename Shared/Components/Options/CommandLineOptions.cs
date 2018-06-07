using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace Shared.Components.Options
{
	public static class CommandLineOptions
	{
		public static IDictionary<string, string> GetDictonary( string[] args )
		{
			var dictionary = new Dictionary<string, string>();
			string option = null;
			foreach( string arg in args )
				if( option is null )
				{
					if( arg.StartsWith( "--" ) )
						option = arg.Substring( 2 );
				}
				else
				{
					dictionary.Add( option, arg );
					option = null;
				}
			return dictionary;
		}
		public static T GetConfigFile< T >( string path ) where T : class
		{
			var serializer = new XmlSerializer( typeof( T ) );
			using( var fileStream = new FileStream( path, FileMode.Open ) )
			{
				using( var xmlReader = XmlReader.Create( fileStream ) )
				{
					return serializer.CanDeserialize( xmlReader ) ? serializer.Deserialize( xmlReader ) as T : null;
				}
			}
		}
		public static void CancelOnCtrlC( CancellationTokenSource cts ) => Console.CancelKeyPress += ( s, e ) =>
		{
			if( e.SpecialKey == ConsoleSpecialKey.ControlC )
			{
				e.Cancel = true;
				cts.Cancel();
			}
		};
	}
}
