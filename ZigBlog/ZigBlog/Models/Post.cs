using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZigBlog.Common.Database;
using ZigBlog.Models.Common;

namespace ZigBlog.Models
{
    public class Post : CustomModelBase
    {
        #region Fields

        private AppUser _blogger;
        private List<Comment> _comments;
        
        #endregion

        #region Main Properties

        public string BloggerId { get; set; }
        public string TitleUrl { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ParsedContent { get; set; }
        public List<string> Likes { get; set; } = new List<string>();
        
        #endregion

        #region Support Properties

        [BsonIgnore]
        public AppUser Blogger
        {
            get
            {
                if (_blogger == null)
                {
                    var filter = Builders<AppUser>.Filter.Eq(x => x.Id, BloggerId);
                    var task = ZigBlogDb.Users.Find(filter).SingleOrDefaultAsync();

                    task.Wait();

                    _blogger = task.Result;
                }

                return _blogger;
            }
        }

        [BsonIgnore]
        public List<Comment> Comments
        {
            get
            {
                if (_comments == null)
                {
                    var filter = Builders<Comment>.Filter.Eq(x => x.PostId, Id);
                    var task = ZigBlogDb.Comments.Find(filter).ToListAsync();

                    task.Wait();

                    _comments = task.Result;
                }

                return _comments;
            }
        }

        #endregion
    }
}