using System.Collections.Generic;

namespace Kastra.Web.API.Models.Role
{
	public class RoleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
		public IList<int> Permissions { get; set; }
    }
}
