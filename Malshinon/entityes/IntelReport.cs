using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.entityes
{
    internal class IntelReport
    {
        private int id;
        private int ReporterId;
        private int TargetId;
        private string ReportTxt;
        private DateTime ReportTime;

        public IntelReport(int rid, int tid, string rtxt, DateTime timestamp = default)
        {
            this.ReporterId = rid;
            this.TargetId = tid;
            this.ReportTxt = rtxt;
            this.ReportTime = timestamp == default ? DateTime.Now : timestamp;
        }

        public int GetId() => this.id;
        public int GetReporterId() => this.ReporterId;
        public int GetTargetId() => this.TargetId;
        public string GetReportTxt() => this.ReportTxt;
        public DateTime GetReportTime() => this.ReportTime;
    }
}
