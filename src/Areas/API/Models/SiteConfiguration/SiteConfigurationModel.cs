namespace Kastra.Web.Areas.API.Models.SiteConfiguration
{
    public class SiteConfigurationModel
    {
        public string Title { get; set; }
        public string HostUrl { get; set; }
        public string Description { get; set; }
        public bool CacheActivated { get; set; }
        public string SmtpHost { get; set; }
        public string SmtpPort { get; set; }
        public string SmtpCredentialsUser { get; set; }
        public string SmtpCredentialsPassword { get; set; }
        public bool SmtpEnableSsl { get; set; }
        public string EmailSender { get; set; }
        public bool RequireConfirmedEmail { get; set; }
    }
}
