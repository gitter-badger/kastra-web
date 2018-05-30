using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Kastra.Web.Identity
{
    public class ApplicationRole : IdentityRole
    {
        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<IdentityRoleClaim<string>> Claims { get; } = new List<IdentityRoleClaim<string>>();
    }
}
