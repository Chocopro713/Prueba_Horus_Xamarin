using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using horus_prueba.Models.Challenges;
using Prism.Commands;
using Prism.Navigation;

namespace horus_prueba.ViewModels
{
    public class GamificationViewModel : BaseViewModel
    {
        #region Attributes
        private INavigationService _navigationService;
        private ObservableCollection<ChallengeModel> _retos;
        #endregion Attributes

        #region Properties
        public ObservableCollection<ChallengeModel> Retos
        {
            get;
            set;
        }

        public ICommand SingOutCommand => new DelegateCommand(OnSingOutCommand);
        #endregion Properties

        #region Constructor
        public GamificationViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;

            this.Retos = new ObservableCollection<ChallengeModel>();
        }
        #endregion Constructor

        #region Methods
        #endregion Methods

        #region Commands
        /// <summary>
        /// Comando para cerrar sesión
        /// </summary>
        private async void OnSingOutCommand()
        {
            var navigation = await this._navigationService.NavigateAsync("LoginPage");
            if (!navigation.Success)
                PageDialog.Alert($"{navigation.Exception}");
        }
        #endregion Commands

        #region Navigation
        #endregion Navigation
    }
}
