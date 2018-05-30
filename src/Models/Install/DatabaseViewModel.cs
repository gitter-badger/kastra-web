namespace Kastra.Web.Models.Install
{
    public class DatabaseViewModel
    {
        public string DatabaseServer { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseLogin { get; set; }
        public string DatabasePassword { get; set; }
        public bool IntegratedSecurity { get; set; }
    }
}