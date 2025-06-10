using Malshinon.Dals;
using Malshinon.Services;
using Malshinon.Tools;
using Malshinon.Generator;
using System;

namespace Malshinon.Managers
{
    public class ReportManager
    {
        private readonly Validator validator = new Validator();
        private readonly PersonService personService = new PersonService();
        private readonly ValidateDal validateDal = new ValidateDal();
        private readonly Dal dal = new Dal();
        private readonly GeneratorCode generator = new GeneratorCode();

        public ReportResult CreateReport(int reporterId, string reportText)
        {
            if (!IsValidReportText(reportText))
                return ReportResult.InvalidText;

            var names = ExtractTargetNames(reportText);
            if (names == null)
                return ReportResult.NoTargetFound;

            int targetId = EnsureTargetExists(names.Value.Item1, names.Value.Item2);
            SaveReportToDb(reporterId, targetId, reportText);

            return ReportResult.Success;
        }

        private bool IsValidReportText(string text)
        {
            return validator.ValidateCapitalLetter(text);
        }

        private (string, string)? ExtractTargetNames(string text)
        {
            return TextParser.ParseNames(text);
        }

        private int EnsureTargetExists(string firstName, string lastName)
        {
            if (validateDal.ExistsInDatabase(firstName))
                return personService.GetId(firstName);
            else
            {
                string code = generator.CodeGenerator();
                dal.setPersonToDb(firstName, lastName, code, "target");
                return personService.GetId(firstName);
            }
        }

        private void SaveReportToDb(int reporterId, int targetId, string text)
        {
            dal.SetReportToDb(reporterId, targetId, text);
        }

        public int idgive(string name)
        {
            return personService.GetId(name);
        }

        public void AddReport()
        {
            Console.WriteLine("Enter your first name:");
            string fname = Console.ReadLine();
            Console.WriteLine("Enter your last name:");
            string lname = Console.ReadLine();

            if (validateDal.ExistsInDatabase(fname))
            {
                if (validateDal.CheckStatus(fname) == "target")
                {
                    dal.UpdateStatus(fname, "both");
                }
            }
            else
            {
                string code = generator.CodeGenerator();
                dal.setPersonToDb(fname, lname, code, "reporter");
            }

            Console.WriteLine("Enter your report:");
            string report = Console.ReadLine();
            var details = TextParser.ParseNames(report);

            if (details.fname.Length > 0)
            {
                string targetFname = details.fname;
                string targetLname = details.lname;

                if (validateDal.ExistsInDatabase(targetFname))
                {
                    if (validateDal.CheckStatus(targetFname) == "reporter")
                    {
                        dal.UpdateStatus(targetFname, "both");
                    }
                }
                else
                {
                    string code = generator.CodeGenerator();
                    dal.setPersonToDb(targetFname, targetLname, code, "target");
                }

                int reporterId = personService.GetId(fname);
                int targetId = personService.GetId(targetFname);
                SaveReportToDb(reporterId, targetId, report);
            }
        }
    }

    public enum ReportResult
    {
        Success,
        InvalidText,
        NoTargetFound
    }
}
