using EasySolution.NetCore.Storage.StorageProviders;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System.Dynamic;
using System.Text.Json;

namespace EasySolution.NetCore.Storage.MongoDB
{
    public class MongoDocumentStorageProvider<TDocument> :  IDocumentStorageProvider<TDocument>
    {
        MongoClient _client;
        IMongoDatabase _database;
        IMongoCollection<BsonDocument> _collection;
        CreateMongoDocumentStorageOptions _options;
        public MongoDocumentStorageProvider(CreateMongoDocumentStorageOptions options)
        {
            _options = options;
            var settings = MongoClientSettings.FromConnectionString(options.ConnectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            _client = new MongoClient(settings);
            _database = _client.GetDatabase(options.DatabaseName);
            _collection = _database.GetCollection<BsonDocument>(options.CollectionName);
        }


        public AddDocumentResult Add(TDocument doc)
        {
            BsonDocument bsonDoc = doc.ToBsonDocument();
            bsonDoc["_id"] = Guid.NewGuid().ToString("N");
            _collection.InsertOne(bsonDoc);
            return new AddDocumentResult { 
                DocumentId = $"{bsonDoc["_id"]}"
            };
        }

        public DeleteDocumentResult DeleteAll()
        {
            int nDocs = (int)_collection.EstimatedDocumentCount();
            var result = new DeleteDocumentResult { 
                matchedCount = nDocs,
                deletedCount = nDocs
            };
            _database.DropCollection(_options.CollectionName);
            _collection = _database.GetCollection<BsonDocument>(_options.CollectionName);
            return result;
        }

        public DeleteDocumentResult DeleteMany(dynamic query)
        {
            throw new Exception("Not implemented");
        }

        public bool DeleteOne(string id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            DeleteResult result = _collection.DeleteOne(filter);
            return result != null && result.DeletedCount == 1;
        }

        public DocumentRecord<TDocument>? Get(string id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            
            BsonDocument result = _collection.Find(filter).FirstOrDefault();
            if (result == null) return null;
            Console.WriteLine(result);

            string jsonStr = result.ToJson( new JsonWriterSettings { 
                OutputMode = JsonOutputMode.Strict
            } );
            if (typeof(TDocument) == typeof(ExpandoObject))
            {
                return new DocumentRecord<TDocument> { source = (dynamic)JsonSerializer.Deserialize<ExpandoObject>(jsonStr), _id = id };
            }
            return new DocumentRecord<TDocument> { source = JsonSerializer.Deserialize<TDocument>(jsonStr), _id = id };
        }

        public UpdateDocumentResult UpdateChanges(string id, dynamic changes)
        {
            throw new NotImplementedException();
        }
    }
}