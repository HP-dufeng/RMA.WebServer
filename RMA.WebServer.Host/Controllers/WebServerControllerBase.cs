using Abp.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMA.WebServer.Host.Controllers
{
    public abstract class WebServerControllerBase : AbpController
    {
        protected WebServerControllerBase()
        {

        }
    }
}
