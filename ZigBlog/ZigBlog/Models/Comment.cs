using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZigBlog.Common;
using ZigBlog.Models.Base;

namespace ZigBlog.Models
{
    public class Comment : ModelBase
    {
        #region Fields
        
        private User _commenter;

        #endregion

        #region Main Properties

        public int PostId { get; set; }
        public int CommenterId { get; set; }
        public string Content { get; set; }
        public List<int> Likes { get; set; }

        #endregion

        #region Support Property

        public User Commenter
        {
            get
            {
                if (_commenter == null)
                {
                    var filter = Builders<User>.Filter.Eq(u => u.Id, CommenterId);
                    var task = ZigBlogDb.Users.Find(filter).SingleOrDefaultAsync();

                    task.Wait();

                    _commenter = task.Result;
                }

                return _commenter;
            }
        }

        #endregion
    }
}