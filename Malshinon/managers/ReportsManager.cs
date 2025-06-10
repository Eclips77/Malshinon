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
        private readonly Validator _validator;
        private readonly PersonService _personService;
        public  ValidateDal _validateDal;
        private readonly Dal _dal;
        private readonly GeneratorCode _codeGenerator;

        public ReportManager(
            Validator validator,
            PersonService personService,
            ValidateDal validateDal,
            Dal dal,
            GeneratorCode codeGenerator)
        {
            _validator = validator;
            _personService = personService;
            _validateDal = validateDal;
            _dal = dal;
            _codeGenerator = codeGenerator;
        }

        public void AddReportInteractive()
        {
            string reporterFirst = PromptName("Enter your first name:");
            string reporterLast = PromptName("Enter your last name:");
            int reporterId = EnsureReporterExists(reporterFirst, reporterLast);

            string reportText = Prompt("Enter your report:");
            var names = TextParser.ParseNames(reportText);
            if (names.fname.Length == 0)
            {
                Console.WriteLine("No target found inside the report text.");
                return;
            }

            int targetId = EnsureTargetExists(names.fname, names.lname);
            _dal.SetReportToDb(reporterId, targetId, reportText);
            Console.WriteLine("Report saved successfully.");
        }
        private int EnsureTargetExists(string firstName, string lastName)
        {
            if (_validateDal.ExistsInDatabase(firstName))
                return GetIdT(firstName);
                

            string code = _codeGenerator.CodeGenerator();
             _dal.setPersonToDb(firstName, lastName, code, "target");
            return GetIdT(firstName);

        }
        private int EnsureReporterExists(string firstName, string lastName)
        {
            if (_validateDal.ExistsInDatabase(firstName))
            {
                if (_validateDal.CheckStatus(firstName) == "target")
                    _dal.UpdateStatus(firstName, "both");
                return GetIdT(firstName);
            }
            string code = _codeGenerator.CodeGenerator();
            _dal.setPersonToDb(firstName, lastName, code, "reporter");
            return GetIdT(firstName);

        }
        private static string Prompt(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }
       
        private static string PromptName(string message)
        {
            string input;
            do
            {
                input = Prompt(message);
                if (string.IsNullOrWhiteSpace(input))
                    Console.WriteLine("name cannot be empty try again");
            } while (string.IsNullOrWhiteSpace(input));

            return CapitalizeFirstLetter(input);
        }

        public void updatePersonType()
        {

        }
        private static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = input.ToLower(); 
            return char.ToUpper(input[0]) + input.Substring(1);
        }
        public void PrintPersonById()
        {
            Console.Write("enter person id:");
            try
            {
                int id = Convert.ToInt32(Console.ReadLine());
                var person = _validateDal.GetPersonById(id);
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
