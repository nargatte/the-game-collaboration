using SingleGame.ViewModels.Commands;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using IModel = SingleGame.Models.IMainM;

namespace SingleGame.ViewModels.Base
{
	interface IViewModel : INotifyPropertyChanged
	{
		IModel Model { get; }
		IAsyncCommand InitializeAsyncCommand { get; }
	}
	class ViewModel : IViewModel
	{
		#region IViewModel
		public virtual event PropertyChangedEventHandler PropertyChanged = delegate { };
		public virtual IModel Model { get; private set; }
		public virtual IAsyncCommand InitializeAsyncCommand { get; protected set; }
		#endregion
		#region ViewModel
		protected virtual Task InitializeAsync() => Task.CompletedTask;
		private ViewModel() => InitializeAsyncCommand = new AsyncCommand( InitializeAsync );
		public ViewModel( IModel model = null ) : this() => Model = model;
		protected bool SetProperty<T>( ref T backingField, T value, [CallerMemberName] string propertyName = null )
		{
			if( EqualityComparer<T>.Default.Equals( backingField, value ) )
				return false;
			else
			{
				backingField = value;
				OnPropertyChanged( propertyName );
				return true;
			}
		}
		protected void OnPropertyChanged( [CallerMemberName] string propertyName = null ) => PropertyChanged.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
		#endregion
	}
}