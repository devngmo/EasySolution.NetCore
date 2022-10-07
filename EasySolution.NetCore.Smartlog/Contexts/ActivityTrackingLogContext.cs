using EasySolution.NetCore.Shared.Commons;
using EasySolution.NetCore.Smartlog.Entities;
using EasySolution.NetCore.Smartlog.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySolution.NetCore.Smartlog.Contexts
{
    public class ActivityTrackingLogContext
    {
        string _actor, _activity_code, _ins_id, _device_id;
        readonly SmartlogService _service;
        public ActivityTrackingLogContext(string actor_id, string device_id, string activity_code, string activity_instance_id, SmartlogService service)
        {
            _actor = actor_id;
            _device_id = device_id;
            _service = service;
            _activity_code = activity_code;
            _ins_id= activity_instance_id;
        }

        public void onStepPassed(string stepCode, string msg, Dictionary<string, object>? data = null)
        {
            _service.logActivityEvent(Logging.LV_DEBUG, Logging.LOG_TAG_STEP_PASSED, 
                Logging.LOG_EVENT_TYPE_USER_ACTION, _actor, _device_id, _ins_id, stepCode, msg, data);
        }

        public void endActivity(string result)
        {
            _service.endActivity(_actor, _device_id, _activity_code, _ins_id, result);
        }

        public void onStepFailed(string stepCode, string msg, Dictionary<string, object>? data = null)
        {
            _service.logActivityEvent(Logging.LV_ERROR, Logging.LOG_TAG_STEP_FAILED, Logging.LOG_EVENT_TYPE_USER_ACTION, _actor, _device_id, _ins_id, stepCode, msg, data);
        }

        public void dataParsingFailed(string event_id, string msg, Dictionary<string, object>? data = null)
        {
            _service.logActivityEvent(Logging.LV_ERROR, Logging.LOG_TAG_DATA_PARSING, Logging.LOG_EVENT_TYPE_DATA_PROCESSING,
                _actor, _device_id, _ins_id,
                event_id, msg, data);
        }
        public void dataParsingSuccess(string event_id, string msg, Dictionary<string, object>? data = null)
        {
            _service.logActivityEvent(Logging.LV_INFO, Logging.LOG_TAG_DATA_PARSING, Logging.LOG_EVENT_TYPE_DATA_PROCESSING,
                _actor, _device_id, _ins_id,
                event_id, msg, data);
        }
        public void dataSavingFailed(string event_id, string msg, Dictionary<string, object>? data = null)
        {
            _service.logActivityEvent(Logging.LV_ERROR, Logging.LOG_TAG_DATA_SAVING, Logging.LOG_EVENT_TYPE_DATA_IO,
                _actor, _device_id, _ins_id,
                event_id, msg, data);
        }
        public void dataSavingSuccess(string event_id, string msg, Dictionary<string, object>? data = null)
        {
            _service.logActivityEvent(Logging.LV_INFO, Logging.LOG_TAG_DATA_SAVING, Logging.LOG_EVENT_TYPE_DATA_IO,
                _actor, _device_id, _ins_id,
                event_id, msg, data);
        }
    }
}
