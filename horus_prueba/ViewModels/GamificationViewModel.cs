using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using horus_prueba.Models.Challenges;
using horus_prueba.Services.Request;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace horus_prueba.ViewModels
{
    public class GamificationViewModel : BaseViewModel
    {
        #region /Attributes
        private IApiManager _apiManager;
        private INavigationService _navigationService;
        
        private ObservableCollection<ChallengeModel> _challenges;
        private long _totalMake;
        private long _totalCards;
        #endregion /Attributes

        #region Properties
        public ObservableCollection<ChallengeModel> Challenges
        {
            get => _challenges;
            set => SetProperty(ref _challenges, value);
        }

        public long TotalMake
        {
            get => _totalMake;
            set => SetProperty(ref _totalMake, value);
        }
        public long TotalCards
        {
            get => _totalCards;
            set => SetProperty(ref _totalCards, value);
        }
        #endregion /Properties

        #region Commands
        public ICommand ChallengeCommand => new DelegateCommand<ChallengeModel>(OnChallengeCommand);
        public ICommand SingOutCommand => new DelegateCommand(OnSingOutCommand);
        #endregion Commands

        #region Constructor
        public GamificationViewModel(IApiManager apiManager, INavigationService navigationService)
        {
            // Variables
            this._apiManager = apiManager;
            this._navigationService = navigationService;
            this.Challenges = new ObservableCollection<ChallengeModel>();

            // Commands
            RefreshCommand = new Command(async () => await RunSafe(InitGamification(), ShowLoading: false));
        }
        #endregion Constructor

        #region Methods
        #endregion Methods

        #region Commands
        private async Task InitGamification()
        {
            // Eventos Iniciales
            await Task.Delay(250);
            this.Challenges.Clear();

            // API
            var challengeApi = await this._apiManager.GetChallenges();
            if (!challengeApi.IsSuccess)
            {
                PageDialog.Alert(challengeApi.Message, "Error", "Aceptar");
                return;
            }
            var challenges = challengeApi.Response as List<ChallengeModel>;
            this.Challenges = new ObservableCollection<ChallengeModel>(challenges);

            this.TotalMake = this.Challenges.Where(c => c.CurrentPoints == c.TotalPoints).ToList().Count();
            this.TotalCards = this.Challenges.Count;
        }

        /// <summary>
        /// Comando para cerrar sesión
        /// </summary>
        private async void OnSingOutCommand()
        {
            var navigation = await this._navigationService.NavigateAsync("/LoginPage");
            if (!navigation.Success)
                PageDialog.Alert($"{navigation.Exception}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="challenge"></param>
        private void OnChallengeCommand(ChallengeModel challenge)
        {

        }
        #endregion Commands

        #region Navigation
        public override void OnAppearing()
        {
            if (!this.IsInit)
                return;

            this.IsRefreshing = true;
            this.IsInit = false;
        }
        #endregion Navigation
    }
}
