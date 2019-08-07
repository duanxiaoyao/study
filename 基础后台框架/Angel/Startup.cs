using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Angel.IServices.ILogin;
using Angel.Services.Login;
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
            services.AddCors(options =>
            {
                options.AddPolicy("allow_all",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<ISqlQuery, SqlQuery>();
            services.AddScoped<ISqlConfig, MsSqlConfig>();
            services.AddSingleton<IUserContext, UserContext>();
            services.AddScoped<ILoginServices, LoginServices>();
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

            app.UseCors("allow_all");
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
                    Title = "单纯玩玩Api",
                    Description = "无聊",
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
