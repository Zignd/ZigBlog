using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZigBlog.Common.Database;
using ZigBlog.Models.Common;

namespace ZigBlog.Models
{
    public class Comment : CustomModelBase
    {
        #region Fields
        
        private AppUser _commenter;

        #endregion

        #region Main Properties

        public int PostId { get; set; }
        public string CommenterId { get; set; }
        public string Content { get; set; }
        public List<int> Likes { get; set; }

        #endregion

        #region Support Property

        public AppUser Commenter
        {
            get
            {
                if (_commenter == null)
                {
                    var filter = Builders<AppUser>.Filter.Eq(x => x.Id, CommenterId);
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