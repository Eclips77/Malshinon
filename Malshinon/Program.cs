using Malshinon.Generator;
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
         
            ReportManager manager = new ReportManager(new Services.PersonService(),new ReportDal(),new PersonDal(),new GeneratorCode());
            MainMenu menu = new MainMenu();
            menu.ShowMenu(manager);
            //ValidateDal f = new ValidateDal();
            //f.PrintById(4);


            //Console.WriteLine(dod.SearchExist("Leone"));
        }
    }
}
