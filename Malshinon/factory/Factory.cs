using Bogus;
using Malshinon.entityes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
namespace Malshinon.factory
{
    internal class Factory
    {
        private readonly Faker faker;
        public Factory()
        {
            this.faker = new Faker();
        }

        public People CreateNewReporter(string firstname,string lastname)
        {
            string secretCode = faker.Random.String2(8);
            return new People(firstname, lastname, "reporter", secretCode);
        }
        public People CreateNewTarget(string firstname, string lastname)
        {
            string secretCode = faker.Random.String2(8);
            return new People(firstname, lastname,"target", secretCode);
        }
        public IntelReport CreateNewReport(int reporterId,int targetId,string text)
        {
            int reporterid = reporterId;
            int targetid = targetId;
            string report = text;
            return new IntelReport(reporterid, targetid,report);
        }
    }
}
