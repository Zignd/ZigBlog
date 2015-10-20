using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZigBlog.Common;
using ZigBlog.Models.Base;

namespace ZigBlog.Models
{
    public class User : ModelBase
    {
        #region Main Properties

        public string UsernameUpper { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public string Email { get; set; }
        public bool IsActivated { get; set; }
        public int Points { get; set; }

        #endregion

        #region Support Properties

        

        #endregion
    }

    public enum UserRole
    {
        Admin,
        Blogger,
        Commenter
    }
}