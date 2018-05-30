using System;
namespace Kastra.Web.Areas.API.Models.SiteConfiguration
{
    public class SiteConfigurationModel
    {
        public string Title { get; set; }
        public string HostUrl { get; set; }
        public string Description { get; set; }
        public bool CacheActivated { get; set; }
    }
}
