using Newtonsoft.Json;

namespace horus_prueba.Models.Login
{
    public class LoginModel
    {
        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }
    }
}
