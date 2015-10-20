using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ZigBlog.Common;
using ZigBlog.Models.Base;

namespace ZigBlog.Models
{
    public class Post : ModelBase
    {
        #region Fields

        private User _blogger;
        private List<Comment> _comments;
        
        #endregion

        #region Main Properties

        public int BloggerId { get; set; }
        public string TitleUrl { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<int> Likes { get; set; }
        
        #endregion

        #region Support Properties

        [BsonIgnore]
        public User Blogger
        {
            get
            {
                if (_blogger == null)
                {
                    var filter = Builders<User>.Filter.Eq(u => u.Id, BloggerId);
                    var task = ZigBlogDb.Users.Find(filter).FirstOrDefaultAsync();

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
                    var filter = Builders<Comment>.Filter.Eq(c => c.PostId, Id);
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