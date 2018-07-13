using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMA.WebServer.Host.Controllers
{
    public class HomeController : WebServerControllerBase
    {
        public ActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}
