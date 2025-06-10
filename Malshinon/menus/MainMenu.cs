using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.managers;

namespace Malshinon.menus
{
    internal class MainMenu
    {
        public void ShowMenu(MalshinManager manager)
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("welcome to malshinon console!");
                Console.WriteLine("1. get people");
                Console.WriteLine("2. set a new person");
                Console.WriteLine("3. set a new report");
                Console.WriteLine("4. exit");
                string choice = Console.ReadLine();
                Console.Clear();
                switch (choice)
                {
                    case "1":
                        break;
                    case "2":
                        manager.dal.setPersonToDb(manager.v.newPersoncheck());
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
