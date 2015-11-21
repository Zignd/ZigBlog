using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZigBlog.Common.Database;

namespace ZigBlog.Models.Common
{
    public class CustomModelBase
    {
        [BsonId(IdGenerator = typeof(IntIdGenerator))]
        public int Id { get; set; }
        public DateTime Created { get; set; }
    }
}