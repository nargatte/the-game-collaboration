using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Shared.Components.Events;
using Shared.Interfaces;
using SingleGame.ViewModels.Base;

namespace SingleGame.ViewModels
{
    interface IGameMasterVM : IViewModel
    {
        IBoardVM Board { get; }
        ObservableCollection<LogArgs> Log { get; }
    }
    class GameMasterVM : ViewModel, IGameMasterVM
    {
        #region ViewModel
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync().ConfigureAwait(false);
            MakeBoardVM();
        }
        #endregion
        #region IGameMasterVM
        private IBoardVM board;
        public virtual IBoardVM Board
        {
            get => board;
            protected set => SetProperty(ref board, value);
        }
        public virtual ObservableCollection<LogArgs> Log { get; private set; } = new ObservableCollection<LogArgs>();
        #endregion
        #region GameMasterVM
        private IGameMaster gameMaster;
        public GameMasterVM(IGameMaster aGameMaster)
        {
            gameMaster = aGameMaster;
            Initialize();
        }
        protected void Initialize() => gameMaster.Log += (s, e) =>
            {
                Application.Current.Dispatcher.Invoke(() => Log.Add(e));
                AppendLogsToFile(e);
            };

        protected void MakeBoardVM() => Board = new BoardVM(gameMaster.Board);
        #endregion
        #region loggingHelpers
        private void AppendLogsToFile(LogArgs e)
        {
            var delim = ",";
            using (var file = new StreamWriter(@"./gamemaster.csv", true, Encoding.UTF8))
            {
                file.WriteLine(e.Type + delim + e.Timestamp.ToUniversalTime() + delim + e.GameId + delim + e.PlayerId + delim + e.PlayerGuid + delim + e.Colour + delim + e.Role);
            }
        }
        #endregion
    }
}
