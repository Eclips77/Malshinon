using Malshinon.Dals;
using Malshinon.entityes;
using Malshinon.Generator;
using Malshinon.managers;
using Malshinon.Managers;
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

            ReportManager manager = new ReportManager(new ReportDal(), new PersonDal());
            PersonManager pmanager = new PersonManager(new PersonDal());
            MainMenu menu = new MainMenu();
            menu.ShowMenu(manager, pmanager);
            //f.PrintById(4);


        }
    }
}
