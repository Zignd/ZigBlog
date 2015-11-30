using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZigBlog.Common.Database;
using ZigBlog.Models.Common;
using ZigBlog.Translations;

namespace ZigBlog.Models
{
    public class Comment : CustomModelBase
    {
        #region Fields
        
        private AppUser _commenter;
        private List<Comment> _children;

        #endregion

        #region Main Properties

        public int PostId { get; set; }

        public string CommenterId { get; set; }

        public int? ParentId { get; set; }

        [Required(ErrorMessageResourceName = "PostCommentContentValidationErrorRequired", ErrorMessageResourceType = typeof(Translation))]
        public string Content { get; set; }

        public string ParsedContent { get; set; }

        public List<string> LikersIds { get; set; } = new List<string>();

        public bool IsTopLevel { get; set; }

        public List<int> ChildrensIds { get; set; } = new List<int>();

        #endregion

        #region Support Property

        [BsonIgnore]
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

        [BsonIgnore]
        public List<Comment> Children
        {
            get
            {
                if (_children == null)
                {
                    var filter = Builders<Comment>.Filter.In(x => x.Id, ChildrensIds);
                    var task = ZigBlogDb.Comments.Find(filter).ToListAsync();

                    task.Wait();

                    _children = task.Result;
                }

                return _children;
            }
        }

        #endregion
    }
}