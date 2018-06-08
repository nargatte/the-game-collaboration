using CommunicationServerCore.Components.Factories;
using CommunicationServerCore.Components.Modules;
using CommunicationServerCore.Components.Options;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TheGame.ViewModels.Base;
using TheGame.ViewModels.Commands;

namespace TheGame.ViewModels
{
	interface IMainVM : IViewModel
	{
		IAsyncCommand KillAsyncCommand { get; }
		ObservableCollection<ICommunicationServerVM> CommunicationServers { get; }
		string CommunicationServerPort { get; set; }
		string CommunicationServerConf { get; set; }
		IAsyncCommand RunCommunicationServerAsyncCommand { get; }
	}
	class MainVM : ViewModel, IMainVM
	{
		#region ViewModel
		#endregion
		#region IMainVM
		public virtual IAsyncCommand KillAsyncCommand { get; }
		public virtual ObservableCollection<ICommunicationServerVM> CommunicationServers { get; } = new ObservableCollection<ICommunicationServerVM>();
		private string communicationServerPort;
		public virtual string CommunicationServerPort
		{
			get => communicationServerPort;
			set => SetProperty( ref communicationServerPort, value );
		}
		private string communicationServerConf;
		public virtual string CommunicationServerConf
		{
			get => communicationServerConf;
			set => SetProperty( ref communicationServerConf, value );
		}
		public virtual IAsyncCommand RunCommunicationServerAsyncCommand { get; }
		#endregion
		#region MainVM
		private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		private int communicationServerId = 0;
		private int gameMasterId = 0;
		private int playerId = 0;
		public MainVM()
		{
			KillAsyncCommand = new AsyncCommand( Kill );
			RunCommunicationServerAsyncCommand = new AsyncCommand( RunCommunicationServer );
		}
		protected Task Kill()
		{
			try
			{
				try
				{
					cancellationTokenSource.Cancel();
				}
				finally
				{
					cancellationTokenSource.Dispose();
				}
			}
			catch( Exception )
			{
			}
			return Task.CompletedTask;
		}
		protected async Task RunCommunicationServer()
		{
			try
			{
				string[] args = new string[]
				{
					"--port", CommunicationServerPort, "--conf", CommunicationServerConf
				};
				var options = new CommunicationServerOptions( args );
				var module = new CommunicationServerModule( "localhost", options.Port, options.Conf, new CommunicationServerFactory() );
				var vm = new CommunicationServerVM( communicationServerId++, module );
				CommunicationServers.Add( vm );
				await vm.RunAsync( cancellationTokenSource.Token ).ConfigureAwait( false );
			}
			catch( Exception e )
			{
				MessageBox.Show( e.ToString(), "Exception" );
			}
		}
		#endregion
	}
}
