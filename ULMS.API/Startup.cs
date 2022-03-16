using Domain.Configurations;
using Domain.Multitenancy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.Implementations.Multitenancy;
using Persistence.Integrations.Email;
using Persistence.Integrations.MailKitModels;
using System;
using System.Text;
using ULMS.ActionResults;
using ULMS.Filters;
using ULMS.Infrastructure.IOC.Extensions;

namespace ULMS.API
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
            services.AddFileExportService()
            .AddRepositories()
            .AddServices()
            .AddCustomIdentity()
            .AddLogging();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JwtTokenSettings:TokenIssuer"],
                    ValidAudience = Configuration["JwtTokenSettings:TokenIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtTokenSettings:TokenKey"]))
                };
                options.RequireHttpsMetadata = false;
            });
            services.Configure<DataProtectionTokenProviderOptions>(o =>
              o.TokenLifespan = TimeSpan.FromHours(3));
            services.AddMvc(options =>
            {
                options.Filters.Add<RequestLoggingFilter>();
                options.Filters.Add<HttpGlobalExceptionFilter>();
            })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context => new ValidationFailedResult(context.ModelState);
                });
            services.AddMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers();

            //services.AddHttpContextAccessor();

           
            services.AddTransient<ITenantService, TenantService>();

            //check
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.Configure<SetModuleToTakenConfig>(Configuration.GetSection("SetModuleToTakenConfig"));
            //check

            services.Configure<TenantSetting>(Configuration.GetSection(nameof(TenantSetting)));
            services.AddAndMigrateTenantDatabases(Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ULMS.API", Version = "v1" });

            });
            services.AddBackgroundTasks();
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
            }

            app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "IDH API");
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
