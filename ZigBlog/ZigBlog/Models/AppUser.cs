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
            get
            {
                return UserName.ToLower();
            }   
        }

        public DateTime Created { get; set; }
    }
}