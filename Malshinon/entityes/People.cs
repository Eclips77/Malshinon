using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.entityes
{
    internal class People
    {
        public int id { get; private set; }
        private string FirstName;
        private string LastName;
        private string SecretCode;
        private string ManType;
        private int NumReports;
        private int NumMentions;

        public People( string fname,string lname, string scode, string mtype, int id = 0, int nreports = 0,int nmentions =0)
        {
            this.id = id;
            this.FirstName = fname;
            this.LastName = lname;
            this.SecretCode = scode;
            this.ManType = mtype;
            this.NumReports = nreports;
            this.NumMentions = nmentions;
        }
        public int GetId() => this.id;
        public string GetFirstName() => this.FirstName;
        public string GetLastName() => this.LastName;
        public string GetSecretCode() => this.SecretCode;
        public string GetManType() => this.ManType;
        public int GetNumReports() => this.NumReports;
        public int GetNumMentions() => this.NumMentions;
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
