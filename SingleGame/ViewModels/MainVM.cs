using System.Threading.Tasks;
using SingleGame.Models;
using SingleGame.ViewModels.Base;

namespace SingleGame.ViewModels
{
	interface IMainVM : IViewModel
	{
		IGameMasterVM GameMaster { get; }
	}
	class MainVM : ViewModel, IMainVM
	{
		#region ViewModel
		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();
			Model.Initialize();
			MakeVMs();
			//Model.Run();
		}
		#endregion
		#region IMainVM
		private IGameMasterVM gameMaster;
		public virtual IGameMasterVM GameMaster
		{
			get => gameMaster;
			protected set => SetProperty( ref gameMaster, value );
		}
		#endregion
		#region MainVM
		public MainVM() : base( new MainM() )
		{
		}
		protected void MakeVMs()
		{

		}
		#endregion
	}
}
