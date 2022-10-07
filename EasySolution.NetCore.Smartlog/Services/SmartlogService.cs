using EasySolution.NetCore.Smartlog.Contexts;
using EasySolution.NetCore.Smartlog.Entities;
using EasySolution.NetCore.Smartlog.Repositories;

namespace EasySolution.NetCore.Smartlog.Services
{
    public class SmartlogService
    {
        readonly ActivityTrackingRepository _activityTrackingRepo;

        public SmartlogService(ActivityTrackingRepository activityTrackingRepository)
        {
            _activityTrackingRepo = activityTrackingRepository;
        }

        public async Task<List<LogEventEntity>> getActivitiesAsync(string actor_id, string? device_id)
        {
            return await _activityTrackingRepo.getActivitiesAsync(actor_id, device_id);
        }

        public async Task<List<LogEventEntity>> getActivityEventsAsync(string activity_id)
        {
            return await _activityTrackingRepo.getActivityEventsAsync(activity_id);
        }

        public string logActivityEvent(int level, string tag, string type, string actor_id, string device_id, string activity_code, string event_id, string msg, Dictionary<string, object>? data = null)
        {
            return _activityTrackingRepo.logActivityEvent(level, tag, type, actor_id, device_id, activity_code, event_id, msg, data);
        }

        public void Clear()
        {
            _activityTrackingRepo.Clear();
        }

        public ActivityTrackingLogContext beginActivity(string actor_id, string device_id, string activity_code)
        {
            string ins_id = _activityTrackingRepo.beginActivity(actor_id, device_id, activity_code);
            return new ActivityTrackingLogContext(actor_id, device_id, activity_code, ins_id, this);
        }

        public ActivityTrackingLogContext endActivity(string actor_id, string device_id, string activity_code, string activity_instance_id, string result)
        {
            string ins_id = _activityTrackingRepo.endActivity(actor_id, device_id, activity_code, activity_instance_id, result);
            return new ActivityTrackingLogContext(actor_id, device_id, activity_code, ins_id, this);
        }
    }
}