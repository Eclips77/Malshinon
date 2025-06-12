using Malshinon.Dals;
using Malshinon.entityes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Malshinon.menus
{
    public class AdvancedAnalysisMenu
    {
        private readonly AnalysisDal analysisDal = new AnalysisDal();
        private readonly PersonDal personDal = new PersonDal();
        private readonly ReportDal reportDal = new ReportDal();

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========== Intelligence Analysis & Reporting ==========");
                Console.WriteLine("1. List all potential agents");
                Console.WriteLine("2. List all dangerous targets");
                Console.WriteLine("3. List all active alerts");
                Console.WriteLine("4. Search reports by person");
                Console.WriteLine("5. View alerts for a person");
                Console.WriteLine("6. Reporter statistics");
                Console.WriteLine("7. Target statistics");
                Console.WriteLine("8. Promotion & flag logs");
                Console.WriteLine("0. Return to Main Menu");

                int choice;
                while (true)
                {
                    Console.Write("\nEnter your choice: ");
                    string input = Console.ReadLine();
                    if (!int.TryParse(input, out choice) || choice < 0 || choice > 8)
                    {
                        Console.WriteLine("Invalid choice. Please enter a number between 0 and 8.");
                        continue;
                    }
                    break;
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
                        SearchReportsByPerson();
                        break;
                    case 5:
                        ShowAlertsForPerson();
                        break;
                    case 6:
                        ShowReporterStats();
                        break;
                    case 7:
                        ShowTargetStats();
                        break;
                    case 8:
                        ShowPromotionAndFlagLogs();
                        break;
                    case 0:
                        return;
                }
            }
        }

        private void ShowPotentialAgents()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Potential Agents ===");
                
                // First promote any reporters that meet the criteria
                analysisDal.PromotePotentialAgents();
                
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
                    foreach (var agent in potentialAgents.OrderByDescending(a => a.ReportCount))
                    {
                        Console.WriteLine("{0,-20} {1,-10} {2,-15:F1}",
                            agent.Name,
                            agent.ReportCount,
                            agent.AverageLength);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AdvancedAnalysisMenu.ShowPotentialAgents] Error: {ex.Message}");
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
                foreach (var target in dangerousTargets.OrderByDescending(t => t.MentionCount))
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
                Console.WriteLine("\n{0,-20} {1,-20} {2,-30}", "Target", "Time Window", "Reason");
                Console.WriteLine(new string('-', 70));
                foreach (var alert in alerts.OrderByDescending(a => a.Timestamp))
                {
                    Console.WriteLine("{0,-20} {1,-20} {2,-30}",
                        alert.TargetName,
                        $"{alert.StartTime:yyyy-MM-dd HH:mm} - {alert.EndTime:HH:mm}",
                        alert.Reason);
                }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void SearchReportsByPerson()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Search Reports by Person ===");
                Console.Write("Enter person name (first or last): ");
                string name = Console.ReadLine();
                var person = personDal.GetPersonByName(name);
                if (person == null)
                {
                    Console.WriteLine("Person not found. Please enter a valid name.");
                    Console.WriteLine("Press any key to try again...");
                    Console.ReadKey();
                    continue;
                }
                Console.WriteLine($"\nReports as Reporter for {person.FirstName} {person.LastName}:");
                var reportsAsReporter = reportDal.GetReportsByReporterId(person.id);
                if (reportsAsReporter.Any())
                {
                    foreach (var report in reportsAsReporter)
                    {
                        Console.WriteLine($"[Target: {report.TargetName}] {report.Timestamp:yyyy-MM-dd HH:mm} - {report.ReportTxt}");
                    }
                }
                else
                {
                    Console.WriteLine("No reports as reporter.");
                }
                Console.WriteLine($"\nReports as Target for {person.FirstName} {person.LastName}:");
                var reportsAsTarget = reportDal.GetReportsByTargetId(person.id);
                if (reportsAsTarget.Any())
                {
                    foreach (var report in reportsAsTarget)
                    {
                        Console.WriteLine($"[Reporter: {report.ReporterName}] {report.Timestamp:yyyy-MM-dd HH:mm} - {report.ReportTxt}");
                    }
                }
                else
                {
                    Console.WriteLine("No reports as target.");
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                break;
            }
        }

        private void ShowAlertsForPerson()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Alerts for Person ===");
                Console.Write("Enter person name (first or last): ");
                string name = Console.ReadLine();
                var person = personDal.GetPersonByName(name);
                if (person == null)
                {
                    Console.WriteLine("Person not found. Please enter a valid name.");
                    Console.WriteLine("Press any key to try again...");
                    Console.ReadKey();
                    continue;
                }
                var alerts = analysisDal.GetActiveAlerts().Where(a => a.TargetId == person.id).ToList();
                if (!alerts.Any())
                {
                    Console.WriteLine("No alerts for this person.");
                }
                else
                {
                    Console.WriteLine("\n{0,-20} {1,-20} {2,-30}", "Target", "Time Window", "Reason");
                    Console.WriteLine(new string('-', 70));
                    foreach (var alert in alerts.OrderByDescending(a => a.Timestamp))
                    {
                        Console.WriteLine("{0,-20} {1,-20} {2,-30}",
                            alert.TargetName,
                            $"{alert.StartTime:yyyy-MM-dd HH:mm} - {alert.EndTime:HH:mm}",
                            alert.Reason);
                    }
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                break;
            }
        }

        private void ShowReporterStats()
        {
            Console.Clear();
            Console.WriteLine("=== Reporter Statistics ===");
            var stats = analysisDal.GetReporterStats();
            if (!stats.Any())
            {
                Console.WriteLine("No reporter statistics found.");
            }
            else
            {
                Console.WriteLine("\n{0,-20} {1,-10} {2,-15}", "Name", "Reports", "Avg Length");
                Console.WriteLine(new string('-', 45));
                foreach (var s in stats.OrderByDescending(s => s.ReportCount))
                {
                    Console.WriteLine("{0,-20} {1,-10} {2,-15:F1}",
                        s.Name,
                        s.ReportCount,
                        s.AverageLength);
                }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void ShowTargetStats()
        {
            Console.Clear();
            Console.WriteLine("=== Target Statistics ===");
            var stats = analysisDal.GetTargetStats();
            if (!stats.Any())
            {
                Console.WriteLine("No target statistics found.");
            }
            else
            {
                Console.WriteLine("\n{0,-20} {1,-10} {2,-10}", "Name", "Mentions", "Dangerous");
                Console.WriteLine(new string('-', 45));
                foreach (var s in stats.OrderByDescending(s => s.MentionCount))
                {
                    Console.WriteLine("{0,-20} {1,-10} {2,-10}",
                        s.Name,
                        s.MentionCount,
                        s.IsDangerous ? "YES" : "NO");
                }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void ShowPromotionAndFlagLogs()
        {
            Console.Clear();
            Console.WriteLine("=== Promotion & Flag Logs ===");
            // Placeholder: You can implement log reading from a file or DB if you store logs
            Console.WriteLine("(Logging functionality not implemented. Add your log source here.)");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
} 