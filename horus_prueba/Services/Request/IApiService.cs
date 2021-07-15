using System;
namespace horus_prueba.Services.Request
{
    using System.Collections.Generic;
    using System.Net;
    using Fusillade;

    public interface IApiService<T>
    {
        T GetApi(Priority priority);
    }
}
