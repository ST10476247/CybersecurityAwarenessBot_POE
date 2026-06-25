using System;

namespace CyberBot.Models
{
    public class ActivityLogItem
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
