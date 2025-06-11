using Malshinon.Generator;
using Malshinon.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.entityes
{
    public class People
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecretCode { get; set; }
        public string ManType { get; set; }
        public int NumReports { get; set; }
        public int NumMentions { get; set; }

        public override string ToString()
        {
            return $"person id: {this.id}\n" +
                $"first name: {this.FirstName}\n" +
                $" last name: {this.LastName}\n" +
                $"secret code: {this.SecretCode}\n" +
                $"type:{this.ManType}\n" +
                $"num reports:{this.NumReports}\n" +
                $"num mentions: {this.NumMentions}";
        }
    }
}
