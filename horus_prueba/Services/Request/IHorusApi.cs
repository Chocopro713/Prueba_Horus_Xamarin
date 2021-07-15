using System.Collections.Generic;
using System.Threading.Tasks;
using horus_prueba.Models.Challenges;
using horus_prueba.Models.Login;
using Refit;


namespace horus_prueba.Services.Request
{
    public interface IHorusApi
    {
        [Post("/api/UserSignIn")]
        Task<ApiResponse<LoginResponseModel>> Login([Body] LoginModel login);

        [Get("/api/Challenges")]
        Task<ApiResponse<List<ChallengeModel>>> GetChallenges();

        /*

        [Post("/user/login?_format=json")]
        Task<HttpResponseMessage> Test([Body(BodySerializationMethod.Serialized)] object apiRequest);

        [Post("/user/logout?_format=json&token={logoutToken}")]
        Task<HttpResponseMessage> Logout(string logoutToken);

        [Post("/wa/sevin/api/usuarios?_format=json")]
        Task<ApiResponse<ApiResponseModel>> UsuariosApi([Body] object apiRequest);

        [Post("/wa/sevin/api?_format=json")]
        Task<ApiResponse<ApiResponseModel>> GlobalesApi([Body] object apiRequest);

        [Post("/wa/sevin/api/pedidos?_format=json")]
        Task<ApiResponse<ApiResponseModel>> PedidosApi([Body] object apiRequest);
        */
    }
}
