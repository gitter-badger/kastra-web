using System.Diagnostics;
using System.Reflection;
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

        public SiteConfigurationController(CacheEngine cacheEngine, IParameterManager parametermanager)
		{
            _cacheEngine = cacheEngine;
			_parameterManager = parametermanager;
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

            return Json(model);
		}

        [HttpPost]
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
        public IActionResult Restart([FromServices] IApplicationLifetime applicationLifetime)
        {
            applicationLifetime.StopApplication();

            return Ok();
        }

        /// <summary>
        /// Gets the application versions.
        /// </summary>
        /// <returns>The application versions.</returns>
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

            return Json(new { ApplicationVersion = applicationVersion, CoreVersion = kastraVersion });
        }
    }
}
