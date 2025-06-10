using Bogus;
using Malshinon.Dals;
using Malshinon.managers;
using Malshinon.factory;
using Malshinon.menus;
using Malshinon.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon
{
    internal class Program
    {
        static void Main(string[] args)
        {
         
            MalshinManager manager = new MalshinManager();
            MainMenu menu = new MainMenu();
            menu.ShowMenu(manager);
            manager.dal.UpdateStatus("Leone","both");
          

            //Console.WriteLine(dod.SearchExist("Leone"));
        }
    }
}
