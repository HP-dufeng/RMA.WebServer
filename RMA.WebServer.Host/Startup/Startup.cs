using System;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.EntityFrameworkCore;
using RMA.WebServer.EntityFrameworkCore;
using Castle.Facilities.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RMA.WebServer.Host.Authentication;
using Microsoft.Extensions.Configuration;
using RMA.WebServer.Configuration;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using System.Linq;
using Abp.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using IdentityModel.AspNetCore.OAuth2Introspection;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using Abp.AspNetCore.SignalR.Hubs;

namespace RMA.WebServer.Host.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IHostingEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //Configure DbContext
            services.AddAbpDbContext<RMAWebServerDbContext>(options =>
            {
                DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add(new CorsAuthorizationFilterFactory(_defaultCorsPolicyName));
            });



            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            services.AddSignalR();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "IdentityBearer";
            }).AddIdentityServerAuthentication("IdentityBearer", options =>
            {
                options.Authority = _appConfiguration["IdentityServer:Url"];
                options.RequireHttpsMetadata = false;

                options.TokenRetriever = request =>
                {
                    string access_token = TokenRetrieval.FromAuthorizationHeader()(request);

                    if (!string.IsNullOrEmpty(access_token))
                        return access_token;
                    else
                        return TokenRetrieval.FromQueryString()(request);

                };

            });

            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "RMA.WebServer API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });

            //Configure Abp and Dependency Injection
            return services.AddAbp<RMAWebServerHostModule>(options =>
            {
                //Configure Log4Net logging
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                );
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp(); //Initializes ABP framework.

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseJwtTokenMiddleware("IdentityBearer");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<AbpCommonHub>("/signalr");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "RMA.WebServer API V1");   
            }); // URL: /swagger



        }
    }
}
