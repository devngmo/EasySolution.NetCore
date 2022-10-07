namespace EasySolution.NetCore.Shared.Commons
{
    public class Logging
    {
        public const int LV_ALL = 0;
        public const int LV_INFO = 1;
        public const int LV_DEBUG = 2;
        public const int LV_WARNING = 3;
        public const int LV_ERROR = 4;

        public const string LOG_EVENT_TYPE_ACTIVITY = "User.Activity";
        public const string LOG_EVENT_TYPE_USER_ACTION = "User.Action";
        public const string LOG_EVENT_TYPE_DATA_PROCESSING = "Data.Processing";
        public const string LOG_EVENT_TYPE_DATA_IO = "Data.IO";

        public const string ACTIVITY_START = "Activity.Start";
        public const string ACTIVITY_END = "Activity.End";

        public const string ACTIVITY_RESULT_SUCCESS = "success";
        public const string ACTIVITY_RESULT_FAILED = "failed";
        public const string ACTIVITY_RESULT_CLOSED = "closed";

        public const string LOG_TAG_ACTIVITY = "Activity";
        public const string LOG_TAG_STEP_PASSED = "Step.Passed";
        public const string LOG_TAG_STEP_FAILED = "Step.Failed";
        public const string LOG_TAG_DATA_PARSING = "Data.Parsing";
        public const string LOG_TAG_DATA_SAVING = "Data.Saving";
    }
}