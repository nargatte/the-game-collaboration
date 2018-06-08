using Shared.Components.Proxies;
using Shared.Interfaces.Events;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using TheGame.ViewModels.Base;
using TheGame.ViewModels.Tuples;

namespace TheGame.ViewModels
{
	interface ICommunicationVM : IViewModel
	{
		ICommunicationObserver CommunicationObserver { get; }
		CollectionViewSource Log { get; }
	}
	class CommunicationVM : ViewModel, ICommunicationVM
	{
		#region ICommunicationVM
		public virtual ICommunicationObserver CommunicationObserver { get; }
		public virtual CollectionViewSource Log { get; } = new CollectionViewSource();
		#endregion
		#region CommunicationVM
		public ObservableCollection<CommunicationTuple> log { get; set; } = new ObservableCollection<CommunicationTuple>();
		public CommunicationVM( ICommunicationObserver observer )
		{
			CommunicationObserver = observer;
			//Log.Source = log;
			observer.Sent += ( s, e ) =>
			{
				Application.Current.Dispatcher.Invoke( () => log.Add( new CommunicationTuple( Kind.Sent, e.Local, e.Remote, e.SerializedMessage ) ) );
			};
			observer.Received += ( s, e ) =>
			{
				Application.Current.Dispatcher.Invoke( () => log.Add( new CommunicationTuple( Kind.Received, e.Local, e.Remote, e.SerializedMessage ) ) );
			};
			observer.SentKeepAlive += ( s, e ) =>
			{
				Application.Current.Dispatcher.Invoke( () => log.Add( new CommunicationTuple( Kind.SentKeepAlive, e.Local, e.Remote, null ) ) );
			};
			observer.ReceivedKeepAlive += ( s, e ) =>
			{
				Application.Current.Dispatcher.Invoke( () => log.Add( new CommunicationTuple( Kind.ReceivedKeepAlive, e.Local, e.Remote, null ) ) );
			};
			observer.Discarded += ( s, e ) =>
			{
				Application.Current.Dispatcher.Invoke( () => log.Add( new CommunicationTuple( Kind.Discarded, e.Local, e.Remote, e.SerializedMessage ) ) );
			};
			observer.Disconnected += ( s, e ) =>
			{
				Application.Current.Dispatcher.Invoke( () => log.Add( new CommunicationTuple( Kind.Disconnected, e.Local, e.Remote, null ) ) );
			};
			Application.Current.Dispatcher.Invoke( () => log.Add( new CommunicationTuple( Kind.Discarded, new Identity( Shared.Enums.HostType.CommunicationServer ), new Identity(), "discard it" ) ) );
			log.Add( new CommunicationTuple( Kind.Discarded, new Identity( Shared.Enums.HostType.CommunicationServer ), new Identity(), "discard it" ) );
			log.Add( new CommunicationTuple( Kind.Discarded, new Identity( Shared.Enums.HostType.CommunicationServer ), new Identity(), "discard it" ) );
			//Log.View.Refresh();
		}
		#endregion
	}
}
