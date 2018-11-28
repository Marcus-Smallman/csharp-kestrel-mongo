using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Function
{
    public static class FunctionHandlerExtentions
    {
        /// <summary>
        /// A reference to an IMongoDatabase.
        /// </summary>
        private static IMongoDatabase Database;

        /// <summary>
        /// A reference to an IMongoCollection<BsonDocument>.
        /// </summary>
        private static IMongoCollection<BsonDocument> Collection;

        /// <summary>
        /// Sets up the MongoDB connection.
        /// </summary>
        /// <param name="functionHandler">An instance of a function handler.</param>
        public static void SetupConnection(this FunctionHandler functionHandler) {
            if (Database == null)
            {
                var client = new MongoClient(string.Format("mongodb://{0}", Environment.GetEnvironmentVariable("mongo_endpoint")));
                var databameName = Environment.GetEnvironmentVariable("mongo_database_name") ?? "default_database";
                Database = client.GetDatabase(databameName);

                var collectionName = Environment.GetEnvironmentVariable("mongo_collection_name") ?? "default_collection";
                Collection = Database.GetCollection<BsonDocument>(collectionName);
            }
        }

        /// <summary>
        /// Gets the Mongo database of this instance.
        /// </summary>
        /// <param name="functionHandler">This instance of a function handler.</param>
        /// <returns>An IMongoDatabase.</returns>
        public static IMongoDatabase GetDatabase(this FunctionHandler functionHandler)
        {
            return Database;
        }

        /// <summary>
        /// Gets the Mongo collection of this instance.
        /// </summary>
        /// <param name="functionHandler">This instance of a function handler.</param>
        /// <returns>An IMongoCollection<BsonDocument>.</returns>
        public static IMongoCollection<BsonDocument> GetCollection(this FunctionHandler functionHandler)
        {
            return Collection;
        }
    }
}
