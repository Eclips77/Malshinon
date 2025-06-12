using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.entityes
{
    public class IntelReport
    {
        public int id { get; set; }
        public int ReporterId { get; set; }
        public int TargetId { get; set; }
        public string ReportTxt { get; set; }
        public DateTime ReportTime { get; set; }
        public override string ToString()
        {
            return $"report id: {this.id}\n" +
                $"reporter id: {this.ReporterId}\n" +
                $" target id: {this.TargetId}\n" +
                $"report txt: {this.ReportTxt}\n" +
                $"time stamp:{this.ReportTime}";
        }
   
    }
}
