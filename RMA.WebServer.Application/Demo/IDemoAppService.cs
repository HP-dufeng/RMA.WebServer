using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RMA.WebServer.Demo
{
    public interface IDemoAppService : IApplicationService
    {
        object Get();

        object GetUser();

        object GetAllClient();

        Task PublishMessage(long userId, string message);
    }
}
