using System;

namespace Malshinon.entityes
{
    public class Alert
    {
        public int Id { get; set; }
        public int TargetId { get; set; }
        public string TargetName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Reason { get; set; }
        public DateTime Timestamp { get; set; }
    }
} 