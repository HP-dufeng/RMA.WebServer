using Abp.EntityFrameworkCore;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace RMA.WebServer.EntityFrameworkCore
{
    [DependsOn(
        typeof(RMAWebServerCoreModule), 
        typeof(AbpEntityFrameworkCoreModule))]
    public class RMAWebServerEntityFrameworkCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(RMAWebServerEntityFrameworkCoreModule).GetAssembly());
        }
    }
}