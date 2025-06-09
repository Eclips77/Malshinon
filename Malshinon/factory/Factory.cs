using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.entityes;
using Bogus;
namespace Malshinon.factory
{
    internal class Factory
    {
        private readonly Faker faker;
        public Factory()
        {
            this.faker = new Faker();
        }

        public People CreateNewAgent(string firstname,string lastname)
        {
            string firstName = firstname;
            string LastName = lastname;
            string secretCode = faker.Random.String2(8);
            return new People(firstName, LastName, secretCode);
        }
        public IntelReport CreateNewReport()
        {
            int reporterid = 0;
            int targetid = 0;
            string report = "";
            return new IntelReport(reporterid, targetid,report);
        }
    }
}
