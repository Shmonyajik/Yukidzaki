using AutoMapper;
using Babadzaki_DAL.Interfaces;
using Babadzaki_DAL.Repositories;
using Babadzaki_Domain.Models;
using Babadzaki_Serivces.Implementations;
using Babadzaki_Serivces.Interfaces;
using Babadzaki_Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Babadzaki
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<Token>, TokenRepository>();
            services.AddScoped<IBaseRepository<Email>, EmailRepository>();
            services.AddScoped<IBaseRepository<SeasonCollection>, SeasonCollectionRepository>();
            services.AddScoped<IBaseRepository<TokensFilters>, TokensFiltersRepository>();
            services.AddScoped<IBaseRepository<Filter>, FilterRepository>();

        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IMailService, CustomMailService>();
            services.AddScoped<IGalleryService, GalleryService>();





        }
    }
}