using Malshinon.Dals;
using Malshinon.entityes;
using Malshinon.factory;
using Malshinon.Services;

//using Malshinon.Logging;
//using Malshinon.Models;
//using Malshinon.Services;
using Malshinon.Tools;
using System;

namespace Malshinon.Managers
{
    public class ReportManager
    {
        private readonly Validator validator = new Validator();
        private readonly TextParser textParser = new TextParser();
        private readonly PersonService personService = new PersonService();
        private readonly Factory reportFactory = new Factory();
        private readonly ValidateDal reportDal = new ValidateDal();
        private readonly Dal dal = new Dal();

        public ReportResult CreateReport(int reporterId, string reportText)
        {

            if (!IsValidReportText(reportText))
                return ReportResult.InvalidText;

            var names = ExtractTargetNames(reportText);
            if (names == null)
                return ReportResult.NoTargetFound;

            int targetId = EnsureTargetExists(names.Value.Item1, names.Value.Item2);
            IntelReport report = BuildReport(reporterId, targetId, reportText);
            SaveReport(report);

            return ReportResult.Success;
        }

        private bool IsValidReportText(string text)
        {
            return validator.ValidateCapitalLetter(text);
        }

        private (string, string)? ExtractTargetNames(string text)
        {
            return textParser.ParseNames(text);
        }

        private int EnsureTargetExists(string firstName, string lastName)
        {
            if (reportDal.SearchExist(firstName))
                return personService.GetId(firstName);
            else
                personService.CreateTarget(firstName, lastName);
            return 1;
        }

        private IntelReport BuildReport(int reporterId, int targetId, string text)
        {
            return reportFactory.CreateNewReport(reporterId, targetId, text);
        }

        private void SaveReport(IntelReport report)
        {
            dal.SetReportToDb(report);
        }
        public int idgive(string name)
        {
            return personService.GetId(name);
        }
        public void AddReport()
        {
            Console.WriteLine("enter yout first name:");
            string fname = Console.ReadLine();
            if (reportDal.SearchExist(fname))
            {
                if (reportDal.CheckStatus(fname) == "target")
                {
                    dal.UpdateStatus(fname,"both");
                }
            }
            else
            {
                dal.setPersonToDb()
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
