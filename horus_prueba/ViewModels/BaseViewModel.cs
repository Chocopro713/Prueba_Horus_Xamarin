using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using horus_prueba.Models.Request;
using Newtonsoft.Json;
using Prism.AppModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Essentials;

namespace horus_prueba.ViewModels
{
    public class BaseViewModel : BindableBase, IInitialize, IInitializeAsync, INavigationAware, IDestructible, IPageLifecycleAware
    {
        #region Attributes
        public bool _isInit;
        public bool _isBusy;
        public bool _isRefreshing;
        public string _title;
        public string _appVersion;
        public string _emptyMsg;
        public string _errorMsg;
        public IUserDialogs PageDialog = UserDialogs.Instance;
        #endregion Attributes

        #region Properties
        public bool IsInit
        {
            get { return _isInit; }
            set { SetProperty(ref _isInit, value); }
        }
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public string AppVersion
        {
            get => _appVersion;
            set => SetProperty(ref _appVersion, value);
        }
        public ICommand RefreshCommand { get; set; }
        #endregion Properties

        #region Constructor
        public BaseViewModel()
        {
            this.IsInit = true;
            this.AppVersion = AppInfo.VersionString;
        }
        #endregion

        #region Events
        /// <summary>
        /// Ejecuta evento
        /// </summary>
        /// <param name="task">Acción a ejecutar</param>
        /// <param name="ShowLoading">Visualizar dialogo de espera</param>
        /// <param name="loadingMessage">Mensaje de espera</param>
        /// <returns></returns>
        public async Task RunSafe(Task task, bool ShowLoading = true, string loadingMessage = null)
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                if (ShowLoading)
                {
                    await Task.Delay(150);
                    UserDialogs.Instance.ShowLoading(loadingMessage ?? "Cargando...");
                    await Task.Delay(150);
                }

                await task;
            }
            catch (Exception exception)
            {
                UserDialogs.Instance.Alert("A ocurrido un error:\n" + exception.ToString(), "Error", "Aceptar");

                var properties = new Dictionary<string, string> {
                    { "Type", "RunSafe" },
                    { "Exception", "RunSafeException" }
                };
            }
            finally
            {
                this.IsBusy = false;
                this.IsRefreshing = false;
                if (ShowLoading)
                    PageDialog.HideLoading();
            }
        }
        #endregion Events

        #region Navigation
        /**
         * <summary>Navegacion Prism, cuando inicia la p?gina.</summary>
         * <param name="parameters">Navigation</param>
         */
        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        /**
         * <summary>Navegacion Prism, cuando inicia la p?gina.</summary>
         * <param name="parameters">Navigation</param>
         */
        public virtual Task InitializeAsync(INavigationParameters parameters)
        {
            return Task.CompletedTask;
        }

        /**
         * <summary>Navegacion, Sale del formulario.</summary>
         * <param name="parameters">Parametros en la Navegacion en el sistema.</param>
         */
        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        /**
         * <summary>Navegacion, Cuando Ingresa Al Formulario.</summary>
         * <param name="parameters">Parametros en la Navegacion en el sistema.</param>
         */
        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        /**
         * <summary>Navegacion, Antes De Ingresar Al formulario.</summary>
         * <param name="parameters">Parametros en la Navegacion en el sistema.</param>
         */
        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
        }

        /**
         * <summary>Navegacion, Finaliza Ventana Formulario.</summary>
         */
        public virtual void Destroy()
        {
        }

        /**
         * <summary>Navegacion, Ingresa interfaz ventana.</summary>
         */
        public virtual void OnAppearing()
        {
        }

        /**
         * <summary>Navegacion, Sale interfaz ventana.</summary>
         */
        public virtual void OnDisappearing()
        {
        }
        #endregion
    }
}
