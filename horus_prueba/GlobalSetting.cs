﻿using System.Text.RegularExpressions;
using horus_prueba.Models.Login;

namespace horus_prueba
{
    /// <summary>
    /// Clase a nivel global lo cual permite almacenar información de forma temporal.
    /// Tag: "Sessión Temporal", hasta que se cierre la aplicación.
    /// Esta información se vuelve almacenar cuando habre la sessión 
    /// Almacena de forma temporal la información.
    /// La infomación se obtiene por medio del "singleton"
    /// </summary>
    public class GlobalSetting
    {
        #region Constructor
        public GlobalSetting()
        {
            instance = this;
        }
        #endregion Constructor

        #region Singleton
        /// <value>Instancia de la clase.</value>
        private static GlobalSetting instance;

        /// <summary>Singleton</summary>
        /// <remarks>Restringir la creación de objetos pertenecientes a una clase o el valor de un tipo a un único objeto. Su intención consiste en garantizar que una clase solo tenga una instancia y proporcionar un punto de acceso global a ella</remarks>
        /// <returns>Singleton <see cref="horus_prueba.GlobalSetting" /> </returns>
        public static GlobalSetting GetInstance()
        {
            if (instance == null)
            {
                return new GlobalSetting();
            }

            return instance;
        }
        #endregion Singleton

        #region API
        #if DEBUG
        public static bool ShowDebugApi = true;
        public static bool IsProduction = false;
        public static string ApiUrl = "https://horuschallenges.azurewebsites.net";
        #else
        public static bool ShowDebugApi = false;
        public static bool IsProduction = true;
        public static string ApiUrl = "https://horuschallenges.azurewebsites.net";
        #endif

        public static string ApiHostName
        {
            get
            {
                var apiHostName = Regex.Replace(ApiUrl, @"^(?:http(?:s)?://)?(?:www(?:[0-9]+)?\.)?", string.Empty, RegexOptions.IgnoreCase)
                                   .Replace("/", string.Empty);
                return apiHostName;
            }
        }
        #endregion API

        #region Properties
        /// <value>Usuario Autenticado.</value>
        public LoginResponseModel User { get; set; }

        /// <value>Token Usuario Autenticado.</value>
        public string Token { get; set; }
        #endregion Properties
    }
}
