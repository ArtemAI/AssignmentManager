﻿using System.Reflection;
using AutoMapper;
using DAL;
using DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Extensions
{
    /// <summary>
    /// Adds Unit of work and AutoMapper services to IServiceCollection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static void AddBll(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork>(uow => new UnitOfWork());
            services.AddAutoMapper(cfg => cfg.AddProfile<AutoMappingProfile>(), Assembly.GetExecutingAssembly());
        }
    }
}