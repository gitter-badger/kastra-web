using System;
using System.Collections.Generic;

namespace Kastra.Web.API.Models.Module
{
    public class ModuleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PageId { get; set;  }
        public int DefinitionId { get; set; }
        public int PlaceId { get; set; }
        public string PageName { get; set; }
		public IList<int> Permissions { get; set; }
    }
}
