using EasySolution.NetCore.Smartlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySolution.NetCore.Smartlog.Repositories
{
    public interface IActivityTrackingRepositoryImplementor
    {
        Task<List<LogEventEntity>> getActivitiesAsync(string actor_id, string? device_id);
        Task<List<LogEventEntity>> getActivityEventsAsync(string activity_id);
        //string beginActivity(string actor_id, string device_id, string activity_code);
        //string endActivity(string actor_id, string device_id, string activity_code, string activity_instance_id);
        string logActivityEvent(int level, string tag, string type, string actor_id, string device_id, string activity_code, string event_id, string msg, Dictionary<string, object>? data = null);
        void Clear();
        void updateActivityResult(string actor_id, string device_id, string activity_code, string activity_instance_id, string result);
    }
}
