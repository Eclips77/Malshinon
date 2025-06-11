using Malshinon.Dals;
using Malshinon.entityes;
using Malshinon.Generator;
using Malshinon.Services;
using Malshinon.Tools;
using System;

namespace Malshinon.Managers
{
    public class ReportManager
    {
        private readonly PersonService _personService;
        public ReportDal _reportDal;
        private readonly PersonDal _personDal;
        private readonly GeneratorCode _codeGenerator;

        public ReportManager(
            PersonService personService,
            ReportDal reportDal,
            PersonDal personDal,
            GeneratorCode codeGenerator)
        {
            _personService = personService;
            _reportDal = reportDal;
            _personDal = personDal;
            _codeGenerator = codeGenerator;
        }

        public void AddReportInteractive()
        {
            string reporterFirst = Validator.PromptName("Enter your first name:");
            string reporterLast = Validator.PromptName("Enter your last name:");
            int reporterId = EnsureReporterExists(reporterFirst, reporterLast);

            string reportText = Validator.Prompt("Enter your report:");
            var names = TextParser.ParseNames(reportText);
            if (names.fname.Length == 0)
            {
                Console.WriteLine("No target found inside the report text.");
                return;
            }

            int targetId = EnsureTargetExists(names.fname, names.lname);
            _reportDal.SetReportToDb(reporterId, targetId, reportText);
            Console.WriteLine("Report saved successfully.");
        }
        private int EnsureTargetExists(string firstName, string lastName)
        {
            if (_personDal.ExistsInDatabase(firstName))
                return GetIdT(firstName);
                

            string code = _codeGenerator.CodeGenerator();
            _personDal.setPersonToDb(firstName, lastName, code, "target");
            return GetIdT(firstName);

        }
        private int EnsureReporterExists(string firstName, string lastName)
        {
            if (_personDal.ExistsInDatabase(firstName))
            {
                if (_personDal.CheckStatus(firstName) == "target")
                    _personDal.UpdateStatus(firstName, "both");
                return GetIdT(firstName);
            }
            string code = _codeGenerator.CodeGenerator();
            _personDal.setPersonToDb(firstName, lastName, code, "reporter");
            return GetIdT(firstName);

        }
       
        public void updatePersonType()
        {

        }
     
        public void PrintPersonById()
        {
            Console.Write("enter person id:");
            try
            {
                int id = Convert.ToInt32(Console.ReadLine());
                var person = _personDal.GetPersonById(id);
                if (person == null)
                {
                    Console.WriteLine("person not found");
                }
                else
                {
                    Console.WriteLine(person);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"c# error in print by id {ex.Message}");
            }

        }
        public int GetIdT(string name) => _personService.GetIdB(name);

    }
}
