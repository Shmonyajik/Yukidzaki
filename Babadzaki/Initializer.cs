using AutoMapper;
using Babadzaki_DAL.Interfaces;
using Babadzaki_DAL.Repositories;
using Babadzaki_Domain.Models;
using Babadzaki_Serivces.Implementations;
using Babadzaki_Serivces.Interfaces;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Babadzaki
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<Token>, TokenRepository>();
        
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
         
        }
    }
}