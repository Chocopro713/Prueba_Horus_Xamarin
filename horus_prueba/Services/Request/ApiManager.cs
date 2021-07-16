namespace horus_prueba.Services.Request
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Acr.UserDialogs;
    using Fusillade;
    using Newtonsoft.Json;
    using Refit;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Prism.Navigation;
    using System.Diagnostics;
    using horus_prueba.Models.Request;
    using horus_prueba.Models.Login;


    // AYUDAS
    // https://xamgirl.com/consuming-restful-web-service-xamarin-forms-using-refit-part-2/
    // https://github.com/CrossGeeks/RefitXamarinFormsSample/tree/Part2-StructuringProject
    public class ApiManager : IApiManager
    {
        #region Attributes
        private bool _isConnected;
        private INavigationService _navigationService;
        private IApiService<IHorusApi> _horusApi;
        #endregion

        #region Porperties
        private IUserDialogs PageDialog = UserDialogs.Instance;


        Dictionary<int, CancellationTokenSource> runningTasks = new Dictionary<int, CancellationTokenSource>();
        Dictionary<string, Task<HttpResponseMessage>> taskContainer = new Dictionary<string, Task<HttpResponseMessage>>();
        #endregion Porperties

        #region Constructor
        public ApiManager(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            this._horusApi = new ApiService<IHorusApi>(GlobalSetting.ApiUrl);
            this._isConnected = (Connectivity.NetworkAccess == NetworkAccess.Internet);

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }
        #endregion Constructor

        #region Public Events

        public async Task<ResponseApiModel> Login(LoginModel login)
        {
            // API
            var task = _horusApi.GetApi(Priority.UserInitiated).Login(login);

            runningTasks.Add(task.Id, new CancellationTokenSource());

            // Response
            var loginApi = await RemoteRequestAsync(task, "Login");
            if (!loginApi.IsSuccess)
                return loginApi;

            // Almacenamos información en sesión
            var response = loginApi.Response as LoginResponseModel;
            var globalSetting = GlobalSetting.GetInstance();

            // Guardar Session
            globalSetting.Token = response.AuthorizationToken;
            globalSetting.User = response;

            // TODO :: Es necesario tener activado un perfil apple en el VS Mac
            if (Device.RuntimePlatform != Device.iOS)
            { 
                await SecureStorage.SetAsync("oauth_token", globalSetting.Token);
                await SecureStorage.SetAsync("oauth_user", JsonConvert.SerializeObject(response));
            }

            return loginApi;
        }

        public async Task<ResponseApiModel> GetChallenges()
        {
            var task = _horusApi.GetApi(Priority.UserInitiated).GetChallenges();
            runningTasks.Add(task.Id, new CancellationTokenSource());
            return await RemoteRequestAsync(task, "GetChallenges");
        }
        #endregion Public Events


        #region Private Events
        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            this._isConnected = (e.NetworkAccess == NetworkAccess.Internet);
            if (!this._isConnected)
                this.ClearAllRunningTask();
        }

        private void ClearAllRunningTask()
        {
            var items = runningTasks.ToList();
            foreach (var item in items)
            {
                item.Value.Cancel();
                runningTasks.Remove(item.Key);
            }
        }

        /// <summary>
        /// Realiza validación consumo API.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="taskApi">Tarea API</param>
        /// <returns></returns>
        private async Task<ResponseApiModel> RemoteRequestAsync<T>(Task<ApiResponse<T>> taskApi, string method)
        {
            try
            {
                // Valida conexión
                var globalSetting = GlobalSetting.GetInstance();
                if (!this._isConnected)
                    return new ResponseApiModel { IsSuccess = false, Message = "No hay conexión a internet." };

                // API
                var response = await taskApi;

                // SUCCESS
                if (response.IsSuccessStatusCode)
                {
                    runningTasks.Remove(taskApi.Id);
                    return new ResponseApiModel
                    {
                        IsSuccess = true,
                        Response = response.Content,
                        StatusCode = response.StatusCode
                    };
                }

                // ERROR
                ClearAllRunningTask();
                ResponseApiModel responseError = new ResponseApiModel { IsSuccess = false, StatusCode = response.StatusCode };
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        if (method != "Login")
                        {
                            this.LimpiarToken();
                            var navParameters = new NavigationParameters { { "message", "Su sesión ha expirado, ingresa nuevamente por favor 😉." } };
                            await this._navigationService.NavigateAsync(new Uri("/LoginPage", UriKind.Absolute), navParameters);
                        }
                        else
                        {
                            responseError.Message = "Usuario o contraseña incorrecta.";
                        }
                        break;
                    case HttpStatusCode.InternalServerError:
                        responseError.Message = "Ops, ha ocurrido un error interno en el servidor, por favor intenta nuevamente.";
                        break;
                    default:
                        responseError.Message = $"Ops, ha ocurrido un error. [{response.StatusCode}]";
                        break;
                }
                return responseError;
            }
            catch (ValidationApiException validationException)
            {
                // handle validation here by using validationException.Content,
                // which is type of ProblemDetails according to RFC 7807

                // If the response contains additional properties on the problem details,
                // they will be added to the validationException.Content.Extensions collection.
                // APPCENTER PRINT ERROR with "method"

                return new ResponseApiModel { IsSuccess = false, Message = "Ops, ha ocurrido un error. [validationException]", StatusCode = validationException.StatusCode };
            }
            catch (ApiException exception)
            {
                // APPCENTER PRINT ERROR with "method"
                // other exception handling
                return new ResponseApiModel { IsSuccess = false, Message = "Ops, ha ocurrido un error. [Api Exception]", StatusCode = exception.StatusCode };
            }
            catch (Exception ex)
            {
                if(ex.Message == "No such host is known")
                    return new ResponseApiModel { IsSuccess = false, Message = "No es posible conectar al servidor de destino."};
                else
                    return new ResponseApiModel { IsSuccess = false, Message = "Ops, ha ocurrido un error. [Exception]"};
            }
        }

        /// <summary>
        /// Limpiar registro de usuario en la aplicación
        /// </summary>
        /// <returns></returns>
        private void LimpiarToken()
        {
            var globalSetting = GlobalSetting.GetInstance();
            globalSetting.User = null;
            globalSetting.Token = null;
            SecureStorage.Remove("oauth_user");
            SecureStorage.Remove("oauth_token");
        }
        #endregion Private Events
    }
}
