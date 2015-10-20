using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZigBlog.Common
{
    public class IntIdGenerator : IIdGenerator
    {

        public object GenerateId(object container, object document)
        {
            try
            {
                // TODO: Find a way to make this id generation not to depend on ZigBlogDb.
                CollectionNamespace collectionNamespace = ((dynamic)container).CollectionNamespace;

                var sort = Builders<BsonDocument>.Sort.Descending(b => b["_id"]);
                var task = ZigBlogDb.Database.GetCollection<BsonDocument>(collectionNamespace.CollectionName).Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
                task.Wait();
                var result = task.Result;

                if (result == null)
                    return 1;

                if (result["_id"].IsDouble)
                    return Convert.ToInt32(result["_id"].AsDouble + 1);
                else
                    return Convert.ToInt32(result["_id"].AsInt32 + 1);
            }
            catch (Exception ex)
            {
                throw new Exception("It was not possible to generate an integer id. An unexpected error occurred in the IntIdGenerator.", ex);
            }
        }

        public bool IsEmpty(object id)
        {
            return id == null || (id.GetType().Equals(typeof(Int32)) && (int)id == 0);
        }
    }
}