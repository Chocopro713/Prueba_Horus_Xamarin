namespace horus_prueba.Models.Request
{
    using System.Net;

    public class ResponseApiModel
    {
        public bool IsSuccess { get; set; }
        public object Response { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
