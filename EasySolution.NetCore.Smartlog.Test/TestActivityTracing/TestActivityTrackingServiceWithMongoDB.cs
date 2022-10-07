using EasySolution.NetCore.CredVault;
using EasySolution.NetCore.Shared.Commons;
using EasySolution.NetCore.Smartlog.Contexts;
using EasySolution.NetCore.Smartlog.Entities;
using EasySolution.NetCore.Smartlog.MongoDB;
using EasySolution.NetCore.Smartlog.Repositories;
using EasySolution.NetCore.Smartlog.Services;
using System.Text.Json;

namespace EasySolution.NetCore.Smartlog.Test.TestActivityTracing
{
    [TestClass]
    public class TestActivityTrackingServiceWithMongoDB
    {
        const string TEST_ACTOR_ID = "tester-x";
        const string TEST_DEVICE_ID = "device-x";
        SmartlogService slService;

        Dictionary<string, object> validData = new Dictionary<string, object>();
        Dictionary<string, object> invalidData = new Dictionary<string, object>();
        [TestInitialize]
        public void Setup()
        {
            var credVault = RedisCredVault.Localhost;
            ActivityTrackingRepository repo = new ActivityTrackingRepository(
                new MongoActivityTrackingRepository(
                    new CreateMongoActivityTrackingRepositoryOptions {
                        ConnectionString = credVault.GetString("my-mongo-cloud-connection-string")!,
                        DatabaseName = "Sandbox",
                        UseObjectID = false
                    }
                    )
                );
            slService = new SmartlogService(repo);

            validData.Add("ext_id", "D0");
            validData.Add("payload", new { BatV = 3.5 });

            invalidData.Add("ext_id", "D0");
            invalidData.Add("command", "text");
        }

        [TestMethod]
        public async Task TestOneActionSuccess()
        {
            slService.Clear();
            string ACTIVITY_CODE_ADD_BOOK = "AddBook";
            string STEPID_BASIC_INFO = "AddBasicInfo";
            string STEPID_UPLOAD_COVER = "UploadCover";
            ActivityTrackingLogContext context = slService.beginActivity(TEST_ACTOR_ID, TEST_DEVICE_ID, ACTIVITY_CODE_ADD_BOOK);
            context.onStepPassed(STEPID_BASIC_INFO, "user has entered basic information");

            var data = new Dictionary<string, object>();
            data.Add("upload_cover_file_id", "1234");
            context.onStepPassed(STEPID_UPLOAD_COVER, "user has uploaded book cover image", data);
            context.endActivity(Logging.ACTIVITY_RESULT_SUCCESS);

            List<LogEventEntity> activities = await slService.getActivitiesAsync(TEST_ACTOR_ID, TEST_DEVICE_ID);
            Assert.AreEqual(1, activities.Count());
            Assert.AreEqual(ACTIVITY_CODE_ADD_BOOK, activities[0].activity_code);
            Assert.AreEqual(Logging.ACTIVITY_RESULT_SUCCESS, activities[0].data["result"]);
        }

        [TestMethod]
        public async Task TestOneActionFailed()
        {
            slService.Clear();
            string ACTIVITY_CODE_ADD_BOOK = "AddBook";
            string STEPID_BASIC_INFO = "AddBasicInfo";
            string STEPID_UPLOAD_COVER = "UploadCover";
            ActivityTrackingLogContext context = slService.beginActivity(TEST_ACTOR_ID, TEST_DEVICE_ID, ACTIVITY_CODE_ADD_BOOK);
            context.onStepPassed(STEPID_BASIC_INFO, "user has entered basic information");

            var data = new Dictionary<string, object>();
            data.Add("Exception", new Exception("File service not available"));
            context.onStepFailed(STEPID_UPLOAD_COVER, "Exception occur while user upload book cover image", data);
            context.endActivity(Logging.ACTIVITY_RESULT_FAILED);

            List<LogEventEntity> activities = await slService.getActivitiesAsync(TEST_ACTOR_ID, TEST_DEVICE_ID);
            Assert.AreEqual(1, activities.Count());
            Assert.AreEqual(ACTIVITY_CODE_ADD_BOOK, activities[0].activity_code);
            Assert.AreEqual(Logging.ACTIVITY_RESULT_FAILED, activities[0].data["result"]);
        }

        [TestMethod]
        public async Task TestSchedule()
        {
            slService.Clear();
            string ACTIVITY_CODE_UPDATE_DEVICE_REALTIME_DATA = "Device.Update.RealtimeData";
            string STEPID_PARSING_DATA = "RealtimeData.Parsing";
            string STEPID_SAVING = "RealtimeData.Saving";
            ActivityTrackingLogContext context = slService.beginActivity(TEST_ACTOR_ID, TEST_DEVICE_ID, ACTIVITY_CODE_UPDATE_DEVICE_REALTIME_DATA);

            Thread.Sleep(1);
            context.dataParsingSuccess(STEPID_PARSING_DATA, $"timepoint {DateTime.Now.ToShortTimeString()}", validData);
            context.dataSavingSuccess(STEPID_SAVING, $"timepoint {DateTime.Now.ToShortTimeString()}", validData);
            context.onStepPassed(STEPID_PARSING_DATA, "parsed data of device: D0");
            context.onStepPassed(STEPID_SAVING, "saved data of device: D0");

            Thread.Sleep(2);
            context.dataParsingFailed(STEPID_PARSING_DATA, $"timepoint {DateTime.Now.ToShortTimeString()}", invalidData);
            context.onStepFailed(STEPID_PARSING_DATA, "Invalid data format of device: D0");
            context.endActivity(Logging.ACTIVITY_RESULT_CLOSED);

            List<LogEventEntity> activities = await slService.getActivitiesAsync(TEST_ACTOR_ID, TEST_DEVICE_ID);
            Assert.AreEqual(1, activities.Count());
            Assert.AreEqual(ACTIVITY_CODE_UPDATE_DEVICE_REALTIME_DATA, activities[0].activity_code);
            Assert.AreEqual(Logging.ACTIVITY_RESULT_CLOSED, activities[0].data["result"]);

            List<LogEventEntity> events = await slService.getActivityEventsAsync(activities[0]._id);
            Assert.AreEqual(6, events.Count());

            var ymlSer = new YamlDotNet.Serialization.Serializer();
            foreach (var evt in events)
            {
                Console.WriteLine(ymlSer.Serialize(evt));
            }
            Assert.AreEqual(Logging.LOG_TAG_DATA_PARSING, events[0].tag);
            Assert.AreEqual(Logging.LOG_EVENT_TYPE_DATA_PROCESSING, events[0].type);
            Assert.AreEqual(STEPID_PARSING_DATA, events[0].event_id);
            Assert.AreEqual(Logging.LV_INFO, events[0].lv);

            Assert.AreEqual(Logging.LOG_TAG_DATA_SAVING, events[1].tag);
            Assert.AreEqual(Logging.LOG_EVENT_TYPE_DATA_IO, events[1].type);
            Assert.AreEqual(STEPID_SAVING, events[1].event_id);
            Assert.AreEqual(Logging.LV_INFO, events[1].lv);
        
            Assert.AreEqual(Logging.LOG_TAG_STEP_FAILED, events[5].tag);
            Assert.AreEqual(Logging.LOG_EVENT_TYPE_USER_ACTION, events[5].type);
            Assert.AreEqual(STEPID_PARSING_DATA, events[5].event_id);
            Assert.AreEqual(Logging.LV_ERROR, events[5].lv);
        }
    }
}