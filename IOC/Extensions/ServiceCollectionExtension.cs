using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Multitenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Implementations.Repositories;
using Persistence.Implementations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULMS.Infrastructure.IOC.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddAndMigrateTenantDatabases(this IServiceCollection services, IConfiguration config)
        {
            var options = services.GetOptions<TenantSetting>(nameof(TenantSetting));
            var defaultConnectionString = options.Defaults?.ConnectionString;
            var defaultDbProvider = options.Defaults?.DBProvider;
            if (defaultDbProvider.ToLower() == "mssql")
            {
                services.AddDbContext<ApplicationContext>(m => m.UseSqlServer(defaultConnectionString));
            }
            var tenants = options.Tenants;
            foreach (var tenant in tenants)
            {
                string connectionString;
                if (string.IsNullOrEmpty(tenant.ConnectionString))
                {
                    connectionString = defaultConnectionString;
                }
                else
                {
                    connectionString = tenant.ConnectionString;
                }
                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                dbContext.Database.SetConnectionString(connectionString);
                if (dbContext.Database.GetMigrations().Count() > 0)
                {
                    dbContext.Database.Migrate();
                }
            }
            return services;
        }
        public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(sectionName);
            var options = new T();
            section.Bind(options);
            return options;
        }
    /*    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationContext>(options =>
                 options.UseSqlServer(connectionString));
            return services;
        }*/

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IInstructorService, InstructorService>();
            services.AddScoped<ILearnerService, LearnerService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<ITopicService, TopicService>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IInstructorRepository, InstructorRepository>();
            services.AddScoped<ILearnerRepository, LearnerRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();

            return services;
        }

        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
           /* services.AddScoped<IUserStore<User>, UserStore>();
            services.AddScoped<IRoleStore<Role>, RoleStore>();
            services.AddIdentity<User, Role>()
                .AddDefaultTokenProviders();
            services.AddScoped<IIdentityService, IdentityService>();*/
            /* services.AddScoped<IEmailService, EmailService>();
             services.AddScoped<IMailSender, MailSender>();*/
            return services;
        }
    }
}
