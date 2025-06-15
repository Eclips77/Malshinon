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
        public ReportDal _reportDal;
        private readonly PersonDal _personDal;
        private readonly AnalysisDal _analysisDal;
        public ReportManager(
            ReportDal reportDal,
            PersonDal personDal,
            AnalysisDal analysisDal)
        {
            _reportDal = reportDal;
            _personDal = personDal;
            _analysisDal = analysisDal;
        }

        public void AddReportInteractive()
        {
            string reporterFirst = Validator.PromptName("Enter your first name:");
            string reporterLast = Validator.PromptName("Enter your last name:");
            int reporterId = EnsureReporterExists(reporterFirst, reporterLast);

            string reportText = Validator.Prompt("Enter your report:");
            var targetName = TextParser.ParseNames(reportText);
            if (targetName.fname.Length == 0 && targetName.lname.Length == 0)
            {
                Console.WriteLine("No target found inside the report text.");
                return;
            }
            else
            {
                int targetId = EnsureTargetExists(targetName.fname, targetName.lname);
                IntelReport report = new IntelReport { ReporterId = reporterId, TargetId = targetId, ReportTxt = reportText};
                _reportDal.SetReportToDb(report);
                Console.WriteLine("Report saved successfully.");
                _analysisDal.CheckForBurstAlerts(reporterId, targetId);
            }
        }
        private int EnsureTargetExists(string firstName, string lastName)
        {
            if (_personDal.ExistsInDatabase(firstName))
            {
                if (_personDal.GetPersonType(firstName) == "reporter")
                {
                    _personDal.SetPersonType(firstName, "both");
                }
            }
            else
            {
                People p = new People { FirstName = firstName, LastName = lastName, SecretCode = GeneratorCode.CodeGenerator(), ManType = "target" };
                _personDal.InsertPersonToDb(p);
            }
            return GetPersonIdByFirstName(firstName);
        }
        private int EnsureReporterExists(string firstName, string lastName)
        {
            if (_personDal.ExistsInDatabase(firstName))
            {
                if (_personDal.GetPersonType(firstName) == "target")
                {
                    _personDal.SetPersonType(firstName, "both");
                }
            }
            else
            {
                People newPerson = new People { FirstName = firstName, LastName = lastName, SecretCode = GeneratorCode.CodeGenerator(), ManType = "reporter" };
                _personDal.InsertPersonToDb(newPerson);
            }
            return GetPersonIdByFirstName(firstName);

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
        public int GetPersonIdByFirstName(string name) => PersonService.GetIdB(name);

    }
}
