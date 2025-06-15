using Malshinon.Dals;
using Malshinon.entityes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Malshinon.menus
{
    public class AnalysisMenu
    {
        private readonly AnalysisDal analysisDal = new AnalysisDal();
        private readonly PersonDal personDal = new PersonDal();
        private readonly ReportDal reportDal = new ReportDal();

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Intelligence Analysis Menu ===");
                Console.WriteLine("1. View Potential Agents");
                Console.WriteLine("2. View Dangerous Targets");
                Console.WriteLine("3. View Active Alerts");
                Console.WriteLine("4. View Reporter Details");
                Console.WriteLine("5. View Target Details");
                Console.WriteLine("0. Return to Main Menu");
                Console.Write("\nEnter your choice: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        ShowPotentialAgents();
                        break;
                    case 2:
                        ShowDangerousTargets();
                        break;
                    case 3:
                        ShowActiveAlerts();
                        break;
                    case 4:
                        ShowReporterDetails();
                        break;
                    case 5:
                        ShowTargetDetails();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void ShowPotentialAgents()
        {
            Console.Clear();
            Console.WriteLine("=== Potential Agents ===");
            var reporterStats = analysisDal.GetReporterStats();
            var potentialAgents = reporterStats.Where(s => s.ReportCount >= 10 && s.AverageLength >= 100);

            if (!potentialAgents.Any())
            {
                Console.WriteLine("No potential agents found.");
            }
            else
            {
                Console.WriteLine("\n{0,-20} {1,-10} {2,-15}", "Name", "Reports", "Avg Length");
                Console.WriteLine(new string('-', 45));
                foreach (var agent in potentialAgents)
                {
                    Console.WriteLine("{0,-20} {1,-10} {2,-15:F1}", 
                        agent.Name, 
                        agent.ReportCount, 
                        agent.AverageLength);
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void ShowDangerousTargets()
        {
            Console.Clear();
            Console.WriteLine("=== Dangerous Targets ===");
            var targetStats = analysisDal.GetTargetStats();
            var dangerousTargets = targetStats.Where(t => t.IsDangerous);

            if (!dangerousTargets.Any())
            {
                Console.WriteLine("No dangerous targets found.");
            }
            else
            {
                Console.WriteLine("\n{0,-20} {1,-10}", "Name", "Mentions");
                Console.WriteLine(new string('-', 30));
                foreach (var target in dangerousTargets)
                {
                    Console.WriteLine("{0,-20} {1,-10}", 
                        target.Name, 
                        target.MentionCount);
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void ShowActiveAlerts()
        {
            Console.Clear();
            Console.WriteLine("=== Active Alerts ===");
            var alerts = analysisDal.GetActiveAlerts();

            if (!alerts.Any())
            {
                Console.WriteLine("No active alerts found.");
            }
            else
            {
                Console.WriteLine("\n{0,-10} {1,-15} {2,-15} {3,-40} {4,-20}", "ID", "Reporter ID", "Target ID", "Reason", "Timestamp");
                Console.WriteLine(new string('-', 100));
                foreach (var alert in alerts.OrderByDescending(a => a.Timestamp))
                {
                    // To display Target Name if needed, fetch it from PersonDal using TargetId
                    // string targetName = personDal.GetPersonById(alert.TargetId)?.FirstName + " " + personDal.GetPersonById(alert.TargetId)?.LastName;

                    Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-40} {4,-20:yyyy-MM-dd HH:mm}",
                        alert.Id,
                        alert.ReporterId,
                        alert.TargetId,
                        alert.Reason,
                        alert.Timestamp);
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void ShowReporterDetails()
        {
            Console.Clear();
            Console.Write("Enter reporter name: ");
            string name = Console.ReadLine();

            var person = personDal.GetPersonByName(name);
            if (person == null)
            {
                Console.WriteLine("Reporter not found.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            var reports = reportDal.GetReportsByReporterId(person.id);
            Console.WriteLine($"\n=== Reporter Details: {person.FirstName} {person.LastName} ===");
            Console.WriteLine($"Total Reports: {person.NumReports}");
            Console.WriteLine($"Type: {person.ManType}");
            
            if (reports.Any())
            {
                Console.WriteLine("\nRecent Reports:");
                Console.WriteLine(new string('-', 80));
                foreach (var report in reports.Take(5))
                {
                    Console.WriteLine($"Target: {report.TargetName}");
                    Console.WriteLine($"Time: {report.Timestamp:yyyy-MM-dd HH:mm}");
                    Console.WriteLine($"Report: {report.ReportTxt}");
                    Console.WriteLine(new string('-', 80));
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void ShowTargetDetails()
        {
            Console.Clear();
            Console.Write("Enter target name: ");
            string name = Console.ReadLine();

            var person = personDal.GetPersonByName(name);
            if (person == null)
            {
                Console.WriteLine("Target not found.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            var reports = reportDal.GetReportsByTargetId(person.id);
            Console.WriteLine($"\n=== Target Details: {person.FirstName} {person.LastName} ===");
            Console.WriteLine($"Total Mentions: {person.NumMentions}");
            Console.WriteLine($"Type: {person.ManType}");
            Console.WriteLine($"Status: {(person.NumMentions >= 20 ? "Dangerous" : "Normal")}");

            if (reports.Any())
            {
                Console.WriteLine("\nRecent Reports:");
                Console.WriteLine(new string('-', 80));
                foreach (var report in reports.Take(5))
                {
                    Console.WriteLine($"Reporter: {report.ReporterName}");
                    Console.WriteLine($"Time: {report.Timestamp:yyyy-MM-dd HH:mm}");
                    Console.WriteLine($"Report: {report.ReportTxt}");
                    Console.WriteLine(new string('-', 80));
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
} 