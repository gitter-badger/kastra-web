﻿using System.Collections.Generic;

namespace Kastra.Web.API.Models.User
{
	public class UserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
		public IList<string> Roles { get; set; }
    }
}
