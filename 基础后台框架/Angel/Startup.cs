using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common.Context;
using Common.DbAccess;
using Common.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
namespace Angel
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<ISqlQuery, SqlQuery>();
            services.AddScoped<ISqlConfig, MsSqlConfig>();
            services.AddScoped<IUserContext, UserContext>();
            services.AddSwaggerGenConfig();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");
            UseSandSwaggerUI(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        /// <summary>
        /// 加载SwaggerUI
        /// </summary>
        /// <param name="app"></param>
        public void UseSandSwaggerUI(IApplicationBuilder app)
        {
            app.UseSwagger(x =>
            {
            });

            app.UseSwaggerUI(c =>
            {
                //c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Sand.UnifiedStorage.Swagger.index.html");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.OAuthClientId("js");
                //c.OAuthAppName("js");
                c.OAuth2RedirectUrl("http://localhost:50001/swagger/oauth2-redirect.html");
                //c.OAuthClientSecret("test-secret");
                //c.OAuthRealm("test-realm");
                c.OAuthScopeSeparator(" ");
                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                //c.HeadContent = StartupExtension.AddSandAuthentication();
                c.DocExpansion(DocExpansion.None);
            });
        }
    }

    public static class StartupExtension
    {
        /// <summary>
        /// 添加swagger
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerGenConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "米索医馆系统Api",
                });
                c.AddSecurityDefinition("oauth2",
                    new OAuth2Scheme()
                    {
                        Type = "oauth2",
                        Flow = "implicit",
                        AuthorizationUrl = "http://localhost:5000/connect/authorize",
                        //TokenUrl = "http://localhost:5000/connect/token/",
                        //Description = "勾选授权范围，获取Token",
                        Scopes = new Dictionary<string, string>(){
                            { "openid","用户标识" },
                            { "profile","用户资料" },
                            { "api1","api"},
                        }
                    });
                //c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.DocInclusionPredicate((docName, description) => true);
                var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Angel.xml");
                var xmlModelPath = Path.Combine(basePath, "Angel.Model.xml");
                c.IncludeXmlComments(xmlPath);
                c.IncludeXmlComments(xmlModelPath);
                c.DocumentFilter<CustomDocumentFiliter>();
            });
        }
    }
}
