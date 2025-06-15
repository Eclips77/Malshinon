using System;

namespace Malshinon.entityes
{
    public class Alert
    {
        public int Id { get; set; }
        public int ReporterId { get; set; }
        public int TargetId { get; set; }
        public string Reason { get; set; }
        public DateTime Timestamp { get; set; }
    }
} 