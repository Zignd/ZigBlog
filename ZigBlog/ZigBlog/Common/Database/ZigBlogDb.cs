using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZigBlog.Models;

namespace ZigBlog.Common.Database
{
    public class ZigBlogDb
    {
        private static MongoClient _client;
        private static IMongoDatabase _database;
        private static IMongoCollection<AppUser> _users;
        private static IMongoCollection<AppRole> _roles;
        private static IMongoCollection<Post> _posts;
        private static IMongoCollection<Comment> _comments;

        private static Parameters _parameters;

        public static MongoClient Client
        {
            get
            {
                // TODO: Change it so the MongoDB url is retrieved from a configuration file.
                if (_client == null)
                    _client = new MongoClient("mongodb://localhost:27017");

                return _client;
            }
        }

        public static IMongoDatabase Database
        {
            get
            {
                // TODO: Change it so the database name is retrieved from a configuration file.
                if (_database == null)
                    _database = Client.GetDatabase("ZigBlog");

                return _database;
            }
        }

        public static IMongoCollection<AppUser> Users
        {
            get
            {
                // TODO: Change it so the collection name is retrieved from a configuration file.
                if (_users == null)
                    _users = Database.GetCollection<AppUser>("Users");

                return _users;
            }
        }

        public static IMongoCollection<AppRole> Roles
        {
            get
            {
                // TODO: Change it so the collection name is retrieved from a configuration file.
                if (_roles == null)
                    _roles = Database.GetCollection<AppRole>("Roles");

                return _roles;
            }
        }

        public static IMongoCollection<Post> Posts
        {
            get
            {
                // TODO: Change it so the collection name is retrieved from a configuration file.
                if (_posts == null)
                    _posts = Database.GetCollection<Post>("Posts");

                return _posts;
            }
        }

        public static IMongoCollection<Comment> Comments
        {
            get
            {
                // TODO: Change it so the collection name is retrieved from a configuration file.
                if (_comments == null)
                    _comments = Database.GetCollection<Comment>("Comments");
                
                return _comments;
            }
        }

        public static Parameters Parameters
        {
            get
            {
                if (_parameters == null)
                {
                    // TODO: Change it so the collection name is retrieved from a configuration file.
                    var collection = Database.GetCollection<Parameters>("Parameters");

                    var filter = Builders<Parameters>.Filter.Eq(p => p.Id, 1);
                    var task = collection.Find(filter).FirstAsync();

                    task.Wait();

                    _parameters = task.Result;
                }

                return _parameters;
            }
        }
    }
}