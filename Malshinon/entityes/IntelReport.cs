using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.entityes
{
    public class IntelReport
    {
        public int Id { get; set; }
        public int ReporterId { get; set; }
        public string ReporterName { get; set; }
        public int TargetId { get; set; }
        public string TargetName { get; set; }
        public string ReportTxt { get; set; }
        public DateTime Timestamp { get; set; }
        public override string ToString()
        {
            return $"report id: {this.Id}\n" +
                $"reporter id: {this.ReporterId}\n" +
                $"reporter name: {this.ReporterName}\n" +
                $"target id: {this.TargetId}\n" +
                $"target name: {this.TargetName}\n" +
                $"report txt: {this.ReportTxt}\n" +
                $"time stamp:{this.Timestamp}";
        }
    }
}
