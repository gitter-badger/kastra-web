using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Kastra.Core;
using Kastra.Core.Business;
using Kastra.Core.Dto;
using Kastra.Core.Services;
using Kastra.Web.Areas.API.Models.SiteConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Kastra.Web.API.Controllers
{
	[Area("Api")]
    [Authorize("Administration")]
	public class SiteConfigurationController : Controller
    {
        private readonly CacheEngine _cacheEngine;
		private readonly IParameterManager _parameterManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SiteConfigurationController(CacheEngine cacheEngine, IParameterManager parametermanager, IHostingEnvironment hostingEnvironment)
		{
            _cacheEngine = cacheEngine;
			_parameterManager = parametermanager;
            _hostingEnvironment = hostingEnvironment;
		}

		[HttpGet]
		public IActionResult Get()
		{
            SiteConfigurationModel model = null;
			SiteConfigurationInfo configuration = _parameterManager.GetSiteConfiguration();

			if(configuration == null)
			{
				return NotFound();
			}

            // Get themes list
            DirectoryInfo themeDirectory = new DirectoryInfo(Path.Combine(_hostingEnvironment.WebRootPath, "themes"));
            DirectoryInfo[] themes = themeDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly);

            model = new SiteConfigurationModel();
            model.Title = configuration.Title;
            model.Description = configuration.Description;
            model.HostUrl = configuration.HostUrl;
            model.CacheActivated = configuration.CacheActivated;
            model.SmtpHost = configuration.SmtpHost;
            model.SmtpPort = configuration.SmtpPort.ToString();
            model.SmtpCredentialsUser = configuration.SmtpCredentialsUser;
            model.SmtpCredentialsPassword = configuration.SmtpCredentialsPassword;
            model.SmtpEnableSsl = configuration.SmtpEnableSsl;
            model.EmailSender = configuration.EmailSender;
            model.RequireConfirmedEmail = configuration.RequireConfirmedEmail;
            model.Theme = configuration.Theme;
            model.ThemeList = themes?.Select(t => t.Name)?.OrderBy(t => t)?.ToArray() ?? new string[] { Constants.SiteConfig.DefaultTheme };

            return Json(model);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([FromBody]SiteConfigurationModel model)
        {
            SiteConfigurationInfo conf = new SiteConfigurationInfo();
            conf.Title = model.Title;
            conf.Description = model.Description;
            conf.HostUrl = model.HostUrl;
            conf.CacheActivated = model.CacheActivated;
            conf.SmtpHost = model.SmtpHost;
            conf.SmtpPort = int.Parse(model.SmtpPort);
            conf.SmtpCredentialsUser = model.SmtpCredentialsUser;
            conf.SmtpCredentialsPassword = model.SmtpCredentialsPassword;
            conf.SmtpEnableSsl = model.SmtpEnableSsl;
            conf.EmailSender = model.EmailSender;
            conf.RequireConfirmedEmail = model.RequireConfirmedEmail;
            conf.Theme = model.Theme;

            // Cache
            if (model.CacheActivated)
            {
                _cacheEngine.EnableCache();
                _cacheEngine.ClearSiteConfig();
            }
            else
            {
                _cacheEngine.DisableCache();
            }

            _parameterManager.SaveSiteConfiguration(conf);
            _cacheEngine.ClearAllCache();

            return Ok();
        }

        /// <summary>
        /// Restart the website.
        /// </summary>
        /// <returns></returns>
        /// <param name="applicationLifetime">Application lifetime.</param>
        [HttpGet]
        public IActionResult Restart([FromServices] IApplicationLifetime applicationLifetime)
        {
            throw new Exception("");
            applicationLifetime.StopApplication();

            return Ok();
        }

        /// <summary>
        /// Gets the application versions.
        /// </summary>
        /// <returns>The application versions.</returns>
        [HttpGet]
        public IActionResult GetApplicationVersions()
        {
            // Get application version
            Assembly assembly = Assembly.GetExecutingAssembly();
            string applicationVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                        .InformationalVersion;

            // Get the Kastra core version
            string kastraVersion = typeof(Configuration).Assembly
                                        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                        .InformationalVersion;

            // Get the dotnet core version
            string dotnetCoreVersion = Assembly.GetEntryAssembly()?
                                    .GetCustomAttribute<TargetFrameworkAttribute>()?
                                    .FrameworkName;

            // Get the OS
            string osPlatform = RuntimeInformation.OSDescription;

            var stats = new
            {
                ApplicationVersion = applicationVersion, 
                CoreVersion = kastraVersion,
                OsPlatform = osPlatform,
                AspDotnetVersion = dotnetCoreVersion
            };

            return Json(stats);
        }
    }
}
