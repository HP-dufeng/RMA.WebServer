using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RMA.WebServer
{
    [DependsOn(
    typeof(RMAWebServerCoreModule),
    typeof(AbpAutoMapperModule))]
    public class RMAWebServerApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(RMAWebServerApplicationModule).GetAssembly());
        }
    }
}
