using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trash.Data.DataContext;
using Trash.Data.Repositories;
using Trash.Models.ContextModels;
using Trash.Services;

namespace Trash.Configurations
{
    public static class ServiceExtensions
    {

        public static void AddJwtAuthentication(this IServiceCollection services,string secretkey)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var Secretkey = Encoding.Default.GetBytes(secretkey);

                var ValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    RequireSignedTokens = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Secretkey),

                    ValidateAudience = true, //default : false
                    ValidAudience = "TrashServer",

                    ValidateIssuer = true, //default : false
                    ValidIssuer = "TrashServer",

                    RequireExpirationTime = true,
                    ValidateLifetime = true
                };
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = ValidationParameters;
                options.SaveToken = true;
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                        //logger.LogError("Authentication failed.", context.Exception);

                        if (context.Exception != null)
                        {
                            return Task.FromException(new UnauthorizedAccessException("The Token Does not accepted"));
                        }
                            

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {

                        
                    },
                    OnChallenge = context =>
                    {
                        return Task.FromException(new UnauthorizedAccessException("The Token Does not accepted"));
                    }
                };
            });
        }

        public static void AddRequirmentServices(this IServiceCollection services)
        {
            services.AddScoped<Repository<User>>();
            services.AddScoped<Repository<Trash.Models.ContextModels.Trash>>();
            services.AddScoped<Repository<Order>>();
            services.AddScoped<CommodityRepo>();
            services.AddScoped<ServiceRepo>();
            services.AddScoped<JwtService>();
            services.AddScoped<AuthService>();
            services.AddScoped<TrashService>();
            services.AddScoped<UserOrderService>();
            services.AddScoped<UserUsageService>();
            services.AddScoped<TrashTypeService>();
        }

        public static void AddDataBase(this IServiceCollection services,string ConnectionString)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(ConnectionString);
            });
        }
    }
}
