using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using RMA.WebServer.Configuration;
using RMA.WebServer.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.AspNetCore.SignalR;

namespace RMA.WebServer.Host.Startup
{
    [DependsOn(
    typeof(RMAWebServerApplicationModule),
    typeof(RMAWebServerEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreModule),
    typeof(AbpAspNetCoreSignalRModule))]
    public class RMAWebServerHostModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public RMAWebServerHostModule(IHostingEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(RMAWebServerConsts.ConnectionStringName);


            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(RMAWebServerApplicationModule).GetAssembly()
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(RMAWebServerHostModule).GetAssembly());
        }
    }
}