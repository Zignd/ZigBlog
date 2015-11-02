using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZigBlog.Models.Common;

namespace ZigBlog.Models
{
    public class User : ModelBase
    {
        #region Fields

        public string _username;

        #endregion

        #region Main Properties

        public string UsernameUpper { get; set; }

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                UsernameUpper = _username.ToUpper();
            }
        }

        public string Password { get; set; }

        public UserRole Role { get; set; }

        public string EmailAddress { get; set; }

        public bool IsActivated { get; set; }

        #endregion
    }

    public enum UserRole
    {
        Admin,
        Blogger,
        Commenter
    }
}