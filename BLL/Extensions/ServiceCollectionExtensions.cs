using AutoMapper;
using DAL;
using DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BLL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBll(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork>(uow => new UnitOfWork());
            services.AddAutoMapper(cfg => cfg.AddProfile<AutoMappingProfile>(), Assembly.GetExecutingAssembly());
        }
    }
}
