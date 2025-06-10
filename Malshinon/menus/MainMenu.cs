using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Malshinon.Managers;

namespace Malshinon.menus
{
    internal class MainMenu
    {
        public void ShowMenu(ReportManager manager)
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("welcome to malshinon console!");
                Console.WriteLine("1. Add report");
                Console.WriteLine("2. Get person ");
                Console.WriteLine("3. Get reports");
                Console.WriteLine("4. exit");
                string choice = Console.ReadLine();
                //string name = Console.ReadLine();
                Console.Clear();
                switch (choice)
                {
                    case "1":
                        manager.AddReport();
                        break;
                    case "2":
                        
                        break;
                    case "3":
                        break;
                    case "4":
                        Console.WriteLine("good bye :)");
                        running = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
