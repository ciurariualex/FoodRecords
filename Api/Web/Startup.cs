using AutoMapper;
using Core.Context;
using Core.Interfaces;
using Core.Services;
using Core.Utils.Automapper;
using Core.Utils.Interfaces;
using Core.Utils.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartBOS.Core.Services;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Web
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
            string dbConnection = Configuration.GetConnectionString("SqlServer");
            services.AddDbContext<CrudApiContext>(
                options => options.UseSqlServer(dbConnection), ServiceLifetime.Scoped);

            services
                .AddHttpContextAccessor()
                .AddDependencies();

            services
                .AddSession()
                .AddSwaggerServices()
                .AddCors(options =>
                {
                    options.AddPolicy(
                        "CorsPolicy",
                        builder => builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
                })
                .AddMvc(opt => opt.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app
                .UseSession()
                .UseAuthentication()
                .UseHttpsRedirection()
                .UseSwagger()
                .UseCors("CorsPolicy")
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShareHope API v1");
                    c.RoutePrefix = string.Empty;
                })
                .UseMvc();
        }
    }

    public static class ServiceCollectionDataExtension
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordUtils, PasswordUtils>();

            return services;
        }

        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "ShareHope", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                var security = new OpenApiSecurityRequirement();
                security.Add(new OpenApiSecurityScheme { Name = "Bearer" }, new List<string>());

                options.AddSecurityRequirement(security);
            });

            return services;
        }
    }
}
