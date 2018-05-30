namespace Kastra.Web.API.Models.Page
{
    public class PageModel
    {
        public int Id { get; set;  }
        public string Name { get; set; }
        public string KeyName { get; set; }
		public int TemplateId { get; set; }
		public string MetaKeywords { get; set; }
		public string MetaDescription { get; set; }
		public string MetaRobot { get; set; }
    }
}
