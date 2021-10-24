using FreeCourse.Frontends.Web.Handler;
using FreeCourse.Frontends.Web.Models;
using FreeCourse.Frontends.Web.Services.Abstracts;
using FreeCourse.Frontends.Web.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FreeCourse.Frontends.Web.Extensions
{
    public static class ServicesExtension
    {
        public static void AddHttpClientServices(this IServiceCollection services, IConfiguration Configuration)
        {
            var serviceApiSettings = Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();

            services.AddHttpClient<IIdentityService, IdentityService>();
            services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();

            services.AddHttpClient<ICatalogService, CatalogService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IPhotoStockService, PhotoStockService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.PhotoStock.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IBasketService, BasketService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Basket.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();
        }

    }
}
