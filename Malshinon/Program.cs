using Bogus;
using Malshinon.Dals;
using Malshinon.menus;
using Malshinon.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.Managers;

namespace Malshinon
{
    internal class Program
    {
        static void Main(string[] args)
        {
         
            ReportManager manager = new ReportManager();
            MainMenu menu = new MainMenu();
            menu.ShowMenu(manager);
            //manager.dal.UpdateStatus("Leone","both");
          

            //Console.WriteLine(dod.SearchExist("Leone"));
        }
    }
}
