using System;
using System.Threading.Tasks;
using System.Windows.Input;
using horus_prueba.Models.Login;
using horus_prueba.Services.Request;
using Prism.Commands;
using Prism.Navigation;

namespace horus_prueba.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Attributes
        private string _email;
        private string _password;
        private bool _showPasswordVisible = true;
        private bool _showEyePassword = true;
        private bool _noShowEyePassword;

        private IApiManager _apiManager;
        private INavigationService _navigationService;
        #endregion /Attributes

        #region Properties
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        public bool ShowPasswordVisible
        {
            get => _showPasswordVisible;
            set => SetProperty(ref _showPasswordVisible, value);
        }
        public bool ShowEyePassword
        {
            get => _showEyePassword;
            set => SetProperty(ref _showEyePassword, value);
        }
        public bool NoShowEyePassword
        {
            get => _noShowEyePassword;
            set => SetProperty(ref _noShowEyePassword, value);
        }
        #endregion /Properties

        #region Commands
        public ICommand ShowPassCommand => new DelegateCommand<string>(OnShowPassCommand);
        public ICommand LoginCommand => new DelegateCommand(async () => await RunSafe(OnLoginCommand(), loadingMessage: "Iniciando Sesión..."));
        #endregion /Commands

        #region Constructor
        public LoginViewModel(IApiManager apiManager, INavigationService navigationService)
        {
            this._apiManager = apiManager;
            this._navigationService = navigationService;

            InitLogin();
        }
        #endregion Constructor

        #region Methods
        private void InitLogin()
        {
            // TEST
            // this.Email = "user2@mail.com";
            // this.Password = "Password2";
        }

        /// <summary>
        /// Validar campos formulario Login
        /// </summary>
        /// <returns>Resultado Validación</returns>
        private bool AreFieldsValid() {
            return !string.IsNullOrWhiteSpace(this.Email) && !string.IsNullOrWhiteSpace(this.Password);
        }
        #endregion Methods

        #region Commands
        /// <summary>
        /// Muestra o no la contraseña
        /// </summary>
        /// <param name="isShow"></param>
        private void OnShowPassCommand(string isShow)
        {
            this.ShowEyePassword = false;
            this.NoShowEyePassword = false;
            if (isShow == "Yes")
            {
                this.NoShowEyePassword = true;
                this.ShowPasswordVisible = false;
            }
            else
            {
                this.ShowEyePassword = true;
                this.ShowPasswordVisible = true;
            }
        }
        /// <summary>
        /// Validación para iniciar sesión
        /// </summary>
        private async Task OnLoginCommand()
        {
            await Task.Delay(250);

            // Validamos Formulario
            if (!AreFieldsValid()) {
                PageDialog.Toast("Por favor completar el formulario.");
                return;
            }

            // Login API
            LoginModel login = new LoginModel
            {
                Email = this.Email,
                Password = this.Password
            };
            var loginApi = await this._apiManager.Login(login);
            if (!loginApi.IsSuccess)
            {
                string titleAlert = loginApi.StatusCode == System.Net.HttpStatusCode.Unauthorized ? "Iniciar Sesión" : "Error";
                PageDialog.Alert(loginApi.Message, titleAlert, "Aceptar");
                return;
            }

            // Success Login
            var navigation = await this._navigationService.NavigateAsync("/GamificationPage");
            if (!navigation.Success)
                PageDialog.Alert($"{navigation.Exception}");
        }
        #endregion Commands

        #region Navigation
        #endregion Navigation
    }
}
