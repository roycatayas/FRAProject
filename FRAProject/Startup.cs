﻿using System;
using FRA.Data.Abstract;
using FRA.Data.Models;
using FRA.IdentityProvider.Entities;
using FRA.IdentityProvider.Stores;
using FRA.Repo.Category;
using FRA.Repo.Risk;
using FRA.Repo.Section;
using FRA.Repo.User;
using FRA.Service.Abstract;
using FRA.Service.Concrete;
using FRA.Service.Models;
using FRA.Web.Infrastructure.Identity;
using FRA.Web.Infrastructure.Services;
using FRA.Web.Infrastructure.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Serialization;

namespace FRA.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder().SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add all dependencies here
            services.AddScoped<ITableDataRepository<Catergory>, CategoryRepository>();
            services.AddScoped<ITableDataRepository<Section>, SectionRepository>();
            services.AddScoped<ITableDataRepository<RiskTemplate>, RiskTemplateRepository>();

            // Add and configure the default identity system that will be used in the application.
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddUserManager<ApplicationUserManager>()
                .AddRoleManager<ApplicationRoleManager>()
                .AddSignInManager<ApplicationSignInManager>()
                .AddDefaultTokenProviders();

            // Add support for non-distributed memory cache in the application.
            services.AddMemoryCache();

            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;
                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 6;
                options.Lockout.AllowedForNewUsers = true;
                // User settings.
                options.User.RequireUniqueEmail = true;
            });

            // Configure cookie settings.
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = false;
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/log-off";
                options.AccessDeniedPath = "/account/login";
                options.SlidingExpiration = true;
            });

            // Map appsettings.json file elements to a strongly typed class.
            services.Configure<AppSettings>(Configuration);
            // Add services required for using options.
            services.AddOptions();

            // Get the connection string from appsettings.json file.
            string connectionString = Configuration.GetConnectionString("FRADbConnection");
            // Configure custom services to be used by the framework.
            services.AddTransient<IDatabaseConnectionService>(e => new DatabaseConnectionService(connectionString));
            services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, RoleStore>();
            services.AddTransient<IEmailSender, MessageServices>();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
            services.AddSingleton<ICacheManagerService, CacheManagerService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRiskAssessmentRepository, RiskAssessmentRepository>();

            services.AddTransient<IEmailService>(e => new EmailService(new SmtpSettings
            {
                From = Configuration["SmtpSettings:From"],
                Host = Configuration["SmtpSettings:Host"],
                Port = int.Parse(Configuration["SmtpSettings:Port"]),
                SenderName = Configuration["SmtpSettings:SenderName"],
                LocalDomain = Configuration["SmtpSettings:LocalDomain"],
                Password = Configuration["SmtpSettings:Password"],
                UserName = Configuration["SmtpSettings:UserName"]
            }));

            //services.Configure<RazorViewEngineOptions>(option =>
            //{
            //    CustomViewLocator expandViews = new CustomViewLocator();
            //    option.ViewLocationExpanders.Add(expandViews);
            //});

            // Add and configure MVC services.
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(setupAction =>
                {
                    // Configure the contract resolver that is used when serializing .NET objects to JSON and vice versa.
                    setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (hostingEnvironment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
            }
            else
            {
                applicationBuilder.UseExceptionHandler("/error/index/500");
            }

            applicationBuilder.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = staticFileResponseContext =>
                {
                    // Configure caching for static files. Files will be cached for 365 days and duration must be provided in seconds.
                    const int maxAge = 365 * 24 * 3600;
                    staticFileResponseContext.Context.Response.Headers[HeaderNames.CacheControl] = $"public,max-age={maxAge}";
                }
            });

            //applicationBuilder.UseIdentity();
            applicationBuilder.UseAuthentication();
            applicationBuilder.UseStatusCodePagesWithRedirects("/error/index?errorCode={0}");            

            applicationBuilder.UseMvc(routes =>
            {                
                routes.MapRoute("administrationAreaRoute", "{area:exists}/{controller=home}/{action=index}/{id?}");
                routes.MapRoute("defaultRoute", "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
