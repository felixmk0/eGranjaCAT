﻿using eGranjaCAT.Infrastructure.Data;
using eGranjaCAT.Infrastructure.Extensions.Cron;
using eGranjaCAT.Infrastructure.Extensions.Cron.Jobs;
using eGranjaCAT.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace eGranjaCAT.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddCronJob<BackupJob>(options =>
            {
                options.TimeZone = TimeZoneInfo.Local;
                options.CronExpression = configuration["CronExpressions:BackupJob"]!;
            });

            return services;
        }
    }
}