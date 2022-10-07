using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySolution.NetCore.Smartlog.Entities
{
    public class LogEventEntity
    {
        public string _id { get; set; }
        public int lv { get; set; }
        public string msg { get; set; }
        public string tag { get; set; }
        public string type{ get; set; } 
        public string actor_id { get; set; }
        public string event_id { get; set; }
        public string device_id { get; set; }
        public string activity_code { get; set; }
        public DateTime at { get; set; }
        public Dictionary<string, object>? data { get; set; }

    }
}
