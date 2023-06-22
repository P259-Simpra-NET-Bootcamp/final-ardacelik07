using ApiService.Helpers;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace ApiService.Extensions
{
    public static class MapperExtension
    {
        public static void AddMapperExtension(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            services.AddSingleton(config.CreateMapper());
        }
    }
}
