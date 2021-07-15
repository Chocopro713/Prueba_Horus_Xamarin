namespace horus_prueba.Services.Request
{
    using System.Threading.Tasks;
    using horus_prueba.Models.Login;
    using horus_prueba.Models.Request;

    public interface IApiManager
    {
        /// <summary>
        /// Iniciar sesión para obtener token
        /// </summary>
        /// <param name="login">Login Model</param>
        /// <returns></returns>
        Task<ResponseApiModel> Login(LoginModel login);

        /// <summary>
        /// Obtiene listado desafios
        /// </summary>
        /// <param name="login">Login Model</param>
        /// <returns></returns>
        Task<ResponseApiModel> GetChallenges();
    }
}
