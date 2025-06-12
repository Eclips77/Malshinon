using Bogus;
using Malshinon.Dals;
using Malshinon.managers;
using Malshinon.Managers;
using System;

namespace Malshinon.menus
{
    public static  class MainMenu
    {
        public static void ShowMenu(ReportManager manager, PersonManager pmanager)
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("welcome to malshinon console!");
                Console.WriteLine("1. Add report");
                Console.WriteLine("2. Get person ");
                Console.WriteLine("3. Add person");
                Console.WriteLine("4. Advanced Intelligence Analysis");
                Console.WriteLine("5. exit");
                string choice = Console.ReadLine();
                Console.Clear();
                switch (choice)
                {
                    case "1":
                        manager.AddReportInteractive();
                        break;
                    case "2":
                        manager.PrintPersonById();
                        break;
                    case "3":
                        pmanager.HandleAddReporter();
                        break;
                    case "4":
                        new AnalysisMenu().ShowMenu();
                        break;
                    case "5":
                        Console.WriteLine("good bye :)");
                        running = false;
                        break;
                    default:
                        break;
                }
                Console.WriteLine("press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
