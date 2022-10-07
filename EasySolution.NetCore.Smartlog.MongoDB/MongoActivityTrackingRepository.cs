using EasySolution.NetCore.Shared.Commons;
using EasySolution.NetCore.Smartlog.Entities;
using EasySolution.NetCore.Smartlog.Repositories;
using MongoDB.Driver;

namespace EasySolution.NetCore.Smartlog.MongoDB
{
    public class CreateMongoActivityTrackingRepositoryOptions
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; } = "ActivityTracking";
        public bool UseObjectID { get; set; } = true;
    }
    public class MongoActivityTrackingRepository : IActivityTrackingRepositoryImplementor
    {
        MongoClient _client;
        IMongoDatabase _database;
        IMongoCollection<LogEventEntity> _collection;
        CreateMongoActivityTrackingRepositoryOptions _options;
        public MongoActivityTrackingRepository(CreateMongoActivityTrackingRepositoryOptions options)
        {
            _options = options;
            var settings = MongoClientSettings.FromConnectionString(options.ConnectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            _client = new MongoClient(settings);
            _database = _client.GetDatabase(options.DatabaseName);
            _collection = _database.GetCollection<LogEventEntity>(options.CollectionName);
        }

        public void Clear()
        {
            _database.DropCollection(_options.CollectionName);
            _collection = _database.GetCollection<LogEventEntity>(_options.CollectionName);
        }

        //public string beginActivity(string actor_id, string device_id, string activity_code)
        //{
        //    LogEventEntity e = new LogEventEntity { 
        //        _id = Guid.NewGuid().ToString("N"),
        //        actor_id = actor_id,
        //        device_id = device_id,
        //        activity_code = activity_code,
        //        at = DateTime.Now,
        //        event_id = "ActivityBegin",
        //        lv = Logging.LV_INFO,
        //        msg = $"Activity {activity_code} begin."
        //    };
        //    _collection.InsertOne(e);
        //    return e._id;
        //}

        //public string endActivity(string actor_id, string device_id, string activity_code, string activity_instance_id)
        //{
        //    LogEventEntity e = new LogEventEntity
        //    {
        //        _id = Guid.NewGuid().ToString("N"),
        //        tag = "",
        //        type = Logging.LOG_EVENT_TYPE_ACTIVITY,
        //        actor_id = actor_id,
        //        device_id = device_id,
        //        activity_code = activity_code,
        //        at = DateTime.Now,
        //        event_id = "ActivityEnded",
        //        lv = Logging.LV_INFO,
        //        msg = activity_instance_id
        //    };
        //    _collection.InsertOne(e);
        //    return e._id;
        //}

        public async Task<List<LogEventEntity>> getActivitiesAsync(string actor_id, string? device_id)
        {
            var filterBuilder = Builders<LogEventEntity>.Filter;
            var filter = filterBuilder.Eq("actor_id", actor_id)
                        & filterBuilder.Eq("device_id", device_id)
                        & filterBuilder.Eq("type", Logging.LOG_EVENT_TYPE_ACTIVITY)
                        & filterBuilder.Eq("event_id", Logging.ACTIVITY_START)
                        
                        ;
            var sort = Builders<LogEventEntity>.Sort.Descending("at");
            return await _collection.Find<LogEventEntity>(filter).Sort(sort).ToListAsync();
        }

        public async Task<List<LogEventEntity>> getActivityEventsAsync(string activity_id)
        {
            var filterBuilder = Builders<LogEventEntity>.Filter;
            var filter = filterBuilder.Eq("activity_code", activity_id)
                        ;
            return await _collection.Find<LogEventEntity>(filter).ToListAsync();
        }

        public string logActivityEvent(int level, string tag, string type, string actor_id, string device_id, string activity_code, 
            string event_id, string msg, Dictionary<string, object>? data)
        {
            LogEventEntity e = new LogEventEntity
            {
                _id = Guid.NewGuid().ToString("N"),
                tag = tag,
                type = type,
                actor_id = actor_id,
                device_id = device_id,
                activity_code = activity_code,
                at = DateTime.Now,
                event_id = event_id,
                lv = level,
                msg = msg,
                data = data
            };
            _collection.InsertOne(e);
            return e._id;
        }

        public void updateActivityResult(string actor_id, string device_id, string activity_code, string activity_instance_id, string result)
        {
            var filterBuilder = Builders<LogEventEntity>.Filter;
            var filter = filterBuilder.Eq("_id", activity_instance_id);
            LogEventEntity activityStartEvent = _collection.Find(filter).FirstOrDefault();
            if (activityStartEvent.data == null)
                activityStartEvent.data = new Dictionary<string, object>();

            activityStartEvent.data.Add("result", result);

            var changes = Builders<LogEventEntity>.Update.Set("data", activityStartEvent.data);
            _collection.UpdateOne(filter, changes);
        }
    }
}