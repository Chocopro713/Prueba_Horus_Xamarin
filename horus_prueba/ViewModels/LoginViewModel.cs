using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;

namespace horus_prueba.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Attributes
        private INavigationService _navigationService;
        #endregion Attributes

        #region Properties
        public ICommand LoginCommand => new DelegateCommand(OnLoginCommand);
        #endregion Properties

        #region Constructor
        public LoginViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
        }
        #endregion Constructor

        #region Methods
        #endregion Methods

        #region Commands
        /// <summary>
        /// Validación para iniciar sesión
        /// </summary>
        private async void OnLoginCommand()
        {
            var navigation = await this._navigationService.NavigateAsync(new Uri("GamificationPage", UriKind.Absolute));
            if (!navigation.Success)
                PageDialog.Alert($"{navigation.Exception}");
        }
        #endregion Commands

        #region Navigation
        #endregion Navigation
    }
}
