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

            // Cache
            if (model.CacheActivated)
            {
                _cacheEngine.EnableCache();
            }
            else
            {
                _cacheEngine.DisableCache();
                _cacheEngine.ClearAllCache();
            }

            _parameterManager.SaveSiteConfiguration(conf);

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
    }
}
