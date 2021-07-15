using System;
using horus_prueba.Models.Login;
using horus_prueba.ViewModels;
using horus_prueba.Views;
using Newtonsoft.Json;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Unity;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace horus_prueba
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer platformInitializer) : base(platformInitializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            var initNavigation = "/LoginPage";
            var oauthToken = await SecureStorage.GetAsync("oauth_token");
            if (!string.IsNullOrWhiteSpace(oauthToken))
            {
                // Sesión
                var globalSetting = GlobalSetting.GetInstance();
                var oauthUser = await SecureStorage.GetAsync("oauth_user");
                globalSetting.Token = oauthToken;
                globalSetting.User = JsonConvert.DeserializeObject<LoginResponseModel>(oauthUser);
                initNavigation = "/GamificationPage";
            }

            INavigationResult result = await NavigationService.NavigateAsync(initNavigation);

            // Page Help !!
            if (!result.Success)
                SetMainPageFromException(result.Exception);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginViewModel>();
            containerRegistry.RegisterForNavigation<GamificationPage, GamificationViewModel>();
        }

        /// <summary>
        /// Ventana de error
        /// </summary>
        /// <param name="ex">Excepción Navegacion</param>
        private void SetMainPageFromException(Exception ex)
        {
            var layout = new StackLayout
            {
                Padding = new Thickness(40)
            };
            layout.Children.Add(new Label
            {
                Text = ex?.GetType()?.Name ?? "Unknown Error encountered",
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            });

            layout.Children.Add(new ScrollView
            {
                Content = new Label
                {
                    Text = $"{ex}",
                    LineBreakMode = LineBreakMode.WordWrap
                }
            });

            MainPage = new ContentPage
            {
                Content = layout
            };
        }
    }
}
