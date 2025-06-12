using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Malshinon.Generator
{
    public static class GeneratorCode
    {
        private static readonly Faker faker = new Faker();

        public static string CodeGenerator()
        {
            string secretCode = faker.Random.AlphaNumeric(10);
            return secretCode;
        }
    
    }
}
