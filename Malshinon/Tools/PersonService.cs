using Malshinon.Dals;
using Malshinon.entityes;
using Malshinon.Generator;
using Malshinon.Tools;
using System;

namespace Malshinon.Services
{
    public static class PersonService
    {
        //private readonly ValidateDal validator = new ValidateDal();
        private static readonly PersonDal _PersonDal = new PersonDal();

        public static int GetIdB(string firstName)
        {
            return _PersonDal.GetIdByName(firstName);
        }

        public static void AddNewReporterToDb()
        {
            string reporterFirst = Validator.PromptName("Enter your first name:");
            string reporterLast = Validator.PromptName("Enter your last name:");
            if (reporterFirst.Length >= 3 || reporterLast.Length >= 3)
            {
                if (!_PersonDal.ExistsInDatabase(reporterFirst))
                {
                    People p = new People {FirstName= reporterFirst,LastName = reporterLast,SecretCode=GeneratorCode.CodeGenerator(),ManType = "reporter" };
                    _PersonDal.InsertPersonToDb(p);
                    Console.WriteLine("added to db secssesfuly");
                }
            }
            else
            {
                Console.WriteLine("invalid names please enter your name again");
                return;
            }
        }
    }
}
