using EasySolution.NetCore.Shared.Commons;
using EasySolution.NetCore.Smartlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySolution.NetCore.Smartlog.Repositories
{
    public class ActivityTrackingRepository
    {
        readonly IActivityTrackingRepositoryImplementor _implementor;
        public ActivityTrackingRepository(IActivityTrackingRepositoryImplementor implementor)
        {
            _implementor = implementor;
        }
        public async Task<List<LogEventEntity>> getActivitiesAsync(string actor_id, string? device_id)
        {
            return await _implementor.getActivitiesAsync(actor_id, device_id);
        }

        public async Task<List<LogEventEntity>> getActivityEventsAsync(string activity_id)
        {
            return await _implementor.getActivityEventsAsync(activity_id);
        }

        public string beginActivity(string actor_id, string device_id, string activity_code)
        {
            //return _implementor.beginActivity(actor_id, device_id, activity_code);
            return logActivityEvent(Logging.LV_INFO, Logging.LOG_TAG_ACTIVITY, Logging.LOG_EVENT_TYPE_ACTIVITY, 
                actor_id, device_id, activity_code, Logging.ACTIVITY_START, "Activity start");
        }

        internal void Clear()
        {
            _implementor.Clear();
        }

        public string endActivity(string actor_id, string device_id, string activity_code, string activity_instance_id, string result)
        {
            _implementor.updateActivityResult(actor_id, device_id, activity_code, activity_instance_id, result);
            //return _implementor.endActivity(actor_id, device_id, activity_code, activity_instance_id);
            return logActivityEvent(Logging.LV_INFO, Logging.LOG_TAG_ACTIVITY, Logging.LOG_EVENT_TYPE_ACTIVITY,
                actor_id, device_id, activity_code, Logging.ACTIVITY_END, activity_instance_id);
        }

        public string logActivityEvent(int level, string tag, string type, string actor_id, string device_id, string activity_code, string event_id, string msg, Dictionary<string, object>? data = null)
        {
            return _implementor.logActivityEvent(level, tag, type, actor_id, device_id, activity_code, event_id, msg, data);
        }
    }
}
