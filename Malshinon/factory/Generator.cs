using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Malshinon.Generator
{
    public class GeneratorCode
    {
        private readonly Faker faker;
        public GeneratorCode()
        {
            this.faker = new Faker();
        }

        public string CodeGenerator()
        {
            string secretCode = faker.Random.AlphaNumeric(10);
            return secretCode;
        }
    
    }
}
