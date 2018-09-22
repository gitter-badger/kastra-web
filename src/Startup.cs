using System;
using System.Reflection;
using System.Threading.Tasks;
using Kastra.Core;
using Kastra.Core.Business;
using Kastra.Core.Dto;
using Kastra.Web.Identity;
using Kastra.Web.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kastra.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            // Add options
            services.AddOptions();

            AppSettings appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            services.Configure<AppSettings>(Configuration);

            if (!String.IsNullOrEmpty(Configuration.GetConnectionString("DefaultConnection")))
            {
                // Add framework services.
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

                services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

				services.ConfigureApplicationCookie(options =>
                {
					options.Events =
                        new CookieAuthenticationEvents
                        {
                            OnRedirectToLogin = ctx =>
                            {
                                if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                                {
                                    ctx.Response.StatusCode = 401;
                                    return Task.FromResult<object>(null);
                                }

                                ctx.Response.Redirect(ctx.RedirectUri);
                                return Task.FromResult<object>(null);
                            }
                        };
                });
            }

            // Add dependencies
            DirectoryAssemblyLoader.LoadAllAssemblies(appSettings);
            int assembliesLength = KastraAssembliesContext.Instance.Assemblies.Count;

            if(assembliesLength > 0)
            {
                Assembly[] assemblies = new Assembly[assembliesLength];
                KastraAssembliesContext.Instance.Assemblies.Values.CopyTo(assemblies, 0);

                services.AddDependencyInjection(Configuration, assemblies);
            }

            services.AddKastraServices();

            // Add caching support
            services.AddMemoryCache();

            // Configure Kastra options with site configuration
            services.ConfigureKastraOptions();

            // Add Mvc
            var mvcBuilder = services.AddMvc();
            mvcBuilder.AddApplicationParts();

            // Add authorizations
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administration", policy => policy.RequireClaim("GlobalSettingsEdition"));
            });

            // Add gzip compression
            services.AddResponseCompression();

			// Add CORS
			if(appSettings.Cors.EnableCors)
			{
				CorsPolicyBuilder corsBuilder = new CorsPolicyBuilder();
                corsBuilder.AllowAnyHeader();
                corsBuilder.AllowAnyMethod();
                corsBuilder.AllowCredentials();

				if (appSettings.Cors.AllowAnyOrigin)
                {
					corsBuilder.AllowAnyOrigin();
                }
				else if (!String.IsNullOrEmpty(appSettings.Cors.Origins))
                {
					corsBuilder.WithOrigins(appSettings.Cors.Origins.Split(','));
                }

				services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", corsBuilder.Build());
                });  
			}
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IViewManager viewManager)
        {
			AppSettings appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();

            // Add log4net logs
            loggerFactory.AddLog4Net("log4net.config", Configuration.GetSection("Log4net"));

            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Page/Error");
            }

            // Update database
            if(appSettings.Configuration.EnableDatabaseUpdate)
            {
                UpdateDatabase(app);
            }

            app.UseBrowserLink();

            app.UseStaticFiles();

            app.UseModuleStaticFiles(
                viewManager, 
                appSettings.Configuration.ModuleDirectoryPath,
                $"/{Constants.SiteConfig.DefaultModuleResourcesPath}");

			if(appSettings.Cors.EnableCors)
			{
				app.UseCors("CorsPolicy");
			}     

            app.UseAuthentication();

            // Count visits
            app.UseMiddleware<VisitorCounterMiddleware>();

            app.UseMvc(routes =>
            {
				routes.MapRoute(name: "areaRoute",
				                template: "{area:exists}/{controller}/{action}/{id?}",
                                defaults: new { controller = "Page", action = "Home" });
				
                routes.AddDefaultRoutes("Page", "Admin/Module");

				routes.MapRoute(name: "adminRoute",
				                template: "{area:exists}/{*catchall}",
				                defaults: new { controller = "Home", action = "Index" });
            });
        }

        /// <summary>
        /// Updates the database.
        /// </summary>
        /// <param name="app">App.</param>
        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
