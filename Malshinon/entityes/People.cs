using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.interfaces
{
    internal class People
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string SecretCode { get; set; }
        string ManType { get; set; }
        int NumReports { get; set; }
        int NumMentions { get; set; }

        public People(string fn, string ln, string sc, string mt,int nr =0,int nm =0)
        {
            this.FirstName = fn;
            this.LastName = ln;
            this.SecretCode = sc;
            this.ManType = mt;
            this.NumReports = nr;
            this.NumMentions = nm;
        }
        public string GetFirstName() => this.FirstName;
        public string GetLastName() => this.LastName;
        public string GetSecretCode() => this.SecretCode;
        public string GetManType() => this.ManType;
        public int GetNumReports() => this.NumReports;
        public int GetNumMentions() => this.NumMentions;
    }
}
