using AspNet.Identity.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZigBlog.Models
{
    public class AppUser : IdentityUser
    {
        public string UserNameLower
        {
            get { return UserName.ToLower(); }
            set { /* MongoDB Driver is forcing me to have a property with both get and set methods in order to map it to the database. Just ignore this empty set block. */ }
        }

        public DateTime Created { get; set; }
    }
}