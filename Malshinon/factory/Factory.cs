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

        public People CreateNewAgent()
        {
            string firstName = faker.Name.FirstName();
            string LastName = faker.Name.FirstName();
            string secretCode = faker.Random.String2(8);
            string type = faker.PickRandom(new[] { "reporter", "target", "both", "potential_agent" });
            return new People(firstName, LastName, secretCode, type);
        }
    }
}
